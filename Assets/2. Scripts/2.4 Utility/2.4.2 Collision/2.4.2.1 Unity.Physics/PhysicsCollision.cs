using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

// [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
// [UpdateAfter(typeof(PhysicsSystemGroup))]
// [BurstCompile]
// public partial struct PhysicsCollision : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         state.RequireForUpdate<ProjectileTag>();
//         state.RequireForUpdate<AsteroidTag>();
//         state.RequireForUpdate<SimulationSingleton>();
//     }
//
//      [BurstCompile]
//      public void OnUpdate(ref SystemState state)
//      {
//          var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
//      
//          state.Dependency = new TriggerCollisionJob()
//          {
//              ProjectileTagGroup = SystemAPI.GetComponentLookup<ProjectileTag>(),
//              AsteroidGroup = SystemAPI.GetComponentLookup<AsteroidTag>(),
//              ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
//          }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
//      }
// }

[BurstCompile]
public struct TriggerCollisionJob : ITriggerEventsJob
{
    public ComponentLookup<ProjectileTag> ProjectileTagGroup;
    public ComponentLookup<AsteroidTag> AsteroidGroup;
    public EntityCommandBuffer ECB;
    
    [BurstCompile]
    public void Execute(TriggerEvent triggerEvent)
    {
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;

        Debug.Log("collided");

        if (ProjectileTagGroup.HasComponent(entityA) && AsteroidGroup.HasComponent(entityB))
        {
            ECB.DestroyEntity(entityA);
            ECB.DestroyEntity(entityB);
            return;
        }
        
        if (ProjectileTagGroup.HasComponent(entityB) && AsteroidGroup.HasComponent(entityA))
        {
            ECB.DestroyEntity(entityA);
            ECB.DestroyEntity(entityB);
            return;
        }
    }

    public void Execute(CollisionEvent collisionEvent)
    {
        Entity entityA = collisionEvent.EntityA;
        Entity entityB = collisionEvent.EntityB;

        bool isBodyAPlayer = ProjectileTagGroup.HasComponent(entityA);
        bool isBodyBAsteroid = AsteroidGroup.HasComponent(entityB);

        if (!isBodyAPlayer || !isBodyBAsteroid) return;
        Debug.Log("collided");

        ECB.DestroyEntity(entityA);
        ECB.DestroyEntity(entityB);
    }
}