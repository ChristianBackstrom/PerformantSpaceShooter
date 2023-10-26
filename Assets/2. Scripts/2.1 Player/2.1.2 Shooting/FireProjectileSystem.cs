using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct FireProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        // var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var (projectile, transform) in SystemAPI.Query<ProjectileShooting, LocalTransform>().WithAll<FireProjectileTag>())
        {
            var newProjectile = ecb.Instantiate(projectile.ProjectilePrefab);
               
            var projectileTransform = LocalTransform.FromPositionRotationScale
                (transform.Position + transform.Right(), transform.Rotation, 0.5f);
                
            ecb.SetComponent(newProjectile, projectileTransform);

            // new FireProjectileJob()
            // {
            //     ECB = ecb,
            //     ProjectilePrefab = projectile.ProjectilePrefab,
            //     Rotation = transform.Rotation,
            //     SpawnPosition = transform.Position + transform.Right()
            // }.Schedule();
        }
        
        // ecb.Playback(state.EntityManager);
        // ecb.Dispose();
    }
}

public partial struct FireProjectileJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float3 SpawnPosition;
    public quaternion Rotation;
    public Entity ProjectilePrefab;
    public void Execute()
    {
        Entity newProjectile = ECB.Instantiate(ProjectilePrefab);

        var transform = new LocalTransform()
        {
            Position = SpawnPosition,
            Rotation = Rotation,
            Scale = 1f
        };
        ECB.SetComponent(newProjectile, transform);
    }
}
