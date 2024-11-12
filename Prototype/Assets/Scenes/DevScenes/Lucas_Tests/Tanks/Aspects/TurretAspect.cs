using Scenes.DevScenes.Lucas_Tests.Tanks.Authorings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Aspects
{
    // Aspects are used to group together components under a higher level of abstraction
    public readonly partial struct TurretAspect : IAspect
    {
        readonly RefRO<Turret> m_Turret;
        readonly RefRO<URPMaterialPropertyBaseColor> m_Color;

        public Entity Projectile => m_Turret.ValueRO.ProjectileEnt;
        public Entity SpawnPoint => m_Turret.ValueRO.SpawnPoint;
        public float4 Color => m_Color.ValueRO.Value; 
    }
}
