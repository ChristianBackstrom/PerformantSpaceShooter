using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AsteroidFlyProperties : IComponentData
{
    public float Speed;
}

public struct AsteroidHeading : IComponentData
{
    public float Value;
}