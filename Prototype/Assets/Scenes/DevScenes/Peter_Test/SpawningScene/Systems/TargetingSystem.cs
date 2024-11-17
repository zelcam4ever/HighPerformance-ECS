using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
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
            RedPositions = new NativeArray<float3>(100, Allocator.Persistent);
            BluePositions = new NativeArray<float3>(100, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(100, Allocator.Persistent);
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
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
        
            FindNearestJob findJob = new FindNearestJob
            {
                RedPositions = RedPositions,
                BluePositions = BluePositions,
                NearestTargetPositions = NearestTargetPositions
            };
            
            JobHandle findNearestHandle = findJob.Schedule(RedPositions.Length, 100);
            findNearestHandle.Complete();
            
            for (int i = 0; i < RedPositions.Length; i++)
            {
                //Debug.Log("Red:" + RedPositions[i] + ", Nearest: " + NearestTargetPositions[i]);
                Debug.DrawLine(RedPositions[i], NearestTargetPositions[i]);
            }
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            RedPositions.Dispose();
            BluePositions.Dispose();
            NearestTargetPositions.Dispose();
        }
    }
}