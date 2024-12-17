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
    [WithAll(typeof(RedTag), typeof(IsAlive))]
    public partial struct FindNearestBlue : IJobEntity
    {
        [ReadOnly] public NativeArray<float3> BluePositions;
        public int blueCount;
        void Execute(ref LocalTransform redTransform, ref Archer archer)
        {
            //find nearest red
            float nearestDistSq = float.MaxValue;
            float3 targetPos = BluePositions[0];
            for (int i = 0; i < blueCount; i++)
            {
                float3 potentialTargetPos = BluePositions[i];
                float distSq = math.distance(redTransform.Position, potentialTargetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    targetPos = potentialTargetPos; 
                }
            }
            //set archers target
            archer.TargetPosition = targetPos;
            //turn archer towards target
            float3 pos = redTransform.Position;
            pos.y = 0;
                    
            float3 pos2 = targetPos;
            pos2.y = 0;
                    
            quaternion end = quaternion.LookRotation(pos2 - pos, math.up());
            redTransform.Rotation = end;
            //draw debug line
            Debug.DrawLine(redTransform.Position, targetPos, Color.red, duration: 2);
        }
    }
}
