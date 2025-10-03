﻿using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private EntityCombat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<EntityCombat>();
    }

    /// <summary>
    /// Get access to entity and let current entity's state know that we want to exit state
    /// </summary>
    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
