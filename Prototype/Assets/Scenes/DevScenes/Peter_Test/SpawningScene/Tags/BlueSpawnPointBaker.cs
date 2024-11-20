using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class BlueSpawnPointBaker : MonoBehaviour
    {
        class BlueSpawnPointBakerBaker : Baker<BlueSpawnPointBaker>
        {
            public override void Bake(BlueSpawnPointBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BlueSpawnPoint>(entity);
            }
        }
    }
    public struct BlueSpawnPoint : IComponentData
    {
    }
}
