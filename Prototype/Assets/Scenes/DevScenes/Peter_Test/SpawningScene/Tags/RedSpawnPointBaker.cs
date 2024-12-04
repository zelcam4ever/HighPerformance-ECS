using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class RedSpawnPointBaker : MonoBehaviour
    {
        class RedSpawnPointBakerBaker : Baker<RedSpawnPointBaker>
        {
            public override void Bake(RedSpawnPointBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<RedSpawnPoint>(entity);
            }
        }
    }
    public struct RedSpawnPoint : IComponentData
    {
    }
}
