using Unity.Burst;
using Unity.Entities;

public partial struct MoveProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new ProjectileMoveJob()
        {
            DeltaTime = deltaTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct ProjectileMoveJob : IJobEntity
{
    public float DeltaTime;
    
    [BurstCompile]
    private void Execute(ProjectileAspect projectile)
    {
        
        projectile.Move(DeltaTime);
    }
}
