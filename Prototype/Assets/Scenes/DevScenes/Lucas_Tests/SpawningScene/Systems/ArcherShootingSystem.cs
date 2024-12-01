using Scenes.DevScenes.Lucas_Tests.SpawningScene.Config;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems
{
    public partial struct ArcherShootingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ArcherShooting>();
            // System setup, if needed
        }

        public void OnUpdate(ref SystemState state)
        {
            // Iterate over all archers
            foreach (var (archer, archerTransform) in SystemAPI.Query<RefRO<Authorings.Archer>,RefRO<LocalToWorld>>())
            {
                if (Time.frameCount % (60*5) == 0) // Shoot roughly every 5th second 
                {
                    var spawnPoint = archer.ValueRO.SpawnPoint;
                 

                    // Instantiate projectile
                    var projectile = state.EntityManager.Instantiate(archer.ValueRO.ProjectilePrefab);

                    // Set projectile position and rotation
                    state.EntityManager.SetComponentData(projectile, new LocalTransform
                    {
                        Position = SystemAPI.GetComponent<LocalToWorld>(spawnPoint).Position,
                        Rotation = quaternion.identity,
                        Scale = SystemAPI.GetComponent<LocalTransform>(archer.ValueRO.ProjectilePrefab).Scale
                    });

                    // Add velocity to the projectile if it has a PhysicsVelocity component
                    if (state.EntityManager.HasComponent<PhysicsVelocity>(projectile))
                    {
                        float3 shootDirection = math.mul(SystemAPI.GetComponent<LocalToWorld>(spawnPoint).Rotation, new float3(0, 1, 0)); // Adjust to the correct direction
                      

                        state.EntityManager.SetComponentData(projectile, new PhysicsVelocity
                        {
                            Linear = shootDirection * archer.ValueRO.Strength,
                            Angular = float3.zero
                        });
                        // Debug.Log("Something" + state.EntityManager.GetComponentData<PhysicsVelocity>(projectile).Linear);
                    }
                }
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            // Cleanup if needed
        }
    }
}
