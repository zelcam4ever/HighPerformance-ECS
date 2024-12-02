using Scenes.DevScenes.Lucas_Tests.SpawningScene.Authorings;
using Scenes.DevScenes.Lucas_Tests.SpawningScene.Config;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Systems
{
    [BurstCompile]
    public partial struct ArcherAngleSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<IsAlive>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            const float g = 9.8f; // Adjust gravity for your game world
            foreach (var (transform, aimer) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Aimer>>())
            {
                float3 startPosition = aimer.ValueRO.WoldPosition + transform.ValueRO.Position;
                float3 targetPosition = aimer.ValueRO.TargetPosition;
                float v0 = aimer.ValueRO.VInitial;                
                
                float x = math.length(new float3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z));
                float h = startPosition.y + transform.ValueRO.Position.y - targetPosition.y;
                float t = math.atan2(x, h);
                float a = g * math.pow(x, 2) / math.pow(v0, 2);
                float denominator = math.sqrt(math.pow(h, 2) + math.pow(x, 2));
                float total =math.acos((a - h) / denominator);
                float angle = 1.570f -((total + t) / 2);
                
                //Debug.Log(math.degrees(angle));
                
                quaternion aimRotation = quaternion.RotateX(angle);
                transform.ValueRW.Rotation = aimRotation;
            }
        }
    }
}
