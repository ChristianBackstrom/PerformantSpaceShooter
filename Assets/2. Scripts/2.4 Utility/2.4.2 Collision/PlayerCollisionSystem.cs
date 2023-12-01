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
        var ECB = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transform, tag, entity) in SystemAPI.Query<LocalTransform, PlayerTag>().WithEntityAccess())
        {
            float2 projectilePosition = transform.Position.xy;

            bool hit = false;
            
            foreach (var (asteroidTransform, asteroidTag, asteroidEntity) in SystemAPI.Query<LocalTransform, AsteroidTag>().WithEntityAccess())
            {
                if (hit) continue;
                
                float2 asteroidPosition = asteroidTransform.Position.xy;

                if (MiscMath.GetDistanceSqr(projectilePosition, asteroidPosition) <= 1)
                {
                    hit = true;
                    
                    // ECB.DestroyEntity(entity);
                    // ECB.DestroyEntity(asteroidEntity);
                }
            }
        }
    }
}
