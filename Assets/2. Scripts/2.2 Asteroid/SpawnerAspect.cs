using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public readonly partial struct SpawnerAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transformAspect;

    private readonly RefRO<SpawnerProperties> _spawnerProperties;
    private readonly RefRW<RandomNumber> _randomNumber;

    public int AsteroidAmount => _spawnerProperties.ValueRO.AsteroidAmount;

    public Entity AsteroidPrefab => _spawnerProperties.ValueRO.Prefab;

    public LocalTransform GetRandomTransform()
    {
        return new LocalTransform()
        {
            Position = GetRandomPosition(),
            Rotation = quaternion.identity,
            Scale = 1f,
        };
    }
    
    private float3 GetRandomPosition()
    {
        float3 randomPosition = _randomNumber.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
        return randomPosition;
    }

    private float3 MinCorner => _transformAspect.ValueRO.Position - HalfDimensions;
    private float3 MaxCorner => _transformAspect.ValueRO.Position + HalfDimensions;
    private float3 HalfDimensions => new()
    {
        x = _spawnerProperties.ValueRO.FieldDimensions.x/2f,
        y = 0f,
        z = _spawnerProperties.ValueRO.FieldDimensions.y/2f
    };
}
