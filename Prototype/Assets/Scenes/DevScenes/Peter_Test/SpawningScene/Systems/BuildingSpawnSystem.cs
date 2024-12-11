using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    partial struct BuildingSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<SpawnBuildings>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();


            float3 position = new float3(0, -3, 0);

            var wallInstance = state.EntityManager.Instantiate(config.WallPrefab);
            state.EntityManager.SetComponentData(wallInstance, new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 1.0f
            });

            var joints = ComponentLookup<RigidJoint>;

        }
    }

    [BurstCompile]
    public struct JointCollisionEvent : IImpulseEventsJob
    {
        [ReadOnly] public BufferLookup<PhysicsJointCompanion> JointCompanionBuffer;
        public EntityCommandBuffer ecb;

        public void Execute(ImpulseEvent impulseEvent)
        {
            Entity jointToBreak = impulseEvent.JointEntity;

            if (JointCompanionBuffer.HasBuffer(jointToBreak))
            {
                var jointCompanionBuffer = JointCompanionBuffer[jointToBreak];
                for (int i = 0; i < jointCompanionBuffer.Length; i++)
                {
                    Debug.Log("DESTROYED" + impulseEvent.Impulse);
                    ecb.DestroyEntity(jointCompanionBuffer[i].JointEntity);
                }
            }

            ecb.DestroyEntity(jointToBreak);
        }
    }
}