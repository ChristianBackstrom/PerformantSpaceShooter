using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct ProjectileAspect : IAspect
{
    public readonly Entity Entity;
    
    private readonly RefRW<LocalTransform> transform;
    private readonly RefRO<ProjectileComponent> projectileComponent;
    
    public float2 Position => transform.ValueRO.Position.xy;

    private float Speed => projectileComponent.ValueRO.Speed;

    public void Move(float DeltaTime)
    {
        transform.ValueRW.Position.xy += transform.ValueRO.Right().xy * Speed * DeltaTime;
    }
}
