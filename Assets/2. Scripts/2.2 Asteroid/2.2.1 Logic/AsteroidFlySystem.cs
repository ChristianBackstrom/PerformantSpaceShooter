using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(SpawnAsteroidSystem))]
public partial struct AsteroidFlySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AsteroidFlyProperties>();
    }
    
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        state.CompleteDependency();
        
        new AsteroidFlyJob
        {
            DeltaTime = deltaTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct AsteroidFlyJob : IJobEntity
{
    public float DeltaTime;
    
    [BurstCompile]
    private void Execute(AsteroidFlyAspect asteroid)
    {
        asteroid.Move(DeltaTime);
    }
}
