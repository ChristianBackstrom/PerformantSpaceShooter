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
    
    private void CleanAndRestartECS()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
