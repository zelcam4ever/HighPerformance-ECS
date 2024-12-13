using System.Threading;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.Main_Scene
{
    partial struct BoulderSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            
            var config = SystemAPI.GetSingleton<Config>();

            
            for (int i = 0; i < 10; i++)
            {
                float3 position = new float3(500 + 10*i, 50, 15 -3*i);
                
                var boulderInstance = state.EntityManager.Instantiate(config.BigBoulderPrefab);
                state.EntityManager.SetComponentData(boulderInstance, new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.identity,
                    Scale = 3.0f
                });
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}