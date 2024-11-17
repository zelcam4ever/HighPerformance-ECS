using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    partial struct TargetingSystem : ISystem
    {
        NativeArray<float3> ArcherPositions;
        NativeArray<float3> TargetPositions;
        NativeArray<float3> NearestTargetPositions;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var archerTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<RedTag>())
            {
                archerTransform.ValueRW.Position += new float3(0.5f * SystemAPI.Time.DeltaTime, 0, 0.5f * SystemAPI.Time.DeltaTime);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
