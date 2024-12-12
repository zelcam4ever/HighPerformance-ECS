using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

partial struct DestructionSystem : ISystem
{
    private NativeArray<Unity.Physics.Joint> DestructibleJoints;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        RefRW<EndSimulationEntityCommandBufferSystem.Singleton> ecb = SystemAPI.GetSingletonRW<EndSimulationEntityCommandBufferSystem.Singleton>();
        
        // int count = 0;
        // foreach (var destructibleJoint in SystemAPI.Query<RefRO<Body>>())
        // {
        //     DestructibleJoints[count] = archerTransform.ValueRO.Position;
        //     //Debug.Log("Red:" + RedPositions[count]);
        //     count++;
        // }

        // Schedule the collision event job
         var JointDestructionJob = new JointCollisionEvent
         {
             JointCompanionBuffer = SystemAPI.GetBufferLookup<PhysicsJointCompanion>(true),
             ecb = ecb.ValueRW.CreateCommandBuffer(state.WorldUnmanaged)
         }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
         
         JointDestructionJob.Complete();
         
         
    }
    

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
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
                // Debug.Log("DESTROYED" + impulseEvent.Impulse);
                ecb.DestroyEntity(jointCompanionBuffer[i].JointEntity);
            }
        }
        ecb.DestroyEntity(jointToBreak);
    }
}