using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ECB = new EntityCommandBuffer(Allocator.Temp);
        
        foreach (var projectileAspect in SystemAPI.Query<ProjectileAspect>())
        {
            float2 projectilePosition = projectileAspect.Position;

            bool hit = false;
            
            foreach (var asteroidAspect in SystemAPI.Query<AsteroidFlyAspect>())
            {
                if (hit) continue;
                
                float2 asteroidPosition = asteroidAspect.Position;

                if (MiscMath.GetDistanceSqr(projectilePosition, asteroidPosition) <= 1)
                {
                    ECB.DestroyEntity(asteroidAspect.Entity);
                    ECB.DestroyEntity(projectileAspect.Entity);
                    hit = true;
                }
            }
        }
        
        ECB.Playback(state.EntityManager);
        ECB.Dispose();
    }
}
