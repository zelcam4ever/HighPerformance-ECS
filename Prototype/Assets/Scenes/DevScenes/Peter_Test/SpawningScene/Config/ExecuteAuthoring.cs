using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class ExecuteAuthoring : MonoBehaviour
    {
        [Header("Spawn Archers (press G to spawn)")] public bool SpawnArchers;

    }

    class ExecuteAuthoringBaker : Baker<ExecuteAuthoring>
    {
        public override void Bake(ExecuteAuthoring authoring)
        {
            var entity  = GetEntity(TransformUsageFlags.None);
            if(authoring.SpawnArchers) AddComponent<SpawnArchers>(entity);
        }
    }
    
    public struct SpawnArchers : IComponentData {}
}