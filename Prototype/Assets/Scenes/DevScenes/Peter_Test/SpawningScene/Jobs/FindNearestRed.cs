using Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    [BurstCompile]
    [WithAll(typeof(BlueTag))]
    public partial struct FindNearestRed : IJobEntity
    {
        [ReadOnly] public NativeArray<float3> RedPositions;
        void Execute(in LocalTransform blueTransform)
        {
            float nearestDistSq = float.MaxValue;
            float3 targetPos = RedPositions[0];
            for (int i = 0; i < RedPositions.Length; i++)
            {
                float3 potentialTargetPos = RedPositions[i];
                float distSq = math.distance(blueTransform.Position, potentialTargetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    targetPos = potentialTargetPos; 
                }
            }
            Debug.DrawLine(blueTransform.Position, targetPos);
        }
    }
}
