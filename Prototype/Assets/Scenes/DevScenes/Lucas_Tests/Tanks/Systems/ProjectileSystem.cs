using System;
using Scenes.DevScenes.Lucas_Tests.Tanks.Aspects;
using Scenes.DevScenes.Lucas_Tests.Tanks.Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Systems
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct ProjectileSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Shooting>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (turret, localToWorld) 
                     in SystemAPI.Query<TurretAspect, RefRO<LocalToWorld>>())
            {
                // Create new projectile instance every frame
                Entity instance = state.EntityManager.Instantiate(turret.Projectile);
                
                state.EntityManager.SetComponentData(instance, new LocalTransform
                {
                    Position = SystemAPI.GetComponent<LocalToWorld>(turret.SpawnPoint).Position,
                    Rotation = quaternion.identity,
                    Scale = SystemAPI.GetComponent<LocalTransform>(turret.Projectile).Scale
                });
                state.EntityManager.SetComponentData(instance, new Projectile
                {
                    Velocity = localToWorld.ValueRO.Up * 20.0f
                });
                
                state.EntityManager.SetComponentData(instance, new URPMaterialPropertyBaseColor {Value = turret.Color});
            }
        }
        
    }
}
