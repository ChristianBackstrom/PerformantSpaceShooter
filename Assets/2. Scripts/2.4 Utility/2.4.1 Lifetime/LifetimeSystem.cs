using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct LifetimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new LifetimeJob()
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        }.ScheduleParallel();

    }
}

[BurstCompile]
public partial struct LifetimeJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float DeltaTime;
    [BurstCompile]
    private void Execute(LifetimeAspect lifetimeAspect)
    {
        float time = lifetimeAspect.Time - DeltaTime;

        if (time <= 0)
        {
            Debug.Log("Died");
            ECB.DestroyEntity(lifetimeAspect.Entity);
        }
            
        var lifetimer = new LifetimeComponent()
        {
            Lifetime = lifetimeAspect.Lifetime,
            Time = time,
        };
        
        ECB.SetComponent(lifetimeAspect.Entity, lifetimer);
    }
}


// [UpdateInGroup(typeof(LateSimulationSystemGroup))]
// public partial struct LifetimeSystem : ISystem
// {
//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
//
//         new LifetimeJob()
//         {
//             DeltaTime = SystemAPI.Time.DeltaTime,
//             ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
//         }.ScheduleParallel();
//
//     }
// }
//
// public partial struct LifetimeJob : IJobEntity
// {
//     public EntityCommandBuffer ECB;
//     public float DeltaTime;
//     private void Execute(LifetimeAspect lifetimeAspect)
//     {
//         float time = lifetimeAspect.Time - DeltaTime;
//
//         if (time <= 0)
//         {
//             Debug.Log("Died");
//             ECB.DestroyEntity(lifetimeAspect.Entity);
//         }
//             
//         var lifetimer = new LifetimeComponent()
//         {
//             Lifetime = lifetimeAspect.Lifetime,
//             Time = time,
//         };
//         
//         ECB.SetComponent(lifetimeAspect.Entity, lifetimer);
//     }
// }

