using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Scenes.Main_Scene
{
    [UpdateAfter(typeof(ArcherSpawningSystem))]
    partial struct NewTargetingSystem : ISystem
    {
        private NativeArray<float3> RedPositions;
        private NativeArray<float3> BluePositions;
        private double elapsedTime;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<EnableTargeting>();
            elapsedTime = SystemAPI.Time.ElapsedTime;
            RedPositions = new NativeArray<float3>(22500, Allocator.Persistent);
            BluePositions = new NativeArray<float3>(22500, Allocator.Persistent);
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            if (SystemAPI.Time.ElapsedTime - elapsedTime > 2.0f)
            {
                var config = SystemAPI.GetSingleton<Config>();
                
                int redCount = 0;
                foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RedTag, IsAlive>())
                {
                    RedPositions[redCount] = archerTransform.ValueRO.Position;
                    redCount++;
                }

                int blueCount = 0;
                foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueTag, IsAlive>())
                {
                    BluePositions[blueCount] = archerTransform.ValueRO.Position;
                    blueCount++;
                }

                switch (config.SchedulingType)
                {
                    case SchedulingType.Schedule:
                        state.Dependency = new FindNearestBlue
                        {
                            blueCount = blueCount,
                            BluePositions = BluePositions,
                        }.Schedule(state.Dependency);
                        state.Dependency = new FindNearestRed
                        {
                            redCount = redCount,
                            RedPositions = RedPositions,
                        }.Schedule(state.Dependency);
                        break;
                    case SchedulingType.ScheduleParallel:
                        state.Dependency = new FindNearestBlue
                        {
                            blueCount = blueCount,
                            BluePositions = BluePositions,
                        }.ScheduleParallel(state.Dependency);
                        state.Dependency = new FindNearestRed
                        {
                            redCount = redCount,
                            RedPositions = RedPositions,
                        }.ScheduleParallel(state.Dependency);
                        break;
                }
                elapsedTime = SystemAPI.Time.ElapsedTime;
            }
        }
    
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            if (RedPositions.IsCreated)
            {
                RedPositions.Dispose();
                BluePositions.Dispose();
            }
        }
    }
}