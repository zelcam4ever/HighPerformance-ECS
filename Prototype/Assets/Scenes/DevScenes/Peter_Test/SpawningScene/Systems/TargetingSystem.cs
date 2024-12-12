using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    [UpdateAfter(typeof(Config))]
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
            //var config = SystemAPI.GetSingleton<Config>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<Config>();
            
            // It seems wasteful to me that we allocate new space for the native arrays on every frame, but I have tried
            // with the persistent allocator as well in an (if IsCreated) statement and the performance is identical? Return later
            // also there is most likely a much nicer way to do this...
            if (!RedPositions.IsCreated)
            {
                switch (config.BattleSize)
                {
                    case BattleSize.Tens:
                        RedPositions = new NativeArray<float3>(75, Allocator.Persistent);
                        BluePositions = new NativeArray<float3>(75, Allocator.Persistent);
                        NearestTargetPositions = new NativeArray<float3>(75, Allocator.Persistent);
                        break;
                    case BattleSize.Hundreds:
                        RedPositions = new NativeArray<float3>(250, Allocator.Persistent);
                        BluePositions = new NativeArray<float3>(250, Allocator.Persistent);
                        NearestTargetPositions = new NativeArray<float3>(250, Allocator.Persistent);
                        break;
                    case BattleSize.Thousands:
                        RedPositions = new NativeArray<float3>(2500, Allocator.Persistent);
                        BluePositions = new NativeArray<float3>(2500, Allocator.Persistent);
                        NearestTargetPositions = new NativeArray<float3>(2500, Allocator.Persistent);
                        break;
                }
            }
            
            int count = 0;
            foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RedTag>())
            {
                RedPositions[count] = archerTransform.ValueRO.Position;
                count++;
            }

            count = 0;
            foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueTag>())
            {
                BluePositions[count] = archerTransform.ValueRO.Position;
                count++;
            }

            if (SystemAPI.Time.ElapsedTime - elapsedTime > 2)
            {
                switch (config.SchedulingType)
                {
                    case SchedulingType.Schedule:
                        state.Dependency = new FindNearestRed
                        {
                            RedPositions = RedPositions,
                        }.Schedule(state.Dependency);
                        state.Dependency = new FindNearestBlue
                        {
                            BluePositions = BluePositions,
                        }.Schedule(state.Dependency);
                        break;

                    case SchedulingType.ScheduleParallel:
                        FindNearestJob findRedTargetsJob = new FindNearestJob
                        {
                            RedPositions = RedPositions,
                            BluePositions = BluePositions,
                            NearestTargetPositions = NearestTargetPositions
                        };
                        
                        JobHandle findNearestRedHandle = findRedTargetsJob.Schedule(RedPositions.Length, 64);
                        findNearestRedHandle.Complete();
                        
                        FindNearestJob findBlueTargetsJob = new FindNearestJob
                        {
                            RedPositions = BluePositions,
                            BluePositions = RedPositions,
                            NearestTargetPositions = NearestTargetPositions
                        };
                        
                        JobHandle findNearestBlueHandle = findBlueTargetsJob.Schedule(RedPositions.Length, 64);
                        findNearestBlueHandle.Complete();
                        // state.Dependency = new FindNearestRed
                        // {
                        //     RedPositions = RedPositions,
                        // }.ScheduleParallel(state.Dependency);
                        // state.Dependency = new FindNearestBlue
                        // {
                        //     BluePositions = BluePositions,
                        // }.ScheduleParallel(state.Dependency);

                        break;
                }
                
                elapsedTime = SystemAPI.Time.ElapsedTime;
            }
            // for (int i = 0; i < RedPositions.Length; i++)
            // {
            //     Debug.DrawLine(RedPositions[i], NearestTargetPositions[i]);
            // }
            // for (int i = 0; i < BluePositions.Length; i++)
            // {
            //     Debug.DrawLine(BluePositions[i], NearestTargetPositions[i]);
            // }
            // if (RedPositions.IsCreated)
            // {
            //     RedPositions.Dispose();
            //     BluePositions.Dispose();
            //     NearestTargetPositions.Dispose();
            // }
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
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> RedPositions;
        [ReadOnly] public NativeArray<float3> BluePositions;
        public NativeArray<float3> NearestTargetPositions;

        public void Execute(int index)
        {
            float3 redPos = RedPositions[index];
            float nearestDistSq = float.MaxValue;
            
            for (int i = 0; i < BluePositions.Length; i++)
            {
                float3 targetPos = BluePositions[i];
                float distSq = math.distance(redPos, targetPos);
                
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    NearestTargetPositions[index] = targetPos;
                }
            }
        }
    }
}