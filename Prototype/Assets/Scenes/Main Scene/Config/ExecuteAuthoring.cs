using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    class ExecuteAuthoring : MonoBehaviour
    {
        [Header("Spawn Archers")] public bool SpawnArchers;
        [Header("Spawn Boulders")] public bool SpawnBoulders;
        [Header("Allow Archers to shoot")] public bool ArcherShooting;
        [Header("Arrow Physics")] public bool ArrowPhysics;
        [Header("Enable Targeting")] public bool EnableTargeting;
    }

    class ExecuteAuthoringBaker : Baker<ExecuteAuthoring>
    {
        public override void Bake(ExecuteAuthoring authoring)
        {
            var entity  = GetEntity(TransformUsageFlags.None);
            if(authoring.SpawnArchers) AddComponent<SpawnArchers>(entity);
            if(authoring.ArcherShooting) AddComponent<ArcherShooting>(entity);
            if(authoring.ArrowPhysics) AddComponent<ArrowPhysics>(entity);
            if(authoring.SpawnBoulders) AddComponent<SpawnBoulders>(entity);
            if(authoring.EnableTargeting) AddComponent<EnableTargeting>(entity);
            if(!authoring.EnableTargeting) AddComponent<RandomTargeting>(entity);
        }
    }
    
    public struct SpawnArchers : IComponentData {}
    public struct ArcherShooting : IComponentData {}
    public struct ArrowPhysics : IComponentData {}
    public struct SpawnBoulders : IComponentData {}
    public struct RandomTargeting : IComponentData {}
    public struct EnableTargeting : IComponentData {}
}