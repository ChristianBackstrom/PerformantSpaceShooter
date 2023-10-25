using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct PhysicsCollision : ISystem
{
  public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectileTag>();
        state.RequireForUpdate<AsteroidTag>();
        state.RequireForUpdate<SimulationSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        state.CompleteDependency();

        state.Dependency = new TriggerCollisionJob()
        {
            ProjectileTagGroup = SystemAPI.GetComponentLookup<ProjectileTag>(),
            AsteroidGroup = SystemAPI.GetComponentLookup<AsteroidTag>(),
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }
}

public struct TriggerCollisionJob : ITriggerEventsJob
{
    public ComponentLookup<ProjectileTag> ProjectileTagGroup;
    public ComponentLookup<AsteroidTag> AsteroidGroup;
    public EntityCommandBuffer ECB;

    public void Execute(TriggerEvent triggerEvent)
    {
        
        Entity entityA = triggerEvent.EntityA;
        Entity entityB = triggerEvent.EntityB;

        bool isBodyAPlayer = ProjectileTagGroup.HasComponent(entityA);
        bool isBodyBAsteroid = AsteroidGroup.HasComponent(entityB);

        if (!isBodyAPlayer || !isBodyBAsteroid) return;
        Debug.Log("collided");

        ECB.DestroyEntity(entityA);
        ECB.DestroyEntity(entityB);
    }
}
