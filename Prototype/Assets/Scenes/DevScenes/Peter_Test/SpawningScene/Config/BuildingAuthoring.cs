using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    public class BuildingAuthoring : MonoBehaviour
    {
        class BuildingAuthoringBaker : Baker<BuildingAuthoring>
        {
            public override void Bake(BuildingAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Building>(entity);
            }
        }
    }
    

    public struct Building : IComponentData
    {
    }
}