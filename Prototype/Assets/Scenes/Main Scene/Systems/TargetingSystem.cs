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
        private double elapsedTime;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            elapsedTime = SystemAPI.Time.ElapsedTime;
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
                    RedPositions[count] = archerTransform.ValueRO.Position;
                    count++;
                }

                count = 0;
                foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueTag, IsAlive>())
                {
                    BluePositions[count] = archerTransform.ValueRO.Position;
                    count++;
                }

                FindNearestJob findRedTargetsJob = new FindNearestJob
                {
                    RedPositions = RedPositions,
                    BluePositions = BluePositions,
                    NearestTargetPositions = NearestTargetPositions
                };

                JobHandle findNearestRedHandle = findRedTargetsJob.Schedule(RedPositions.Length, 64);
                findNearestRedHandle.Complete();

                int index = 0;
                foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRO<LocalTransform>>()
                             .WithAll<RedTag, IsAlive>())
                {
                    archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];
                    if(config.EnableTargetingDebug)
                        Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.red, duration: 2.0f);
                    index++;
                }

                ;

                FindNearestJob findBlueTargetsJob = new FindNearestJob
                {
                    RedPositions = BluePositions,
                    BluePositions = RedPositions,
                    NearestTargetPositions = NearestTargetPositions
                };

                JobHandle findNearestBlueHandle = findBlueTargetsJob.Schedule(RedPositions.Length, 64);
                findNearestBlueHandle.Complete();
                index = 0;
                foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRO<LocalTransform>>()
                             .WithAll<BlueTag, IsAlive>())
                {
                    archer.Item1.ValueRW.TargetPosition = NearestTargetPositions[index];
                    if(config.EnableTargetingDebug)
                        Debug.DrawLine(archer.Item2.ValueRO.Position, NearestTargetPositions[index], Color.blue, duration: 2.0f);
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