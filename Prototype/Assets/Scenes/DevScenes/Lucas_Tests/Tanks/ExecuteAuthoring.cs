using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.Tanks
{
    class ExecuteAuthoring : MonoBehaviour
    {
        [Header ("Feature 1")] public bool TurretRotation;

        [Header("Feature 2")] public bool TankMovement;

        [Header("Feature 3")] public bool Shooting;
        
        [Header("Feature 4")] public bool ProjectilePhysics;
        
        [Header ("Feature 5")] public bool TankSpawning;
    }

    class ExecuteAuthoringBaker : Baker<ExecuteAuthoring>
    {
        public override void Bake(ExecuteAuthoring authoring)
        {
            // Get entity form of GameObject holding Execute script
            var ent = GetEntity(TransformUsageFlags.None);
            // Add "system check" components to entity if flag is set 
            if(authoring.TurretRotation) AddComponent<TurretRotation>(ent);
            if(authoring.TankMovement) AddComponent<TankMovement>(ent);
            if(authoring.Shooting) AddComponent<Shooting>(ent);
            if(authoring.ProjectilePhysics) AddComponent<ProjectilePhysics>(ent);
            if(authoring.TankSpawning) AddComponent<TankSpawning>(ent);

        }
    }
    // Components
    public struct TurretRotation : IComponentData {}
    public struct TankMovement : IComponentData{}
    public struct Shooting : IComponentData {}
    public struct ProjectilePhysics : IComponentData {}
    public struct TankSpawning : IComponentData {}
}