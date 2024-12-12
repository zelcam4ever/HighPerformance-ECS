// using System.Linq;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using UnityEngine;
//
// namespace Scenes.DevScenes.Peter_Test.SpawningScene
// {
//     partial struct CountingSystem : ISystem
//     {
//         private float start_time;
//
//         [BurstCompile]
//         public void OnCreate(ref SystemState state)
//         {
//             start_time = Time.time;
//         }
//
//         [BurstCompile]
//         public void OnUpdate(ref SystemState state)
//         {
//             if (Time.time - start_time > 3)
//             {
//                 var uq = state.EntityManager.UniversalQuery;
//
//                 string entities = "THIS COUNTER IS NOT BURSTED!\nCurrent entities: ";
//                 entities += uq.CalculateEntityCount();
//
//                 Debug.Log(entities);
//
//                 start_time = Time.time;
//             }
//         }
//
//         [BurstCompile]
//         public void OnDestroy(ref SystemState state)
//         {
//
//         }
//     }
// }
