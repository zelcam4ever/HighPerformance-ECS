using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.DevScenes.Lucas_Tests.SpawningScene.Authorings
{
    public class AimerAuthoring : MonoBehaviour
    {
        public Transform worldPosition;
        public Transform targetPosition;
        public float vInitial;
    }
    class AimerAuthoringBaker : Baker<AimerAuthoring>
    {
        public override void Bake(AimerAuthoring authoring)
        {
            
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Aimer()
            {
                WoldPosition = authoring.worldPosition.position,
                TargetPosition = GetEntity(authoring.targetPosition, TransformUsageFlags.Dynamic),
                VInitial = authoring.vInitial
            });
        }
    }
    
    public struct Aimer : IComponentData
    {
        public float3 WoldPosition;
        public Entity TargetPosition;
        public float VInitial;
    }
}