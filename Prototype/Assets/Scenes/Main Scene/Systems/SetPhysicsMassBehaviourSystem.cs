using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;

namespace Scenes.Main_Scene
{
    [UpdateAfter(typeof(PhysicsBodyBakingSystem))]
    [UpdateAfter(typeof(RigidbodyBakingSystem))]
    [UpdateAfter(typeof(EndJointBakingSystem))]
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    [UpdateAfter(typeof(ArcherSpawningSystem))]
    public partial struct SetPhysicsMassBehaviourSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // Fill in the MassProperties based on the potential calculated value by BuildCompoundColliderBakingSystem
            foreach (var (mass, setPhysicsMass)
                     in SystemAPI.Query<RefRW<PhysicsMass>, RefRO<SetPhysicsMass>>()
                         .WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities))
            {
                mass.ValueRW.InverseInertia[0] =
                    setPhysicsMass.ValueRO.InfiniteInertiaX ? 0 : setPhysicsMass.ValueRO.BaselineInertiaX;
                mass.ValueRW.InverseInertia[1] =
                    setPhysicsMass.ValueRO.InfiniteInertiaY ? 0 : setPhysicsMass.ValueRO.BaselineInertiaY;
                mass.ValueRW.InverseInertia[2] =
                    setPhysicsMass.ValueRO.InfiniteInertiaZ ? 0 : setPhysicsMass.ValueRO.BaselineInertiaZ;
                mass.ValueRW.InverseMass = setPhysicsMass.ValueRO.InfiniteMass ? 0 : mass.ValueRW.InverseMass;
            }
        }
    }
}