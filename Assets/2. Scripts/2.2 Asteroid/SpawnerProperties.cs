using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerProperties : IComponentData
{
    public float2 FieldDimensions;
    public float MinSpawnDistance;
    public float MaxSpawnDistance;
    public Entity Prefab;
    public float AsteroidSpawnRate;
}

public struct AsteroidSpawnTimer : IComponentData
{
    public float Value;
}

public struct LimitTest : IComponentData
{
    public bool Value;
}