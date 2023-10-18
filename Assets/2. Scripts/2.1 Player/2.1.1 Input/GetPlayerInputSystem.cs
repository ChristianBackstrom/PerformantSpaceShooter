using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class GetPlayerInputSystem : SystemBase
{
    private PlayerInputs playerInputs;
    private InputAction FireAction;
    private Entity playerEntity;
    private bool fireActionAssigned = false;
    
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerTag>();
        RequireForUpdate<MovementInputProperties>();
        
        playerInputs = new PlayerInputs();
        FireAction = playerInputs.Player.Fire;
    }

    protected override void OnStartRunning()
    {
        playerInputs.Enable();
        FireAction.Enable();
        FireAction.performed += Fire;

        playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
    }

    protected override void OnStopRunning()
    {
        playerInputs.Disable();
        FireAction.Disable();
        
        FireAction.performed -= Fire;
        playerEntity = Entity.Null;
    }

    protected override void OnUpdate()
    {
        Vector2 currentMovementInput = playerInputs.Player.Move.ReadValue<Vector2>();

        SystemAPI.SetSingleton(new MovementInputProperties(){Value = currentMovementInput});
    }

    private void Fire(InputAction.CallbackContext ctx)
    {
        if (!SystemAPI.Exists(playerEntity)) return;
        
        SystemAPI.SetComponentEnabled<FireProjectileTag>(playerEntity, true);
    }
}
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct ResetInputSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (tag, entity) in SystemAPI.Query<FireProjectileTag>().WithEntityAccess())
        {
            ecb.SetComponentEnabled<FireProjectileTag>(entity, false);
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}