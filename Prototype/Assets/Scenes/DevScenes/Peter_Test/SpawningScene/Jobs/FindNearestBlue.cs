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
    [WithAll(typeof(RedTag))]
    public partial struct FindNearestBlue : IJobEntity
    {
        [ReadOnly] public NativeArray<float3> BluePositions;
        void Execute(in LocalTransform redTransform)
        {
            float nearestDistSq = float.MaxValue;
            float3 targetPos = BluePositions[0];
            for (int i = 0; i < BluePositions.Length; i++)
            {
                float3 potentialTargetPos = BluePositions[i];
                float distSq = math.distance(redTransform.Position, potentialTargetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    targetPos = potentialTargetPos; 
                }
            }
            Debug.DrawLine(redTransform.Position, targetPos, Color.red, duration: 2);
        }
    }
}
