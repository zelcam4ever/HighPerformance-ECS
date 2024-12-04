using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class BlueTagBaker : MonoBehaviour
    {
        class BlueTagBakerBaker : Baker<BlueTagBaker>
        {
            public override void Bake(BlueTagBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BlueTag>(entity);
            }
        }
    }

    public struct BlueTag : IComponentData
    {
    }
}