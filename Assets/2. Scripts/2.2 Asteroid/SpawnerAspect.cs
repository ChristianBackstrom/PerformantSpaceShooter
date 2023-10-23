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
    private LocalTransform Transform => _transformAspect.ValueRO;
    public float3 Position => Transform.Position;

    public bool ShouldLimitTest => _limitTest.ValueRO.Value;
    public float2 FieldDimensions => _spawnerProperties.ValueRO.FieldDimensions;

    private readonly RefRO<SpawnerProperties> _spawnerProperties;
    private readonly RefRW<RandomNumber> _randomNumber;
    private readonly RefRW<AsteroidSpawnTimer> _asteroidSpawnTimer;
    private readonly RefRO<LimitTest> _limitTest;
    public Entity AsteroidPrefab => _spawnerProperties.ValueRO.Prefab;

    public float2 RandomPosOnUnitCircle()
    {
        return _randomNumber.ValueRW.Value.NextFloat2Direction();
    }

    public float3 GetRandomPosition()
    {
        float3 randomPosition = new float3();

        float2 randomUnitCircle = _randomNumber.ValueRW.Value.NextFloat2Direction();

        randomPosition = new()
        {
            x = randomUnitCircle.x,
            y = randomUnitCircle.y,
            z = 0
        };

        randomPosition *= _randomNumber.ValueRW.Value.NextFloat(_spawnerProperties.ValueRO.MinSpawnDistance, _spawnerProperties.ValueRO.MaxSpawnDistance);

        return randomPosition;
    }

    public float2 GetRandomPositionInBox()
    {
        return _randomNumber.ValueRW.Value.NextFloat2(-FieldDimensions/2, FieldDimensions/2);
    }

    private float3 MinCorner => _transformAspect.ValueRO.Position - HalfDimensions;
    private float3 MaxCorner => _transformAspect.ValueRO.Position + HalfDimensions;
    private float3 HalfDimensions => new()
    {
        x = _spawnerProperties.ValueRO.FieldDimensions.x/2f,
        y = 0f,
        z = _spawnerProperties.ValueRO.FieldDimensions.y/2f
    };
    private const float PlayerSafetyRadius = 100;
    private quaternion GetRandomRotation() => quaternion.RotateZ(_randomNumber.ValueRW.Value.NextFloat(-.25f, .25f));
    private float GetRandomScale(float min) => _randomNumber.ValueRW.Value.NextFloat(min, 1f);

    public float AsteroidSpawnTimer
    {
        get => _asteroidSpawnTimer.ValueRO.Value;
        set => _asteroidSpawnTimer.ValueRW.Value = value;
    }

    public bool TimeToSpawnAsteroid => AsteroidSpawnTimer <= 0;

    public float AsteroidSpawnRate => _spawnerProperties.ValueRO.AsteroidSpawnRate;
    
    
}
