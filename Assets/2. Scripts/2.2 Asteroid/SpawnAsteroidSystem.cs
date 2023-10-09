using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

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
        throw new System.NotImplementedException();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var spawnerEntity = SystemAPI.GetSingletonEntity<SpawnerProperties>();
        var spawner = SystemAPI.GetAspect<SpawnerAspect>(spawnerEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < spawner.AsteroidAmount; i++)
        {
            ecb.Instantiate(spawner.AsteroidPrefab);
        }
        
        ecb.Playback(state.EntityManager);
    }
}
