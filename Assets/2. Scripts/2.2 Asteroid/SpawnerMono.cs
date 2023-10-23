using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Random = Unity.Mathematics.Random;

public class SpawnerMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public float MinSpawnDistance;
    public float MaxSpawnDistance;
    public GameObject Prefab;
    public uint RandomSeed;
    public float AsteroidSpawnRate;
    public bool LimitTest;

    [SerializeField] private bool useGizmos;
    
    private void OnDrawGizmos()
    {
        if (!useGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new()
        {
            x = FieldDimensions.x,
            y = FieldDimensions.y,
            z = 0,
        });
        
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position, MinSpawnDistance);
        Gizmos.DrawWireSphere(transform.position, MaxSpawnDistance);

        float step = (MaxSpawnDistance - MinSpawnDistance) / 100;

        for (int i = 0; i < 100; i++)
        {
            Gizmos.DrawWireSphere(transform.position, MinSpawnDistance + step * i);
        }
    }
}

public class SpawnerBaker : Baker<SpawnerMono>
{
    public override void Bake(SpawnerMono authoring)
    {
        var spawnerEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(spawnerEntity, new SpawnerProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            AsteroidSpawnRate = authoring.AsteroidSpawnRate,
            MinSpawnDistance = authoring.MinSpawnDistance,
            MaxSpawnDistance = authoring.MaxSpawnDistance,
        });
        
        AddComponent(spawnerEntity, new RandomNumber
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed),
        });
        
        AddComponent(spawnerEntity, new AsteroidSpawnTimer()
        {
            Value = authoring.AsteroidSpawnRate
        });
        
        AddComponent(spawnerEntity, new LimitTest()
        {
            Value = authoring.LimitTest
        });
    }
}