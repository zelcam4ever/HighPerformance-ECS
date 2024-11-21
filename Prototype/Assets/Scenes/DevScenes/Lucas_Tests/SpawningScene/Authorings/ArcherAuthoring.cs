using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Authorings
{
    class ArcherAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public Transform SpawnPoint;
    }

    class Baker : Baker<ArcherAuthoring>
    {
        public override void Bake(ArcherAuthoring authoring)
        {
            
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Archer
            {
                ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                SpawnPoint = GetEntity(authoring.SpawnPoint, TransformUsageFlags.Dynamic)
            });
        }

      
    }

    public struct Archer : IComponentData
    {
        public Entity ProjectilePrefab;
        public Entity SpawnPoint;
    }
}