using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public struct MovementInputProperties : IComponentData
{
    public float2 Value;
}

public struct MovementValues : IComponentData
{
    public float ThrustSpeed;
    public float TurnRate;
}

public struct PlayerHeading : IComponentData
{
    public float Value;
}


