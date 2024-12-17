// using Unity.Burst;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Transforms;
// using Unity.Collections;
//
// namespace Scenes.Main_Scene
// {
//     [UpdateAfter(typeof(ArcherSpawningSystem))]
//     partial struct RandomTargetingSystem : ISystem
//     {
//         private NativeArray<float3> RedPositions;
//         private NativeArray<float3> BluePositions;
//         private double elapsedTime;
//         private Random rnd;
//
//         [BurstCompile]
//         public void OnCreate(ref SystemState state)
//         {
//             state.RequireForUpdate<Config>();
//             state.RequireForUpdate<RandomTargeting>();
//             elapsedTime = SystemAPI.Time.ElapsedTime;
//             RedPositions = new NativeArray<float3>(22500, Allocator.Persistent);
//             BluePositions = new NativeArray<float3>(22500, Allocator.Persistent);
//             rnd = new Random();
//         }
//
//         [BurstCompile]
//         public void OnUpdate(ref SystemState state)
//         {
//
//             if (SystemAPI.Time.ElapsedTime - elapsedTime > 2.0f)
//             {
//                 int redCount = 0;
//                 foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<RedTag, IsAlive>())
//                 {
//                     RedPositions[redCount] = archerTransform.ValueRO.Position;
//                     redCount++;
//                 }
//
//                 int blueCount = 0;
//                 foreach (var archerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BlueTag, IsAlive>())
//                 {
//                     BluePositions[blueCount] = archerTransform.ValueRO.Position;
//                     blueCount++;
//                 }
//                 rnd = new Random();
//                 
//                 int randomRedTarget = rnd.NextInt(0, redCount);
//                 int randomBlueTarget = rnd.NextInt(0, blueCount);
//                 
//                 foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRW<LocalTransform>>()
//                              .WithAll<RedTag, IsAlive>())
//                 {
//                     archer.Item1.ValueRW.TargetPosition = BluePositions[randomBlueTarget];
//                 }
//
//                 
//                 foreach (var archer in SystemAPI.Query<RefRW<Archer>, RefRW<LocalTransform>>()
//                              .WithAll<BlueTag, IsAlive>())
//                 {
//                     archer.Item1.ValueRW.TargetPosition = RedPositions[randomRedTarget];
//                 }
//             }
//         }
//         [BurstCompile]
//          public void OnDestroy(ref SystemState state)
//          {
//              if (RedPositions.IsCreated)
//              {
//                  RedPositions.Dispose();
//                  BluePositions.Dispose();
//              }
//          }
//     }
// }