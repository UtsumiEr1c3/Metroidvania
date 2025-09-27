using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    private EntityVFX entityVfx;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    private void Awake()
    {
        entityVfx = GetComponent<EntityVFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
        {
            return;
        }

        entityVfx?.PlayerOnDamageVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if (maxHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
    }
}
