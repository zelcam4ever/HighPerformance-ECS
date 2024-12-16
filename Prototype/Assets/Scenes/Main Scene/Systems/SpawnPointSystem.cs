using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.Main_Scene
{
    partial struct SpawnPointSystem : ISystem
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
            switch (config.BattleSize)
            {
                case BattleSize.Tens:
                    for (int i = 0; i < 3; i++)
                    {
                        float3 position = new float3(i * 10.0f, 0, 0); //Can be changed to fit castle locations
                        float3 offset = new float3(0, 0, 10.0f);
                        var blueSpawnPoint = state.EntityManager.Instantiate(config.BlueSpawnPoint);
                        state.EntityManager.SetComponentData(blueSpawnPoint, new LocalTransform
                        {
                            Position = position,
                            Scale = 1.0f
                        });
                        var redSpawnPoint = state.EntityManager.Instantiate(config.RedSpawnPoint);
                        state.EntityManager.SetComponentData(redSpawnPoint, new LocalTransform
                        {
                            Position = position + offset,
                            Scale = 1.0f
                        });
                    }
                    break;
                case BattleSize.Hundreds:
                    for (int i = 0; i < 10; i++)
                    {
                        float3 position = new float3(i * 10.0f, 0, 0); //Can be changed to fit castle locations
                        float3 offset = new float3(0, 0, 10.0f);
                        var blueSpawnPoint = state.EntityManager.Instantiate(config.BlueSpawnPoint);
                        state.EntityManager.SetComponentData(blueSpawnPoint, new LocalTransform
                        {
                            Position = position,
                            Scale = 1.0f
                        });
                        var redSpawnPoint = state.EntityManager.Instantiate(config.RedSpawnPoint);
                        state.EntityManager.SetComponentData(redSpawnPoint, new LocalTransform
                        {
                            Position = position + offset,
                            Scale = 1.0f
                        });
                    }
                    break;
                case BattleSize.Thousands:
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            float3 position = new float3(i * 10.0f, 0, j * -10.0f); //Can be changed to fit castle locations
                            float3 offset = new float3(0, 0, 100.0f);
                            var blueSpawnPoint = state.EntityManager.Instantiate(config.BlueSpawnPoint);
                            state.EntityManager.SetComponentData(blueSpawnPoint, new LocalTransform
                            {
                                Position = position,
                                Scale = 1.0f
                            });
                            var redSpawnPoint = state.EntityManager.Instantiate(config.RedSpawnPoint);
                            state.EntityManager.SetComponentData(redSpawnPoint, new LocalTransform
                            {
                                Position = position + offset,
                                Scale = 1.0f
                            });
                        }
                    }
                    break;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
