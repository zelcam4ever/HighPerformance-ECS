using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    partial struct BuildingSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<SpawnBuildings>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();

            for (int i = 0; i < 1; i++)
            {

                float3 position = new float3(0, 100, 1000 * i);

                var wallInstance = state.EntityManager.Instantiate(config.WallPrefab);
                state.EntityManager.SetComponentData(wallInstance, new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.identity,
                    Scale = 1.0f
                });
            }
        }
    }
}