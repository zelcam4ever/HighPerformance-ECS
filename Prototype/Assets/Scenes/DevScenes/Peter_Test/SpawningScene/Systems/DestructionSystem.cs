// using Unity.Burst;
// using Unity.Entities;
// using Unity.Collections;
// using Unity.Mathematics;
// using Unity.Physics;
// using Unity.Physics.Authoring;
// using UnityEngine;
//
// partial struct DestructionSystem : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         
//     }
//
//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         var ecb = new EntityCommandBuffer(Allocator.TempJob);
//
//         // Schedule the collision event job
//         var JoinDestructionJob = new JointCollisionEvent
//         {
//             JointComponents = SystemAPI.GetComponentLookup<PhysicsJoint>(isReadOnly: false),
//             ecb = ecb
//         }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//     }
//
//     [BurstCompile]
//     public void OnDestroy(ref SystemState state)
//     {
//         
//     }
// }
//
// [BurstCompile]
// public struct JointCollisionEvent : ICollisionEventsJob
// {
//     public ComponentLookup<PhysicsJoint> JointComponents;
//     public EntityCommandBuffer ecb;
//
//     public void Execute(CollisionEvent collisionEvent)
//     {
//         var normalImpulse = collisionEvent.Normal;
//
//         // Convert impulse to force (F = dp / dt; assuming dt ≈ 1 for simplicity)
//         float force = math.length(normalImpulse);
//         
//         if (JointComponents.HasComponent(collisionEvent.EntityA))
//         {
//             Debug.Log("Collision force applied");
//             if (force > 0.1f)
//             {
//                 Debug.Log("Collision force applied");
//                 ecb.RemoveComponent<PhysicsJoint>(collisionEvent.EntityA);
//             }
//         }
//
//         if (JointComponents.HasComponent(collisionEvent.EntityB))
//         {
//             Debug.Log("Collision force applied");
//             if (force > 0.1f)
//             {
//                 Debug.Log("Collision force applied");
//                 ecb.RemoveComponent<PhysicsJoint>(collisionEvent.EntityB);
//             }
//         }
//     }
// }