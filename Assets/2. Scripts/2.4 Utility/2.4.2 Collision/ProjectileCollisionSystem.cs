using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct ProjectileCollisionSystem : ISystem
{
    private EntityQuery projectileGroup;
    private EntityQuery asteroidGroup;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (asteroidTransform, asteroidEntity) in SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<AsteroidTag>().WithEntityAccess())
        {
            foreach (var (projectileTransform, bulletEntity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<ProjectileTag>().WithEntityAccess())
            {
                
                if (MiscMath.GetDistanceSqr(projectileTransform.ValueRO.Position.xy, asteroidTransform.ValueRO.Position.xy) < 1f)
                {
                    ecb.DestroyEntity(asteroidEntity);
                    ecb.DestroyEntity(bulletEntity);
                }
            }
        }
    }
}

// Slow as fuck for some reason
[BurstCompile]
public struct ProjectileCollisionJob : IJobChunk
{
    [ReadOnly] public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
    [ReadOnly] public EntityTypeHandle EntityTypeHandle;
    [ReadOnly] public NativeArray<LocalTransform> TransformsToTestAgainst;
    [ReadOnly] public NativeArray<Entity> EntitiesToTestAgainst;
    

    public EntityCommandBuffer ECB;

    [BurstCompile]
    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        NativeArray<LocalTransform> transformArray = chunk.GetNativeArray(ref TransformTypeHandle);
        NativeArray<Entity> entities = chunk.GetNativeArray(EntityTypeHandle);

        for (int i = 0; i < transformArray.Length; i++)
        {
            LocalTransform position = transformArray[i];

            bool hit = false;

            for (int j = 0; j < TransformsToTestAgainst.Length; j++)
            {
                if (hit) continue;
                
                LocalTransform position2 = TransformsToTestAgainst[j];
                
                if (!(MiscMath.GetDistanceSqr(position.Position.xy, position2.Position.xy) <= 2)) continue;
                
                ECB.DestroyEntity(entities[i]);
                ECB.DestroyEntity(EntitiesToTestAgainst[j]);
                hit = true;
            }
        } 
    }
}