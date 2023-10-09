using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct SpawnerAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transformAspect;

    private readonly RefRO<SpawnerProperties> _spawnerProperties;
    private readonly RefRW<RandomNumber> _randomNumber;

    public int AsteroidAmount => _spawnerProperties.ValueRO.AsteroidAmount;

    public Entity AsteroidPrefab => _spawnerProperties.ValueRO.Prefab;
}
