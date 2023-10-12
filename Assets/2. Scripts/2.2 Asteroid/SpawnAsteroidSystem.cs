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
        state.Enabled = false;
        var spawnerEntity = SystemAPI.GetSingletonEntity<SpawnerProperties>();
        var spawnerAspect = SystemAPI.GetAspect<SpawnerAspect>(spawnerEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < spawnerAspect.AsteroidAmount; i++)
        {
            Entity newAsteroid = ecb.Instantiate(spawnerAspect.AsteroidPrefab);
            var newAsteroidTransform = spawnerAspect.GetRandomTransform();
            
            ecb.SetComponent(newAsteroid, newAsteroidTransform);
        }
        
        ecb.Playback(state.EntityManager);
    }
}
