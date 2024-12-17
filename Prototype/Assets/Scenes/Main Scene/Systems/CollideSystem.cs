using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;

namespace Scenes.Main_Scene
{
    partial struct CollideSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            RefRW<EndSimulationEntityCommandBufferSystem.Singleton> ecb = SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            var killArcherJob = new KillArcherJob
            {
                ecb = ecb.ValueRW.CreateCommandBuffer(state.WorldUnmanaged),
                allBoulders = SystemAPI.GetComponentLookup<BoulderTag>(true),
                allProjectiles = SystemAPI.GetComponentLookup<ProjectileTag>(true),
                allMasses = SystemAPI.GetComponentLookup<SetPhysicsMass>(),
                PhysicsMassData = SystemAPI.GetComponentLookup<PhysicsMass>(),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            killArcherJob.Complete();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

    [BurstCompile]
    struct KillArcherJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<BoulderTag> allBoulders;
        [ReadOnly] public ComponentLookup<ProjectileTag> allProjectiles;
        public ComponentLookup<SetPhysicsMass> allMasses;
        public ComponentLookup<PhysicsMass> PhysicsMassData;
        public EntityCommandBuffer ecb;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (allBoulders.HasComponent(entityA) && allMasses.HasComponent(entityB))
            {
                var massComponent = allMasses[entityB];
                var inertiaComponent = PhysicsMassData[entityB];
                inertiaComponent.InverseInertia = new float3(massComponent.BaselineInertiaX,
                    massComponent.BaselineInertiaY, massComponent.BaselineInertiaZ);
                PhysicsMassData[entityB] = inertiaComponent;
                massComponent.Alive = false;
                massComponent.InfiniteInertiaX = false;
                massComponent.InfiniteInertiaZ = false;
                allMasses[entityB] = massComponent;
                ecb.RemoveComponent<IsAlive>(entityB);
            }

            else if (allBoulders.HasComponent(entityB) && allMasses.HasComponent(entityA))
            {
                var massComponent = allMasses[entityA];
                var inertiaComponent = PhysicsMassData[entityA];
                inertiaComponent.InverseInertia = new float3(massComponent.BaselineInertiaX,
                    massComponent.BaselineInertiaY, massComponent.BaselineInertiaZ);
                PhysicsMassData[entityA] = inertiaComponent;
                massComponent.Alive = false;
                massComponent.InfiniteInertiaX = false;
                massComponent.InfiniteInertiaZ = false;
                allMasses[entityA] = massComponent;
                ecb.RemoveComponent<IsAlive>(entityA);
            }
            
            else if (allProjectiles.HasComponent(entityA) && allMasses.HasComponent(entityB))
            {
                var massComponent = allMasses[entityB];
                var inertiaComponent = PhysicsMassData[entityB];
                inertiaComponent.InverseInertia = new float3(massComponent.BaselineInertiaX,
                    massComponent.BaselineInertiaY, massComponent.BaselineInertiaZ);
                PhysicsMassData[entityB] = inertiaComponent;
                massComponent.Alive = false;
                massComponent.InfiniteInertiaX = false;
                massComponent.InfiniteInertiaZ = false;
                allMasses[entityB] = massComponent;
                ecb.RemoveComponent<IsAlive>(entityB);
                ecb.DestroyEntity(entityA); //can consider this, it removes a bullet if it touches an archer
            }
            else if (allProjectiles.HasComponent(entityB) && allMasses.HasComponent(entityA))
            {
                var massComponent = allMasses[entityA];
                var inertiaComponent = PhysicsMassData[entityA];
                inertiaComponent.InverseInertia = new float3(massComponent.BaselineInertiaX,
                    massComponent.BaselineInertiaY, massComponent.BaselineInertiaZ);
                PhysicsMassData[entityA] = inertiaComponent;
                massComponent.Alive = false;
                massComponent.InfiniteInertiaX = false;
                massComponent.InfiniteInertiaZ = false;
                allMasses[entityA] = massComponent;
                ecb.RemoveComponent<IsAlive>(entityA);
                ecb.DestroyEntity(entityB);
            }
        }
    }
}