using Scenes.DevScenes.Lucas_Tests.Tanks.Authorings;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Systems
{
    public partial struct TurretRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // Unless we disable "automatic bootstrapping", all systems are automatically instantiated and added to the default world.
            // This RequireForUpdate call makes this system update only if at least one entity has the Step2.TurretRotation component.
            // So when we play a scene, this system will only update if this component is added to an entity in a sub scene.
            // This pattern effectively allows each scene to choose whether a system should update.
        
            state.RequireForUpdate<TurretRotation>();
        }
        /*
         QUESTION:
         I want to also pass a variable that is tuned on the turret object
         which is used to multiply the rotation speed. How do I do this?
         
         ANSWER: You can access this, if you add a public field to the Turret component struct.
         You would have to Query for a RefR0 or RefRW of the Turret component to access ValueR0 or ValueRW
         of the rotation speed field. Note that this changes how many entities we loop over in the queries.
         Depending on how you query, it might still affect every entity with just a Transform. See comment below. 
         */
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var rot = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
            // Loop over every entity that has a Local Transform attached and a Turret component.
            // We don't need to access the Turret component values, since we only want to modify
            // the local transforms of entities that represent Turrets.
            // Without this WithAll() call, the query would match *all* entities having transform components,
            // and this foreach would rotate more than just the turrets.
            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Turret>())
            {
              
                // We write to each Transform's rotation and update it with its current rotation
                // multiplied with the "rot" value.
                transform.ValueRW.Rotation = math.mul(rot, transform.ValueRO.Rotation);
            }
        }
    }
}
