
using Scenes.DevScenes.Lucas_Tests.SpawningScene.Authorings;
using Scenes.DevScenes.Lucas_Tests.SpawningScene.Config;
using Unity.Burst;
using Unity.Collections;
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
        private ComponentLookup<LocalTransform> _localTransformLookup;
        private ComponentLookup<LocalToWorld> _localToWorldLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ArcherShooting>();
            _localTransformLookup = state.GetComponentLookup<LocalTransform>(true);
            _localToWorldLookup = state.GetComponentLookup<LocalToWorld>(true); // Set as ReadOnly
        }
            
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _localTransformLookup.Update(ref state);
            _localToWorldLookup.Update(ref state);
            // Cache current frame count for this update
            int currentFrame = Time.frameCount;

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            // Schedule the shooting job
            state.Dependency = new ShootingJob
            {
                CurrentFrame = currentFrame,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                LocalTransformLookup = _localTransformLookup,
                LocalToWorldLookup = _localToWorldLookup
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        public partial struct ShootingJob : IJobEntity
        {
            public int CurrentFrame;
            public EntityCommandBuffer.ParallelWriter ECB;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> LocalToWorldLookup;

            public void Execute(Entity entity, [EntityIndexInQuery] int entityInQueryIndex, in Authorings.Archer archer,  in IsAlive isAlive)
            {
                // Only shoot every 5 seconds
                if (CurrentFrame % (60 * 5) != 0) return;

                // Get the spawn point and projectile prefab
                // Get the spawn point's LocalToWorld
                if (!LocalToWorldLookup.TryGetComponent(archer.SpawnPoint, out var spawnPoint))
                    return; // Skip if spawn point data is missing


                // Instantiate the projectile
                var projectile = ECB.Instantiate(entityInQueryIndex, archer.ProjectilePrefab);
         

                // Set projectile's transform
                ECB.SetComponent(entityInQueryIndex, projectile, new LocalTransform
                {
                    Position = spawnPoint.Position,
                    Rotation = quaternion.identity,
                    Scale = LocalTransformLookup[archer.ProjectilePrefab].Scale
                });

                // Apply velocity to the projectile
                float3 shootDirection = math.mul(spawnPoint.Rotation, new float3(0, 1, 0)); // Shooting along Y-axis
                ECB.SetComponent(entityInQueryIndex, projectile, new PhysicsVelocity
                {
                    Linear = math.normalize(shootDirection) * archer.Strength,
                    Angular = float3.zero
                });
            }
        }
    }
}
