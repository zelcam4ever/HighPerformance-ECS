using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Config
{
    class ConfigAuthoring : MonoBehaviour
    {
        public GameObject ArcherPrefab;
        public int ArcherCount;
        public int NumberOfBattalions;
        public int ArcherFormation;
        
    }

    class ConfigAuthoringBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var ent = GetEntity(TransformUsageFlags.None);
            AddComponent(ent, new Config
            {
                ArcherPrefab = GetEntity(authoring.ArcherPrefab, TransformUsageFlags.Dynamic),
                ArcherCount = authoring.ArcherCount,
                NumberOfBattalions = authoring.NumberOfBattalions,
                ArcherFormation = authoring.ArcherFormation,
                
            });
        }
    }

    public struct Config : IComponentData
    {
        public Entity ArcherPrefab;
        public int ArcherCount;
        public int NumberOfBattalions;
        public int ArcherFormation;
       
    }
}