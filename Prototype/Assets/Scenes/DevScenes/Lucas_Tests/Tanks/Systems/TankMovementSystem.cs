using Scenes.DevScenes.Lucas_Tests.Tanks.Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Systems
{
    public partial struct TankMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TankMovement>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
 
            var dt = SystemAPI.Time.DeltaTime;
            /*
             QUESTION:
             Does this loop over every entity with a LocalTransform and then every entity with a Tank component?
             ANSWER:
             Not sure yet if it loops over those, but I don't think so.
             The system only seems to affect the Tank entity during testing.
             */
            // Loop over transforms and tanks
            foreach( var (transform, tank, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Tank>>().WithAll<Tank>().WithEntityAccess())
            {
                var pos = transform.ValueRO.Position;
                
                pos.y = (float)entity.Index;
                var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
                var dir = float3.zero;
                math.sincos(angle, out dir.x, out dir.z);

                transform.ValueRW.Position += dir * dt * 5.0f * tank.ValueRO.Speed;
                transform.ValueRW.Rotation = quaternion.RotateY(angle);
            }
        }
    }
}
