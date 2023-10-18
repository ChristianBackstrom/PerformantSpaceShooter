using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidMono : MonoBehaviour
{
    public float Speed;
    public float Lifetime = 20;
}

public class AsteroidBaker : Baker<AsteroidMono>
{
    public override void Bake(AsteroidMono authoring)
    {
        var asteroidEntity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(asteroidEntity, new AsteroidFlyProperties()
        {
            Speed = authoring.Speed,
        });
        AddComponent<AsteroidHeading>(asteroidEntity);
        
        AddComponent(asteroidEntity, new LifetimeComponent()
        {
            Time = authoring.Lifetime,
            Lifetime = authoring.Lifetime,
        });
        
        AddComponent<AsteroidTag>(asteroidEntity);
    }
}
