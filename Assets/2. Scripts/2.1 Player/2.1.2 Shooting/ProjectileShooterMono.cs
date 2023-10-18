using Unity.Entities;
using UnityEngine;

public class ProjectileShooterMono : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public float Cooldown;
}

public class ProjectileShooterBaker : Baker<ProjectileShooterMono>
{
    public override void Bake(ProjectileShooterMono authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent<ProjectileShooting>(entity, new ProjectileShooting()
        {
            ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
            Cooldown = authoring.Cooldown
        });
        AddComponent<FireProjectileTag>(entity);
        SetComponentEnabled<FireProjectileTag>(entity, false);
    }
}