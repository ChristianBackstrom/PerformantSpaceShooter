using Unity.Entities;
using Unity.Transforms;

public readonly partial struct ProjectileAspect : IAspect
{
    private readonly RefRW<LocalTransform> transform;
    private readonly RefRO<ProjectileComponent> projectileComponent;

    private float Speed => projectileComponent.ValueRO.Speed;

    public void Move(float DeltaTime)
    {
        transform.ValueRW.Position.xy += transform.ValueRO.Right().xy * Speed * DeltaTime;
    }
}
