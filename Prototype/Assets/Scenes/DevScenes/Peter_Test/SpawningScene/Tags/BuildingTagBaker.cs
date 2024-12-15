using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    public class BuildingTagBaker : MonoBehaviour
    {
        class BuildingTagBakerBaker : Baker<BuildingTagBaker>
        {
            public override void Bake(BuildingTagBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BuildingTag>(entity);
            }
        }
    }

    public struct BuildingTag : IComponentData
    {
    }
}