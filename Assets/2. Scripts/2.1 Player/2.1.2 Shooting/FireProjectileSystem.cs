using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FireProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (projectile, transform) in SystemAPI.Query<ProjectileShooting, LocalTransform>().WithAll<FireProjectileTag>())
        {
            var newProjectile = ecb.Instantiate(projectile.ProjectilePrefab);
               
            var projectileTransform = LocalTransform.FromPositionRotationScale
                (transform.Position + transform.Right(), transform.Rotation, 0.5f);
                
            ecb.SetComponent(newProjectile, projectileTransform);
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
