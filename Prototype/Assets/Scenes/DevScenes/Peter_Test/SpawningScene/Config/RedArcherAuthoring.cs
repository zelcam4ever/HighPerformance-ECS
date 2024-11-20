using Unity.Entities;
using UnityEngine;

namespace Scenes.DevScenes.Peter_Test.SpawningScene
{
    class RedArcherAuthoring : MonoBehaviour
    {
        public GameObject RedArcherPrefab;
    }
    
    class RedArcherAuthoringBaker : Baker<RedArcherAuthoring>
    {
        public override void Bake(RedArcherAuthoring authoring)
        {
            var ent = GetEntity(TransformUsageFlags.None);
            AddComponent(ent, new RedArcher
            {
                RedArcherPrefab = GetEntity(authoring.RedArcherPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }

    public struct RedArcher : IComponentData, IEnableableComponent
        {
            public Entity RedArcherPrefab;
        }
}



