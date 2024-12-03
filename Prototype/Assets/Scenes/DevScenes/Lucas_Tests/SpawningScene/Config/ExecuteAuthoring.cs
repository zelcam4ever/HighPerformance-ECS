using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Config
{
    class ExecuteAuthoring : MonoBehaviour
    {
        [Header("Spawn Archers (press G to spawn)")] public bool SpawnArchers;
        [Header("Allow Archers to shoot")] public bool ArcherShooting;
        [Header("Arrow Physics")] public bool ArrowPhysics;

    }

    class ExecuteAuthoringBaker : Baker<ExecuteAuthoring>
    {
        public override void Bake(ExecuteAuthoring authoring)
        {
            var entity  = GetEntity(TransformUsageFlags.None);
            if(authoring.SpawnArchers) AddComponent<SpawnArchers>(entity);
            if(authoring.ArcherShooting) AddComponent<ArcherShooting>(entity);
            if(authoring.ArrowPhysics) AddComponent<ArrowPhysics>(entity);
        }
    }
    
    public struct SpawnArchers : IComponentData {}
    public struct ArcherShooting : IComponentData {}
    public struct ArrowPhysics : IComponentData {}
}