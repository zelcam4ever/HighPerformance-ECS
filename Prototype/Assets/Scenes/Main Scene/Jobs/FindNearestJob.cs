using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace Scenes.Main_Scene
{
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> AimerPositions;
        [ReadOnly] public NativeArray<float3> TargetPositions;
        public NativeArray<float3> NearestTargetPositions;
        public int targetCount;

        public void Execute(int index)
        {
            float3 aimerPos = AimerPositions[index];
            float nearestDistSq = float.MaxValue;
            float3 zeroVec = new float3(0, 0, 0);
            
            for (int i = 0; i < targetCount; i++)
            {
                float3 targetPos = TargetPositions[i];
                float distSq = math.distance(aimerPos, targetPos);
                
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
