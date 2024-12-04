using Scenes.DevScenes.Lucas_Tests.SpawningScene.Authorings;
using Scenes.DevScenes.Lucas_Tests.SpawningScene.Config;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems
{
    public partial struct ArcherProjectileCollisionSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _localTransformLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ArrowPhysics>();
            _localTransformLookup = state.GetComponentLookup<LocalTransform>(true);
            
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Update component lookups
            _localTransformLookup.Update(ref state);

            // Get the end simulation ECB
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            // Schedule the collision job
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld.CollisionWorld;

            state.Dependency = new CollisionJob
            {
                LocalTransformLookup = _localTransformLookup,
                ECB = ecb
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
        [BurstCompile]
        private struct CollisionJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            public EntityCommandBuffer ECB;

            public void Execute(CollisionEvent collisionEvent)
            {
                // Get the entities involved in the collision
                var entityA = collisionEvent.EntityA;
                var entityB = collisionEvent.EntityB;

                // Determine if one entity is a projectile and the other an archer
                bool isProjectileA = LocalTransformLookup.HasComponent(entityA);
                bool isArcherB = LocalTransformLookup.HasComponent(entityB);

                if (isProjectileA && isArcherB)
                {
                    HandleCollision(entityA, entityB);
                }
                else if (!isProjectileA && !isArcherB)
                {
                    HandleCollision(entityB, entityA);
                }
            }

            private void HandleCollision(Entity projectile, Entity archer)
            {
                Debug.Log($"Projectile {projectile} hit Archer {archer}");
                // Stick projectile to archer
                var archerTransform = LocalTransformLookup[archer];
                ECB.SetComponent(projectile, new LocalTransform
                {
                    Position = archerTransform.Position,
                    Rotation = quaternion.identity, // Adjust as needed
                    Scale = LocalTransformLookup[projectile].Scale // Or any scale logic you want
                });

                // Mark the archer as dead
                ECB.RemoveComponent<IsAlive>(archer);
                ECB.RemoveComponent<PhysicsVelocity>(projectile);
            }
        }
    }
}
