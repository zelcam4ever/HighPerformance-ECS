using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class ExecuteAuthoring : MonoBehaviour
    {
        public bool SpawnArchers;
        public bool SpawnBuildings;
    }

    class ExecuteAuthoringBaker : Baker<ExecuteAuthoring>
    {
        public override void Bake(ExecuteAuthoring authoring)
        {
            var entity  = GetEntity(TransformUsageFlags.None);
            if(authoring.SpawnArchers) AddComponent<SpawnArchers>(entity);
            if(authoring.SpawnBuildings) AddComponent<SpawnBuildings>(entity);
        }
    }
    
    public struct SpawnArchers : IComponentData {}
    public struct SpawnBuildings : IComponentData {}
}