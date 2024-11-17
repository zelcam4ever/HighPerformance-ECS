using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
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