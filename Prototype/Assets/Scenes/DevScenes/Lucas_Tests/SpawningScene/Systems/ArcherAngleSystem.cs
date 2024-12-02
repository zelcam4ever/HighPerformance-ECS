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
                Entity target = aimer.ValueRO.TargetPosition;
                float3 targetPosition = SystemAPI.GetComponent<LocalTransform>(target).Position;
                var v0 = aimer.ValueRO.VInitial;                
                
                var x = math.length(new float3(targetPosition.x - startPosition.x, 0, targetPosition.z - startPosition.z));
                var h = startPosition.y + transform.ValueRO.Position.y - targetPosition.y;
                var t = math.atan2(x, h);
                var a = g * math.pow(x, 2) / math.pow(v0, 2);
                var denominator = math.sqrt(math.pow(h, 2) + math.pow(x, 2));
                var cos = (a - h) / denominator;
                if (cos is < -1 or > 1)
                {
                    //Target is unreachable!
                    continue;
                }
                var total =math.acos(cos);
                var angle = 1.570f -((total + t) / 2);
                
                //Debug.Log(math.degrees(angle));
                
                quaternion aimRotation = quaternion.RotateX(angle);
                transform.ValueRW.Rotation = aimRotation;
            }
        }
    }
}
