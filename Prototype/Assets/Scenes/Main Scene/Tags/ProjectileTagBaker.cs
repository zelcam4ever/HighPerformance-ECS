using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Scenes.Main_Scene
{
    public class ProjectileTagBaker : MonoBehaviour
    {
        class ProjectileTagBakerBaker : Baker<ProjectileTagBaker>
        {
            public override void Bake(ProjectileTagBaker authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ProjectileTag>(entity);
            }
        }
    }

    public struct ProjectileTag : IComponentData
    {
    }
}