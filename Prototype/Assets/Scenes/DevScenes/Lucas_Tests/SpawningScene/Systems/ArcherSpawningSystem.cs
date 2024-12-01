using Scenes.DevScenes.Lucas_Tests.SpawningScene.Config;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems
{
    partial struct ArcherSpawningSystem : ISystem
    {
        private bool hasSpawned;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnArchers>();
            state.RequireForUpdate<Config.Config>();
            hasSpawned = false;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!hasSpawned && Input.GetKeyDown(KeyCode.G))
            {
                var config = SystemAPI.GetSingleton<Config.Config>();

                // Determine the total number of archers per battalion
                int archersPerBattalion = math.min(config.ArcherCount / config.NumberOfBattalions, 100);
                int totalBattalions = (int)math.ceil(config.ArcherCount / (float)archersPerBattalion);

                for (int b = 0; b < totalBattalions; b++)
                {
                    // Calculate the formation size (e.g., square root for roughly square formation)
                    int columns = (int)math.ceil(math.sqrt(archersPerBattalion));
                    int rows = (int)math.ceil(archersPerBattalion / (float)columns);
                    Debug.Log($"Num Columns: {columns}, Num Rows: {rows}");
                    for (int i = 0; i < archersPerBattalion; i++)
                    {
                        int x = i % columns;
                        int y = i / columns;

                        // Calculate position offset within each battalion
                        float3 position = new float3(x * 1.5f, 0.0f, y * 1.5f) + new float3(b * 20, 0.0f, 0.0f);

                        // Instantiate the Archer at the calculated position
                        var archerInstance = state.EntityManager.Instantiate(config.ArcherPrefab);
                        state.EntityManager.SetComponentData(archerInstance, new LocalTransform
                        {
                            Position = position,
                            Rotation = quaternion.identity,
                            Scale = 1.0f
                        });
                    }
                }

                hasSpawned = true; // Prevent multiple spawns
            }
        }
    }
}
