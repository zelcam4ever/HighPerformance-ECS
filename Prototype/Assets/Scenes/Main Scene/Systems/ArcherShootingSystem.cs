
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
using static Unity.Entities.SystemAPI;
using Time = UnityEngine.Time;

namespace Scenes.Main_Scene
{
    public partial struct ArcherShootingSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _localTransformLookup;
        private ComponentLookup<LocalToWorld> _localToWorldLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<ArcherShooting>();
            _localTransformLookup = state.GetComponentLookup<LocalTransform>(true);
            _localToWorldLookup = state.GetComponentLookup<LocalToWorld>(true); // Set as ReadOnly
        }
            
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _localTransformLookup.Update(ref state);
            _localToWorldLookup.Update(ref state);
            var config = GetSingleton<Config>();
            // Cache current frame count for this update

            var ecbSingleton = GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            // Schedule the shooting job
            switch (config.SchedulingType)
            {
                case SchedulingType.ScheduleParallel:
                    state.Dependency = new ShootingJob
                    {
                        ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                        LocalTransformLookup = _localTransformLookup,
                        LocalToWorldLookup = _localToWorldLookup,
                        DeltaTime = Time.deltaTime
                    }.ScheduleParallel(state.Dependency);
                    break;
                
                case SchedulingType.Schedule:
                    state.Dependency = new ShootingJob
                    {
                        ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                        LocalTransformLookup = _localTransformLookup,
                        LocalToWorldLookup = _localToWorldLookup,
                        DeltaTime = Time.deltaTime
                    }.Schedule(state.Dependency);
                    break;
            }
        }
        
        [BurstCompile]
        public partial struct ShootingJob : IJobEntity
        {
            public float DeltaTime;
            public EntityCommandBuffer.ParallelWriter ECB;
            [ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;
            [ReadOnly] public ComponentLookup<LocalToWorld> LocalToWorldLookup;
            public void Execute([ChunkIndexInQuery] int entityInQueryIndex, ref Archer archer,  in IsAlive isAlive)
            {
                const float g = 9.8f;
                
                if (!LocalTransformLookup.TryGetComponent(archer.Aimer, out var aimer))
                    return;
                if (!LocalToWorldLookup.TryGetComponent(archer.SpawnPoint, out var spawnPoint))
                    return;
                
                
                float3 startPosition = spawnPoint.Position;
                float3 targetPosition = archer.TargetPosition;

                var v0 = archer.Strength;                
                
                var x = math.length(new float3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z));
                var h = startPosition.y - targetPosition.y;
                var t = math.atan2(x, h);
                var a = g * math.pow(x, 2) / math.pow(v0, 2);
                var denominator = math.sqrt(math.pow(h, 2) + math.pow(x, 2));
                var cos = (a - h) / denominator;
                if (cos is < -1 or > 1)
                {
                    //Target is unreachable!
                    return;
                }
                var total =math.acos(cos);
                var angle = 1.570f -((total + t) / 2);
                //Debug.Log(math.degrees(angle));
                
                quaternion aimRotation = quaternion.RotateX(angle);
                ECB.SetComponent(entityInQueryIndex, archer.Aimer, new LocalTransform
                {
                    Position = aimer.Position, // Keep the position (or modify as needed)
                    Rotation = aimRotation,
                    Scale = 1f // Set scale (modify if required)
                });
                aimer.Rotation = aimRotation;

           
                
                archer.CurrentTimeToShoot += DeltaTime;

                if (archer.CurrentTimeToShoot < archer.TimerReload){
                    return;
                }
                archer.CurrentTimeToShoot = 0;
     
                // Get the spawn point and projectile prefab
                // Get the spawn point's LocalToWorld
                 // Skip if spawn point data is missing


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
