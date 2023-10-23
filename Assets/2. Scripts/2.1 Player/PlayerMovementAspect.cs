using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct PlayerMovementAspect : IAspect
{
    private readonly RefRO<MovementInputProperties> movementInputProperties;
    private readonly RefRO<MovementValues> movementValues;
    private readonly RefRW<PlayerHeading> playerHeading;
    private readonly RefRW<LocalTransform> transform;

    public float2 MoveInput => movementInputProperties.ValueRO.Value;

    public float2 Position => transform.ValueRO.Position.xy;

    public float Speed => movementValues.ValueRO.ThrustSpeed;
    public float TurnRate => movementValues.ValueRO.TurnRate;
    
    public void Move(float deltaTime)
    {
        float2 movementInput = MoveInput;
        
        if (movementInput.Equals(float2.zero)) return;

        playerHeading.ValueRW.Value += TurnRate * deltaTime * movementInput.x;
        
        transform.ValueRW.Rotation = quaternion.RotateZ(playerHeading.ValueRO.Value);
        
        transform.ValueRW.Position += transform.ValueRO.Right() * (movementInput.y * deltaTime * Speed);

    }
}
