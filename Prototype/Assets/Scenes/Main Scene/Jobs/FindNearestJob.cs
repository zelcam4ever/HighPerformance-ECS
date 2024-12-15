using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Scenes.Main_Scene
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
            float nearestDistSq = float.MaxValue;
            float3 zeroVec = new float3(0, 0, 0);
            
            for (int i = 0; i < BluePositions.Length; i++)
            {
                float3 targetPos = BluePositions[i];
                float distSq = math.distance(redPos, targetPos);
                
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    if (!targetPos.Equals(zeroVec))
                    {
                        NearestTargetPositions[index] = targetPos;
                    }
                }
            }
        }
    }
}
