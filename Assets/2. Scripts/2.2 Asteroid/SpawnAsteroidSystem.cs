using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnAsteroidSystem : ISystem 
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {  
        state.RequireForUpdate<SpawnerProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        
        new SpawnAsteroidJob()
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
        }.Run();
    }
}
[BurstCompile]
public partial struct SpawnAsteroidJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ECB;
    private void Execute(SpawnerAspect spawnerAspect)
    {
        spawnerAspect.AsteroidSpawnTimer -= DeltaTime;
        if (!spawnerAspect.TimeToSpawnAsteroid) return;

        spawnerAspect.AsteroidSpawnTimer = spawnerAspect.AsteroidSpawnRate;

        Entity newAsteroid = ECB.Instantiate(spawnerAspect.AsteroidPrefab);

        var newAsteroidTransform = spawnerAspect.GetRandomTransform();
        ECB.SetComponent(newAsteroid, newAsteroidTransform);
    }
}
