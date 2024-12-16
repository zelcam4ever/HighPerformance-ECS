using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class ConfigAuthoring : MonoBehaviour
    {
        public GameObject RedArcherPrefab;
        public GameObject BlueArcherPrefab;
        public GameObject BlueSpawnPoint;
        public GameObject RedSpawnPoint;
        public GameObject BigBoulderPrefab;
        public int RedArcherCount;
        public int BlueArcherCount;
        public BattleSize BattleSize;
        public SchedulingType SchedulingType;
        public bool EnableTargetingDebug;
        
    }

    class ConfigAuthoringBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            // int armySize;
            // switch (authoring.BattleSize)
            // {
            //     case BattleSize.Tens:
            //         armySize = 75;
            //         break;
            //     case BattleSize.Hundreds:
            //         armySize = 250;
            //         break;
            //     case BattleSize.Thousands:
            //         armySize = 2500;
            //         break;
            //     default:
            //         armySize = 0;
            //         break;
            // }
            var ent = GetEntity(TransformUsageFlags.None);
            AddComponent(ent, new Config
            {
                RedArcherPrefab = GetEntity(authoring.RedArcherPrefab, TransformUsageFlags.Dynamic),
                BlueArcherPrefab = GetEntity(authoring.BlueArcherPrefab, TransformUsageFlags.Dynamic),
                BlueSpawnPoint = GetEntity(authoring.BlueSpawnPoint, TransformUsageFlags.None),
                RedSpawnPoint = GetEntity(authoring.RedSpawnPoint, TransformUsageFlags.None),
                BigBoulderPrefab = GetEntity(authoring.BigBoulderPrefab, TransformUsageFlags.Dynamic),
                RedArcherCount = authoring.RedArcherCount,
                BlueArcherCount = authoring.BlueArcherCount,
                SchedulingType = authoring.SchedulingType,
                BattleSize = authoring.BattleSize,
                EnableTargetingDebug = authoring.EnableTargetingDebug,
                // RedPositions = new NativeArray<float3>(armySize, Allocator.Persistent),
                // BluePositions = new NativeArray<float3>(armySize, Allocator.Persistent),
                // NearestTargetPositions = new NativeArray<float3>(armySize, Allocator.Persistent),
            });
        }
    }

    public struct Config : IComponentData
    {
        public Entity BlueArcherPrefab;
        public Entity RedArcherPrefab;
        public Entity BlueSpawnPoint;
        public Entity RedSpawnPoint;
        public Entity BigBoulderPrefab;
        public int RedArcherCount;
        public int BlueArcherCount;
        public SchedulingType SchedulingType;
        public BattleSize BattleSize;
        public bool EnableTargetingDebug;
        // public NativeArray<float3> RedPositions;
        // public NativeArray<float3> BluePositions;
        // public NativeArray<float3> NearestTargetPositions;
    }
    

    public enum SchedulingType //I think we need to be able to switch between these, so I made it preemptively
    {
        Schedule,
        ScheduleParallel
    }

    public enum BattleSize
    {
        Tens, //150 (75/75 split)
        Hundreds, //500 (250/250 split)
        Thousands, //5000 (2500/2500 split)
    }
}