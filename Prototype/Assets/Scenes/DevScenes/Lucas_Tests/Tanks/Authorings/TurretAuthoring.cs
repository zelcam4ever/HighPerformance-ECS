using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Authorings
{
    class TurretAuthoring : MonoBehaviour
    {
        // Note: Bakers convert authoring MonoBehaviours into entities and components.
        public GameObject ProjectileGO;
        public Transform SpawnPoint;
    }
    // This script is for baking in the new entities of Projectile and SpawnPoint
    class TurretAuthoringBaker : Baker<TurretAuthoring>
    {
        public override void Bake(TurretAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic); // Get baked entity representing the turret
            
            // Add Turret component to turret entity 
            AddComponent(entity, new Turret { 
                // The GO representation gets passed to GetEntity() to get baked entity. 
                // TransformUsageFlags.Dynamic means the entity needs Transform-components
                // that can be mutated at runtime.
                ProjectileEnt = GetEntity(authoring.ProjectileGO, TransformUsageFlags.Dynamic),
                SpawnPoint = GetEntity(authoring.SpawnPoint, TransformUsageFlags.Dynamic),
            });
            AddComponent<Shooting>(entity);
        }
    }

    /// <summary>
    /// Holds references to the entities that will be instantiated once shooting is enabled.
    /// One holds the actual projectile, while the other 
    /// </summary>
    public struct Turret : IComponentData
    {
        public Entity ProjectileEnt; // Stores Entity form of projectile 
        public Entity SpawnPoint; // Stores Entity form of the projectile spawn point
    }
    public struct Shooting : IComponentData, IEnableableComponent
    {
    }
}