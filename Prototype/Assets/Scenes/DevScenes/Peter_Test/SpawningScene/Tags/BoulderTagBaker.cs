using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
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