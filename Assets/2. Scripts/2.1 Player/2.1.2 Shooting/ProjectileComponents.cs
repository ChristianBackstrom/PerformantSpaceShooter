using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ProjectileShooting : IComponentData
{
    public float Cooldown;
    public Entity ProjectilePrefab;
}

public struct ProjectileCooldownTimer : IComponentData
{
    public float Value;
}

public struct ProjectileComponent : IComponentData
{
    public float Speed;
    public Entity ProjectilePrefab;
}

public struct FireProjectileTag : IEnableableComponent, IComponentData { }

public struct ProjectileTag : IComponentData { }