using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct LifetimeComponent : IComponentData
{
    public float Time;
    public float Lifetime;
}
