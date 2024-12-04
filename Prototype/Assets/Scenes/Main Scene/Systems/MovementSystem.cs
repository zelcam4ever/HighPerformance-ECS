using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Scenes.Main_Scene
{
    [UpdateAfter(typeof(SetPhysicsMassBehaviourSystem))]
    partial struct MovementSystem : ISystem
    {
        // private uint startseed;
        // private Random rng;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            // startseed = 1;
            // rng = new Random(startseed);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //state.Enabled = false;
            foreach (var (archerVelocity, physicsMass)  in SystemAPI.Query<RefRW<PhysicsVelocity>,RefRO<SetPhysicsMass>>().WithAll<RedTag>())
            {
                if (physicsMass.ValueRO.Alive)
                {
                    archerVelocity.ValueRW.Linear[0] = 1;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}
