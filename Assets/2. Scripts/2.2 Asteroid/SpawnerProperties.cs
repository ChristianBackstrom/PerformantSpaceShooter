using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerProperties : IComponentData
{
    public float2 FieldDimensions;
    public int AsteroidAmount;
    public Entity Prefab;
}
