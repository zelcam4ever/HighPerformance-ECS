using Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Scenes.DevScenes.Peter_Test.SpawningScene.Jobs
{
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> ArcherPositions;
        public NativeArray<float3> NearestTargetPositions;

        public void Execute(int index)
        {
            float3 archerPos = ArcherPositions[index];
            float nearestDistSq = float.MaxValue;
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                float3 targetPos = TargetPositions[i];
                float distSq = math.distance(archerPos, targetPos);
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    NearestTargetPositions[index] = targetPos;
                }
            }
        }
    }
}
