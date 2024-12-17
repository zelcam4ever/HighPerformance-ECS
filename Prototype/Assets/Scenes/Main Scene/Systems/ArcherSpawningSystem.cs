using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scenes.Main_Scene
{
    partial struct ArcherSpawningSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnArchers>();
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            
            var config = SystemAPI.GetSingleton<Config>();

            foreach (var blueSpawnPoint in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueSpawnPoint>())
            {
                for (int c = 0; c < 5; c++)
                {
                    for (int r = 0; r < 5; r++)
                    {
                        float3 position = new float3(c * 1.5f, 0, r * 1.5f) + blueSpawnPoint.ValueRO.Position; //Creates formation offsets
                        var archerInstance = state.EntityManager.Instantiate(config.BlueArcherPrefab);
                        state.EntityManager.SetComponentData(archerInstance, new LocalTransform
                        {
                            Position = position,
                            Rotation = quaternion.identity,
                            Scale = 1.0f
                        });
                    }
                }
            }
            
            foreach (var redSpawnPoint in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RedSpawnPoint>())
            {
                for (int c = 0; c < 5; c++)
                {
                    for (int r = 0; r < 5; r++)
                    {
                        float3 position = new float3(c * 1.5f, 0, r * 1.5f) + redSpawnPoint.ValueRO.Position; //Creates formation offsets
                        float3 offset = new float3(0, 0, 40f);
                        var archerInstance = state.EntityManager.Instantiate(config.RedArcherPrefab);
                        quaternion rotation = quaternion.Euler(0f, math.radians(180f), 0f);
                        
                        state.EntityManager.SetComponentData(archerInstance, new LocalTransform
                        {
                            Position = position + offset,
                            Rotation = rotation,
                            Scale = 1.0f
                        });
                    }
                }
            }
        }
    }
}
