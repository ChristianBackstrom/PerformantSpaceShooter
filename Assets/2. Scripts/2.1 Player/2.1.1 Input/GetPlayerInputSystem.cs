using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
public partial class GetPlayerInputSystem : SystemBase
{
    private PlayerInputs playerInputs;
    private Entity playerEntity;
    
    protected override void OnCreate()
    {
        playerInputs = new PlayerInputs();
        
    }

    protected override void OnStartRunning()
    {
        playerInputs.Enable();
    }

    protected override void OnStopRunning()
    {
        playerInputs.Disable();
    }

    protected override void OnUpdate()
    {
        Vector2 currentMovementInput = playerInputs.Player.Move.ReadValue<Vector2>();
        var movementInputProperties = new MovementInputProperties();

        if (SystemAPI.TryGetSingleton(out movementInputProperties))
        {
            movementInputProperties.Value = currentMovementInput;
            SystemAPI.SetSingleton(movementInputProperties);
        }
    }
}
