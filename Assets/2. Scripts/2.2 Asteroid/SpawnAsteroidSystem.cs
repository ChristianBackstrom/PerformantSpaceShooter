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
        }.Schedule();
    }
}
[BurstCompile]
public partial struct SpawnAsteroidJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ECB;
    private void Execute(SpawnerAspect spawnerAspect)
    {
        if (spawnerAspect.ShouldLimitTestAsteroid) return;
        
        spawnerAspect.AsteroidSpawnTimer -= DeltaTime;
        if (!spawnerAspect.TimeToSpawnAsteroid) return;

        spawnerAspect.AsteroidSpawnTimer = spawnerAspect.AsteroidSpawnRate;
        Entity newAsteroid = ECB.Instantiate(spawnerAspect.AsteroidPrefab);

        
        float3 position = spawnerAspect.GetRandomPosition();
        var asteroidHeading = MiscMath.GetHeading(position.xy, spawnerAspect.GetRandomPositionInBox());
        ECB.SetComponent(newAsteroid, new AsteroidHeading(){Value = asteroidHeading});
        
        var newAsteroidTransform = new LocalTransform()
        {
            Position = position,
            Rotation = quaternion.RotateZ(asteroidHeading),
            Scale = 1f
        };
        ECB.SetComponent(newAsteroid, newAsteroidTransform);
    }
}