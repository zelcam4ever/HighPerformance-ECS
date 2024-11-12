using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Authorings
{
    class ConfigAuthoring : MonoBehaviour
    {
        public GameObject TankPrefab;
        public int tankCount;
    
    }

    class ConfigAuthoringBaker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Config
            {
                TankPrefab = GetEntity(authoring.TankPrefab, TransformUsageFlags.Dynamic),
                tankCount = authoring.tankCount
            });
        }
    }

    public struct Config : IComponentData
    {
        public Entity TankPrefab;
        public int tankCount;
    }
}