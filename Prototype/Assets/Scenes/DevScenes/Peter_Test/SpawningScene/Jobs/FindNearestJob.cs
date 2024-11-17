using Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> RedPositions;
        [ReadOnly] public NativeArray<float3> BluePositions;
        public NativeArray<float3> NearestTargetPositions;

        public void Execute(int index)
        {
            float3 redPos = RedPositions[index];
            //Debug.Log("Red Pos" + redPos);
            float nearestDistSq = float.MaxValue;
            for (int i = 0; i < BluePositions.Length; i++)
            {
                float3 targetPos = BluePositions[i];
                //Debug.Log("Blue Pos" + BluePositions[i]);
                float distSq = math.distance(redPos, targetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    NearestTargetPositions[index] = targetPos;
                    //Debug.Log("Target Pos" + NearestTargetPositions[index]);
                    
                }

                
            }
        }
    }
}
