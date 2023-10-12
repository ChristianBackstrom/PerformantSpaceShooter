using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

public class SpawnerMono : MonoBehaviour
{
    public float2 FieldDimensions;
    public int AsteroidAmount;
    public GameObject Prefab;
    public uint RandomSeed;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(FieldDimensions.x, 2, FieldDimensions.y));
    }
}

public class SpawnerBaker : Baker<SpawnerMono>
{
    public override void Bake(SpawnerMono authoring)
    {
        AddComponent(new SpawnerProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            AsteroidAmount = authoring.AsteroidAmount,
            Prefab = GetEntity(authoring.Prefab)
        });
        
        AddComponent(new RandomNumber
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed),
        });
    }
}
