using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public readonly partial struct LifetimeAspect : IAspect
{
    public readonly Entity Entity;
    
    private readonly RefRW<LifetimeComponent> lifetimeTimer;


    public float Lifetime => lifetimeTimer.ValueRO.Lifetime;
    public float Time => lifetimeTimer.ValueRO.Time;
        
    public void UpdateTime(float DeltaTime)
    {
        lifetimeTimer.ValueRW.Time -= DeltaTime;
    }
}
