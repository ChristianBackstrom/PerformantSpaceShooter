using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct LimitTestSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        
         if (!SystemAPI.TryGetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>(out var ecbSingleton))
         {
             return;
         }

        Entity projectilePrefab = SystemAPI.GetSingleton<ProjectileShooting>().ProjectilePrefab;

        new SpawnAsteroidTestJob()
        {
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
        }.Schedule();

        new SpawnProjectileTestJob()
        {
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            ProjectilePrefab = projectilePrefab
        }.Schedule();
    }
}

[BurstCompile]
public partial struct SpawnAsteroidTestJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    private void Execute(SpawnerAspect spawnerAspect)
    {
        if (!spawnerAspect.ShouldLimitTestAsteroid) return;
        
        for (int i = 0; i < spawnerAspect.AsteroidLimitTestAmount; i++)
        {
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
}

[BurstCompile]
public partial struct SpawnProjectileTestJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public Entity ProjectilePrefab;
    private void Execute(SpawnerAspect spawnerAspect)
    {
        if (!spawnerAspect.ShouldLimitTestProjectile) return;

        for (int i = 0; i < spawnerAspect.ProjectileLimitTestAmount; i++)
        {
            Entity newProjectile = ECB.Instantiate(ProjectilePrefab);
            
            float3 position = float3.zero;
            position.xy = spawnerAspect.RandomPosOnUnitCircle();
            var heading = MiscMath.GetHeading(position.xy, spawnerAspect.GetRandomPosition().xy);
            
            var transform = new LocalTransform()
            {
                Position = position,
                Rotation = quaternion.RotateZ(heading),
                Scale = 1f
            };
            ECB.SetComponent(newProjectile, transform);
        }
    }
}
