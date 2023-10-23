using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

[BurstCompile]
public partial struct PlayerCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        var ECB = new EntityCommandBuffer(Allocator.Temp);
        
        foreach (var (transform, tag, entity) in SystemAPI.Query<LocalTransform, PlayerTag>().WithEntityAccess())
        {
            float2 projectilePosition = transform.Position.xy;

            bool hit = false;
            
            foreach (var asteroidAspect in SystemAPI.Query<AsteroidFlyAspect>())
            {
                if (hit) continue;
                
                float2 asteroidPosition = asteroidAspect.Position;

                if (MiscMath.GetDistanceSqr(projectilePosition, asteroidPosition) <= 1)
                {
                    CleanAndRestartECS();
                    hit = true;
                }
            }
        }
        
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
    
    private static void CleanAndRestartECS()
    {
        var defaultWorld = World.DefaultGameObjectInjectionWorld;
        defaultWorld.EntityManager.CompleteAllTrackedJobs();
        foreach (var system in defaultWorld.Systems)
        {
            system.Enabled = false;
        }
        defaultWorld.Dispose();
        DefaultWorldInitialization.Initialize("Default World", false);
        if (!ScriptBehaviourUpdateOrder.IsWorldInCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld))
        {
            ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
        }  
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
