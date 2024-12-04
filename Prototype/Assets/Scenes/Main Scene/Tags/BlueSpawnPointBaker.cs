using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
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
