using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using Unity.Rendering;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Authorings
{
    class ProjectileAuthoring : MonoBehaviour
    {
    
    }

    class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            var ent = GetEntity(TransformUsageFlags.Dynamic);
            // When we don't assign a value to the velocity field
            // it will default to a zero float3.
            AddComponent<Projectile>(ent);
            
            // Can be used later to add color schemes to projectiles
            AddComponent<URPMaterialPropertyBaseColor>(ent); 
        }
    }

    public struct Projectile : IComponentData
    {
        public float3 Velocity;
    }
}