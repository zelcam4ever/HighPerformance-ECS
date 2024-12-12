using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Scenes.Main_Scene
{
    partial struct TargetingSystem : ISystem
    {
        private NativeArray<float3> RedPositions;
        private NativeArray<float3> BluePositions;
        NativeArray<float3> NearestTargetPositions;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            //var config = SystemAPI.GetSingleton<Config>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<Config>();
            // It seems wasteful to me that we allocate new space for the native arrays on every frame, but I have tried
            // with the persistent allocator as well in an (if IsCreated) statement and the performance is identical? Return later
            // also there is most likely a much nicer way to do this...
            switch (config.BattleSize)
            {
                case BattleSize.Tens:
                    RedPositions = new NativeArray<float3>(75, Allocator.TempJob);
                    BluePositions = new NativeArray<float3>(75, Allocator.TempJob);
                    NearestTargetPositions = new NativeArray<float3>(75, Allocator.TempJob);
                    break;
                case BattleSize.Hundreds:
                    RedPositions = new NativeArray<float3>(250, Allocator.TempJob);
                    BluePositions = new NativeArray<float3>(250, Allocator.TempJob);
                    NearestTargetPositions = new NativeArray<float3>(250, Allocator.TempJob);
                    break;
                case BattleSize.Thousands:
                    RedPositions = new NativeArray<float3>(2500, Allocator.TempJob);
                    BluePositions = new NativeArray<float3>(2500, Allocator.TempJob);
                    NearestTargetPositions = new NativeArray<float3>(2500, Allocator.TempJob);
                    break;
            }
            
            
            int count = 0;
            foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RedTag>())
            {
                RedPositions[count] = archerTransform.ValueRO.Position;
                //Debug.Log("Red:" + RedPositions[count]);
                count++;
            }

            count = 0;
            foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueTag>())
            {
                BluePositions[count] = archerTransform.ValueRO.Position;
                //Debug.Log("Blue:" + BluePositions[count]);
                count++;
            }
        
            FindNearestJob findRedTargetsJob = new FindNearestJob
            {
                RedPositions = RedPositions,
                BluePositions = BluePositions,
                NearestTargetPositions = NearestTargetPositions
            };
            
            JobHandle findNearestRedHandle = findRedTargetsJob.Schedule(RedPositions.Length, 100);
            findNearestRedHandle.Complete();
            
            int index = 0;
            foreach (var archer in SystemAPI.Query<RefRW<Archer>,RefRO<LocalTransform>>().WithAll<RedTag>())
            {
                archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];
                Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.red);
                index++;
            };
            
            FindNearestJob findBlueTargetsJob = new FindNearestJob
            {
                RedPositions = BluePositions,
                BluePositions = RedPositions,
                NearestTargetPositions = NearestTargetPositions
            };
            
            JobHandle findNearestBlueHandle = findBlueTargetsJob.Schedule(RedPositions.Length, 100);
            findNearestBlueHandle.Complete();
            index = 0;
            foreach (var archer in SystemAPI.Query<RefRW<Archer>,RefRO<LocalTransform>>().WithAll<BlueTag>())
            {
                archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];
                Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.blue);
                index++;
            };
            
            
            RedPositions.Dispose();
            BluePositions.Dispose();
            NearestTargetPositions.Dispose();
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