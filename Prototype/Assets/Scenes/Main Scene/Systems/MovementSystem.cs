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
    partial struct MovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<Config>();
            switch (config.SchedulingType)
            {
                case SchedulingType.Schedule:
                    state.Dependency = new MovementJob{dt = SystemAPI.Time.DeltaTime}.Schedule(state.Dependency);
                    break;
                
                case SchedulingType.ScheduleParallel:
                    state.Dependency = new MovementJob{dt = SystemAPI.Time.DeltaTime}.ScheduleParallel(state.Dependency);
                    break;
            }
        }
        
    }
    [BurstCompile]
    [WithAll(typeof(RedTag), typeof(IsAlive))]
    public partial struct MovementJob : IJobEntity
    {
        public float dt;
        void Execute(ref LocalTransform transform)
        {
            transform.Position += new float3(0, 0, -2 * dt);
        }
    }
}
