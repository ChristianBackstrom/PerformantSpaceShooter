using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    public float ThrustSpeed;
    public float TurnRate;
}

public class PlayerBaker : Baker<PlayerMono>
{
    public override void Bake(PlayerMono authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent<MovementInputProperties>(entity);
        
        AddComponent(entity, new MovementValues()
        {
            ThrustSpeed = authoring.ThrustSpeed,
            TurnRate = authoring.TurnRate,
        });
        
        AddComponent(entity, new PlayerHeading()
        {
            Value = 0,
        });
    }
}
