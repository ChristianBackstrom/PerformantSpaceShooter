using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct AsteroidFlyAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transformAspect;

    private readonly RefRO<AsteroidFlyProperties> _asteroidFlyProperties;
    private readonly RefRO<AsteroidHeading> _asteroidHeading;

    public float Heading => _asteroidHeading.ValueRO.Value;
    public float Speed => _asteroidFlyProperties.ValueRO.Speed;
    
    public void Move(float deltaTime)
    {
        _transformAspect.ValueRW.Position += _transformAspect.ValueRO.Right() * Speed * deltaTime;
    }
}
