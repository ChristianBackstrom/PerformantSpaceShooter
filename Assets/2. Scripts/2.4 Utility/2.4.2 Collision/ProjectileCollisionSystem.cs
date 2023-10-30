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
        
        // projectileGroup = state.GetEntityQuery(ComponentType.ReadOnly<LocalTransform>(), ComponentType.ReadOnly<ProjectileTag>());
        // asteroidGroup = state.GetEntityQuery(ComponentType.ReadOnly<LocalTransform>(), ComponentType.ReadOnly<AsteroidTag>());
        // var projectileCollisionJob = new ProjectileCollisionJob()
        // {
        //     TransformTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false),
        //     EntityTypeHandle = state.GetEntityTypeHandle(),
        //     TransformsToTestAgainst = asteroidGroup.ToComponentDataArray<LocalTransform>(Allocator.TempJob),
        //     EntitiesToTestAgainst = asteroidGroup.ToEntityArray(Allocator.TempJob),
        //     ECB = ecb
        // };
        //
        // state.Dependency = projectileCollisionJob.ScheduleParallel(projectileGroup, state.Dependency);
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


// foreach (var asteroidAspect in SystemAPI.Query<AsteroidFlyAspect>())
// {
//     float2 asteroidPosition = asteroidAspect.Position;
//
//     bool hit = false;
//             
//     foreach (var projectileAspect in SystemAPI.Query<ProjectileAspect>())
//     {
//         if (hit) continue;
//                 
//         float2 projectilePosition = projectileAspect.Position;
//
//         if (MiscMath.GetDistanceSqr(projectilePosition, asteroidPosition) <= 1)
//         {
//             ECB.DestroyEntity(asteroidAspect.Entity);
//             ECB.DestroyEntity(projectileAspect.Entity);
//             hit = true;
//         }
//     }
// }