using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

public class SpawnerMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public float MinSpawnDistance;
    public float MaxSpawnDistance;
    public GameObject Prefab;
    public uint RandomSeed;
    public float AsteroidSpawnRate;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new()
        {
            x = FieldDimensions.x,
            y = FieldDimensions.y,
            z = 0,
        });
    }
}

public class SpawnerBaker : Baker<SpawnerMono>
{
    public override void Bake(SpawnerMono authoring)
    {
        AddComponent(new SpawnerProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            Prefab = GetEntity(authoring.Prefab),
            AsteroidSpawnRate = authoring.AsteroidSpawnRate,
            MinSpawnDistance = authoring.MinSpawnDistance,
            MaxSpawnDistance = authoring.MaxSpawnDistance,
        });
        
        AddComponent(new RandomNumber
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed),
        });
        
        AddComponent<AsteroidSpawnTimer>();
    }
}
