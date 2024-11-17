using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    partial struct ArcherSpawningSystem : ISystem
    {
        NativeArray<float3> RedTransforms;
        NativeArray<float3> BlueTransforms;
        Config config;
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
            
            config = SystemAPI.GetSingleton<Config>();

            // Determine the total number of archers per battalion
            // int archersPerBattalion = math.min(config.ArcherCount / config.NumberOfBattalions, 100);
            // int totalBattalions = (int)math.ceil(config.ArcherCount / (float)archersPerBattalion);

            int redFormation = (int)math.sqrt(config.RedArcherCount);
            int blueFormation = (int)math.sqrt(config.BlueArcherCount);
            RedTransforms = new NativeArray<float3>(config.RedArcherCount, Allocator.Persistent);
            BlueTransforms = new NativeArray<float3>(config.BlueArcherCount, Allocator.Persistent);
            int count = 0;

            for (int c = 0; c < redFormation; c++)
            {
                for (int r = 0; r < redFormation; r++)
                {
                    float3 position = new float3(c * 1.5f, 0, r * 1.5f);
                    
                    var archerInstance = state.EntityManager.Instantiate(config.RedArcherPrefab);
                    state.EntityManager.SetComponentData(archerInstance, new LocalTransform
                    {
                        Position = position,
                        Rotation = quaternion.identity,
                        Scale = 1.0f
                    });
                    RedTransforms[count] = position;
                    count++;
                }
            }

            count = 0;
            for (int c = 0; c < blueFormation; c++)
            {
                for (int r = 0; r < blueFormation; r++)
                {
                    float3 position = new float3(c * 1.5f + 30, 0, r * 1.5f + 10);
                    
                    var archerInstance = state.EntityManager.Instantiate(config.BlueArcherPrefab);
                    state.EntityManager.SetComponentData(archerInstance, new LocalTransform
                    {
                        Position = position,
                        Rotation = quaternion.identity,
                        Scale = 1.0f
                    });
                    BlueTransforms[count] = position;
                    count++;
                }
            
            }
        }

        public void OnDestroy()
        {
            RedTransforms.Dispose();
            BlueTransforms.Dispose();
        }
    }
}
