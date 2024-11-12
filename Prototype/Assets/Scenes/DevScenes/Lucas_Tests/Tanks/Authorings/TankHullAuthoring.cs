using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.Tanks.Authorings
{
    class TankHullAuthoring : MonoBehaviour
    {
        public float Speed;
    }

    class Baker : Baker<TankHullAuthoring>
    {
        public override void Bake(TankHullAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Tank {Speed = authoring.Speed});
        }
    }

    public struct Tank : IComponentData
    {
        public float Speed;
    }
}