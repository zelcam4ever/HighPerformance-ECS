using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class ArcherAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Transform SpawnPoint;
        public Transform Aimer;
        public float3 TargetPosition;
        public float Strength;
        public float TimerReload;
        public float CurrentTimeToShoot;
    }

    class Baker : Baker<ArcherAuthoring>
    {
        public override void Bake(ArcherAuthoring authoring)
        {
            
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Archer
            {
                ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                SpawnPoint = GetEntity(authoring.SpawnPoint, TransformUsageFlags.Dynamic),
                TargetPosition = authoring.TargetPosition,
                Aimer = GetEntity(authoring.Aimer, TransformUsageFlags.Dynamic),
                Strength = authoring.Strength,
                TimerReload = authoring.TimerReload,
                CurrentTimeToShoot = 0
            });
            AddComponent<IsAlive>(entity);
        }

      
    }

    public struct Archer : IComponentData
    {
        public Entity ProjectilePrefab;
        public Entity SpawnPoint;
        public Entity Aimer;
        public float3 TargetPosition;
        public float Strength;
        public float TimerReload;
        public float CurrentTimeToShoot;
    }
    
    public struct IsAlive :IComponentData {}
}