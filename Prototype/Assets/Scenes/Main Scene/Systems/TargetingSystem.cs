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
    partial struct TargetingSystem : ISystem
    {
        private NativeArray<float3> RedPositions;
        private NativeArray<float3> BluePositions;
        NativeArray<float3> NearestTargetPositions;
        private double elapsedTime;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            elapsedTime = SystemAPI.Time.ElapsedTime;
            RedPositions = new NativeArray<float3>(22500, Allocator.Persistent);
            BluePositions = new NativeArray<float3>(22500, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(22500, Allocator.Persistent);
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

                FindNearestJob findRedTargetsJob = new FindNearestJob
                {
                    targetCount = blueCount,
                    AimerPositions = RedPositions,
                    TargetPositions = BluePositions,
                    NearestTargetPositions = NearestTargetPositions
                };

                JobHandle findNearestRedHandle = findRedTargetsJob.Schedule(redCount, 64);
                findNearestRedHandle.Complete();

                int index = 0;
                foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRW<LocalTransform>>()
                             .WithAll<RedTag, IsAlive>())
                {
                    archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];

                    if (config.EnableTargetingDebug)
                    {
                        Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.red, duration: 2.0f);
                        float3 pos = archer.Item2.ValueRO.Position;
                        pos.y = 0;
                    
                        float3 pos2 = NearestTargetPositions[index];
                        pos2.y = 0;
                    
                        quaternion end = quaternion.LookRotation(pos2 - pos, math.up());
                        archer.Item2.ValueRW.Rotation = end;
                    }
                    index++;
                }

                FindNearestJob findBlueTargetsJob = new FindNearestJob
                {
                    targetCount = redCount,
                    AimerPositions = BluePositions,
                    TargetPositions = RedPositions,
                    NearestTargetPositions = NearestTargetPositions
                };

                JobHandle findNearestBlueHandle = findBlueTargetsJob.Schedule(blueCount, 64);
                findNearestBlueHandle.Complete();
                index = 0;
                foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRW<LocalTransform>>()
                             .WithAll<BlueTag, IsAlive>())
                {
                    archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];
                    if (config.EnableTargetingDebug)
                    {
                        Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.blue, duration: 2.0f);
                        float3 pos = archer.Item2.ValueRO.Position;
                        pos.y = 0;
                    
                        float3 pos2 = NearestTargetPositions[index];
                        pos2.y = 0;
                    
                        quaternion end = quaternion.LookRotation(pos2 - pos, math.up());
                        archer.Item2.ValueRW.Rotation = end;
                    }
                    index++;
                };

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
                NearestTargetPositions.Dispose();
            }
        }
    }
}