using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public readonly partial struct LifetimeAspect : IAspect
{
    private readonly RefRW<LifetimeComponent> lifetimeTimer;

    public void UpdateTime(float DeltaTime)
    {
        lifetimeTimer.ValueRW.Time -= DeltaTime;
        
        
    }
}
