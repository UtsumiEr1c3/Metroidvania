using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private EntityVFX entityVfx;
    private Entity entity;

    [SerializeField] private float currentHp;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")] 
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockBackDuration = 0.5f;

    [Header("On Heavy Damage")] 
    [SerializeField] private float heavyDamegeThreshold = 0.3f; // Percentage of health you should lose to consider damage as heavy

    private void Awake()
    {
        entityVfx = GetComponent<EntityVFX>();
        entity = GetComponent<Entity>();

        currentHp = maxHp;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
        {
            return;
        }

        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);

        entity?.ReceiveKnockback(knockback, duration);
        entityVfx?.PlayerOnDamageVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    /// <summary>
    /// calculate this damage's knockback
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDealer"></param>
    /// <returns></returns>
    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;

        return knockback;
    }

    /// <summary>
    /// calculate knockback duration
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockBackDuration : knockbackDuration;
    }

    /// <summary>
    /// check if this damage is a heavy damage
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private bool IsHeavyDamage(float damage)
    {
        return damage / maxHp > heavyDamegeThreshold;
    }
}
