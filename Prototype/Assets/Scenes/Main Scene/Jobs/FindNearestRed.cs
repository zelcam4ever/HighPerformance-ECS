using Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems;
using Scenes.Main_Scene;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.Main_Scene
{
    [BurstCompile]
    [WithAll(typeof(BlueTag), typeof(IsAlive))]
    public partial struct FindNearestRed : IJobEntity
    {
        [ReadOnly] public NativeArray<float3> RedPositions;
        public int redCount;
        [ReadOnly] public Config Config;
        void Execute(ref LocalTransform blueTransform, ref Archer archer)
        {
            //find nearest red
            float nearestDistSq = float.MaxValue;
            float3 targetPos = RedPositions[0];
            for (int i = 0; i < redCount; i++)
            {
                float3 potentialTargetPos = RedPositions[i];
                float distSq = math.distance(blueTransform.Position, potentialTargetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    targetPos = potentialTargetPos; 
                }
            }
            //set archers target
            archer.TargetPosition = targetPos;
            //turn archer towards target
            float3 pos = blueTransform.Position;
            pos.y = 0;
                    
            float3 pos2 = targetPos;
            pos2.y = 0;
                    
            quaternion end = quaternion.LookRotation(pos2 - pos, math.up());
            blueTransform.Rotation = end;
            //draw debug line
            if(Config.EnableTargetingDebug)
                Debug.DrawLine(blueTransform.Position, targetPos, Color.blue, duration: 2);
        }
    }
}
