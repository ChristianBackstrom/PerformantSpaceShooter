using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct LifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (lifetimeTimer, entity) in SystemAPI.Query<LifetimeComponent>().WithEntityAccess())
        {
            float time = lifetimeTimer.Time - SystemAPI.Time.DeltaTime;

            if (time <= 0)
            {
                Debug.Log("Died");
                ecb.DestroyEntity(entity);
                continue;
            }
            
            var lifetimer = new LifetimeComponent()
            {
                Lifetime = lifetimeTimer.Lifetime,
                Time = time,
            };
            ecb.SetComponent(entity, lifetimer);
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
