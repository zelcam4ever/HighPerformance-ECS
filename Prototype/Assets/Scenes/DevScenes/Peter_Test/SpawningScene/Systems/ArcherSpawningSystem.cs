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
            
            int redFormation = (int)math.sqrt(config.RedArcherCount);
            int blueFormation = (int)math.sqrt(config.BlueArcherCount);

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
                }
            }
            
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
                }
            }
        }

        [BurstCompile]
        public void OnDestroy()
        {
        }
    }
}
