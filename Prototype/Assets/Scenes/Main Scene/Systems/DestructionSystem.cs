using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Physics;


namespace Scenes.Main_Scene
{
    partial struct DestructionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            RefRW<EndSimulationEntityCommandBufferSystem.Singleton> ecb =
                SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>();

            // Schedule the impulse event job
            var JointDestructionJob = new JointDestructionEvent
            {
                JointCompanionBuffer = SystemAPI.GetBufferLookup<PhysicsJointCompanion>(true),
                ecb = ecb.ValueRW.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            
            JointDestructionJob.Complete();
        }
    }

    [BurstCompile]
    public struct JointDestructionEvent : IImpulseEventsJob
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
                    ecb.DestroyEntity(jointCompanionBuffer[i].JointEntity);
                }
            }
            ecb.DestroyEntity(jointToBreak);
        }
    }
}