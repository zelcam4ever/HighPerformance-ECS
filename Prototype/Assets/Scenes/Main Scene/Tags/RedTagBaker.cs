using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class RedTagBaker : MonoBehaviour
    {
        class RedTagBakerBaker : Baker<RedTagBaker>
        {
            public override void Bake(RedTagBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<RedTag>(entity);
            }
        }
    }

    public struct RedTag : IComponentData
    {
    }
}