using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    public class BoulderTagBaker : MonoBehaviour
    {
        class BoulderTagBakerBaker : Baker<BoulderTagBaker>
        {
            public override void Bake(BoulderTagBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BoulderTag>(entity);
            }
        }
    }

    public struct BoulderTag : IComponentData
    {
    }
}