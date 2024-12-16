using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace Scenes.Main_Scene
{
    class ConfigAuthoring : MonoBehaviour
    {
        public GameObject RedArcherPrefab;
        public GameObject BlueArcherPrefab;
        public GameObject BlueSpawnPoint;
        public GameObject RedSpawnPoint;
        public GameObject BigBoulderPrefab;
        [Header("Choose the battle size. Carnage enables the Soldiers variable\nand is meant for the Main Scene with no castle")]
        public BattleSize BattleSize;
        [Header("Number of soldiers in Carnage mode")]
        [Range (0, 45000)]
        public int Soldiers;
        public SchedulingType SchedulingType;
        
    }

    class ConfigAuthoringBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var ent = GetEntity(TransformUsageFlags.None);
            AddComponent(ent, new Config
            {
                RedArcherPrefab = GetEntity(authoring.RedArcherPrefab, TransformUsageFlags.Dynamic),
                BlueArcherPrefab = GetEntity(authoring.BlueArcherPrefab, TransformUsageFlags.Dynamic),
                BlueSpawnPoint = GetEntity(authoring.BlueSpawnPoint, TransformUsageFlags.None),
                RedSpawnPoint = GetEntity(authoring.RedSpawnPoint, TransformUsageFlags.None),
                BigBoulderPrefab = GetEntity(authoring.BigBoulderPrefab, TransformUsageFlags.Dynamic),
                Soldiers = (int)math.sqrt(authoring.Soldiers / 50f),
                SchedulingType = authoring.SchedulingType,
                BattleSize = authoring.BattleSize, 
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
        public SchedulingType SchedulingType;
        public BattleSize BattleSize;
        public int Soldiers;
    }
    

    public enum SchedulingType
    {
        Schedule,
        ScheduleParallel
    }

    public enum BattleSize
    {
        Tens, //150 (75/75 split)
        Hundreds, //500 (250/250 split)
        Thousands, //5000 (2500/2500 split)
        Carnage //Free (45.000 soldiers result in 230.000 entities at sim start and is the max value atm)
    }
}