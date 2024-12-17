using Unity.Entities;
using UnityEngine;

namespace Scenes.Main_Scene
{
    public class SetPhysicsMassBaker : MonoBehaviour
    {
        [Header("Physics Mass")] public bool Alive = true;

        //These magic numbers are the baseline inertias for a capsule physics mass
        public float BaselineInertiaX = 3.305785f;
        public float BaselineInertiaY = 8.695652f;
        public float BaselineInertiaZ = 3.305785f;
        public bool InfiniteInertiaX = false;
        public bool InfiniteInertiaY = false;
        public bool InfiniteInertiaZ = false;

        public bool InfiniteMass = false;
        // [Header("Physics Mass Override")]
        // public bool IsKinematic = false;
        // public bool SetVelocityToZero = false;

        class SetPhysicsMassBakerBaker : Baker<SetPhysicsMassBaker>
        {

            public override void Bake(SetPhysicsMassBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SetPhysicsMass()
                {
                    Alive = authoring.Alive,
                    BaselineInertiaX = authoring.BaselineInertiaX,
                    BaselineInertiaY = authoring.BaselineInertiaY,
                    BaselineInertiaZ = authoring.BaselineInertiaZ,
                    InfiniteInertiaX = authoring.InfiniteInertiaX,
                    InfiniteInertiaY = authoring.InfiniteInertiaY,
                    InfiniteInertiaZ = authoring.InfiniteInertiaZ,
                    InfiniteMass = authoring.InfiniteMass
                });
            }
        }
    }

    public struct SetPhysicsMass : IComponentData
    {
        public bool Alive;
        public float BaselineInertiaX;
        public float BaselineInertiaY;
        public float BaselineInertiaZ;
        public bool InfiniteInertiaX;
        public bool InfiniteInertiaY;
        public bool InfiniteInertiaZ;
        public bool InfiniteMass;
    }
}
