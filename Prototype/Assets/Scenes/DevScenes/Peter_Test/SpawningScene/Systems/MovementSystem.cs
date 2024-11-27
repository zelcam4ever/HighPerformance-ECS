using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    partial struct MovementSystem : ISystem
    {
        private uint startseed;
        private Random rng;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            startseed = 1;
            rng = new Random(startseed);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var archerTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<RedTag>())
            {
                archerTransform.ValueRW.Position += new float3(rng.NextFloat(-1.0f, 1.0f) * SystemAPI.Time.DeltaTime, 0, rng.NextFloat(-2.0f, 1.0f) * SystemAPI.Time.DeltaTime);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}
