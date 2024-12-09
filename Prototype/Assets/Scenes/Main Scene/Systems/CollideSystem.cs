using Scenes.DevScenes.Peter_Test.SpawningScene;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenes.Main_Scene
{
    partial struct CollideSystem : ISystem
    {


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var triggerJob = new TriggerJob
            {
                allBoulders = SystemAPI.GetComponentLookup<BoulderTag>(true),
                allProjectiles = SystemAPI.GetComponentLookup<ProjectileTag>(true),
                allMasses = SystemAPI.GetComponentLookup<SetPhysicsMass>(),
                PhysicsMassData = SystemAPI.GetComponentLookup<PhysicsMass>(),
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            triggerJob.Complete();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }

    [BurstCompile]
    struct TriggerJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<BoulderTag> allBoulders;
        [ReadOnly] public ComponentLookup<ProjectileTag> allProjectiles;
        public ComponentLookup<SetPhysicsMass> allMasses;
        public ComponentLookup<PhysicsMass> PhysicsMassData;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if ((allBoulders.HasComponent(entityA) || allProjectiles.HasComponent(entityA)) && allMasses.HasComponent(entityB))
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
            }

            else if ((allBoulders.HasComponent(entityB) || allProjectiles.HasComponent(entityB)) && allMasses.HasComponent(entityA))
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
            }
        }
    }
}