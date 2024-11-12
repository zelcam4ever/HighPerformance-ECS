using Scenes.DevScenes.Lucas_Tests.Tanks.Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Systems
{
    partial struct ProjectilePhysicsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ProjectilePhysics>();
            // state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // This system handles the playback and disposal of the ECB at the End of the Simulation.
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var projectileJob = new ProjectileJob
            {
                // The "WorldUnmanaged" allocator will automatically be disposed after each update of the world.
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                DeltaTime = SystemAPI.Time.DeltaTime   
            };
            projectileJob.Schedule();
        }
        [BurstCompile] 
        public partial struct ProjectileJob : IJobEntity
        {
            public EntityCommandBuffer ECB;
            public float DeltaTime;

            void Execute(Entity ent, ref Projectile projectile, ref LocalTransform localTransform)
            {
                var gravity = new float3(0.0f, -9.82f, 0.0f);
                var invertY = new float3(1.0f, -1.0f, 1.0f);
                
                localTransform.Position += projectile.Velocity * DeltaTime;

                if (localTransform.Position.y < 0.0f)
                {
                    localTransform.Position *= invertY;
                    projectile.Velocity *= invertY * 0.8f; // Invert velocity multiplied by some restitution
                }
                projectile.Velocity += gravity * DeltaTime;
                
                
                if (math.lengthsq(projectile.Velocity) < 0.1f)
                {
                    // Structural change operations can usually only be done on the main thread and not in jobs.
                    // To circumvent this, we use the ECB to record the command of destroying the projectile entity,
                    // which is then played back later.
                    ECB.DestroyEntity(ent);
                }
            }
        }
    }
}
