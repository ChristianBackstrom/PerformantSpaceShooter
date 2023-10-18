using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ProjectileMono : MonoBehaviour
{
    public float Speed;
}

public class ProjectileBaker : Baker<ProjectileMono>
{
    public override void Bake(ProjectileMono authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new ProjectileComponent()
        {
            Speed = authoring.Speed
        });
        
        AddComponent<ProjectileTag>(entity);
    }
}
