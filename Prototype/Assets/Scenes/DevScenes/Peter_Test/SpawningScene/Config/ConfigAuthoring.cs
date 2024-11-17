using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class ConfigAuthoring : MonoBehaviour
    {
        public GameObject RedArcherPrefab;
        public GameObject BlueArcherPrefab;
        public int RedArcherCount;
        public int BlueArcherCount;
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
                RedArcherCount = authoring.RedArcherCount,
                BlueArcherCount = authoring.BlueArcherCount,
            });
        }
    }

    public struct Config : IComponentData
    {
        public Entity BlueArcherPrefab;
        public Entity RedArcherPrefab;
        public int RedArcherCount;
        public int BlueArcherCount;
    }
}