using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Allied,
    Enemy
}
public interface IDamageable
{
    public void TakeDamage(float amount, EntityType entityType);
}
