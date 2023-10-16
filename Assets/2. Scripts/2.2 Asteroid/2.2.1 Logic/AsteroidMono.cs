using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AsteroidMono : MonoBehaviour
{
    public float Speed;
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
    }
}
