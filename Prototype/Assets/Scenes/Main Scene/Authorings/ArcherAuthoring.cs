using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class ArcherAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Transform SpawnPoint;
        public float Strength;
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
                Strength = authoring.Strength
            });
            AddComponent<IsAlive>(entity);
        }

      
    }

    public struct Archer : IComponentData
    {
        public Entity ProjectilePrefab;
        public Entity SpawnPoint;
        public float Strength;
    }
    
    public struct IsAlive :IComponentData {}
}