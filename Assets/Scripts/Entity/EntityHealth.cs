using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private EntityVFX entityVfx;
    private Entity entity;
    private EntityStats stats;

    [SerializeField] private float currentHp;
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
        stats = GetComponent<EntityStats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
        {
            return false;
        }

        if (IsAttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded this attack");
            return false;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float minigation = stats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - minigation);

        float resistance = stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);

        ReduceHp(physicalDamageTaken + elementalDamageTaken);

        return true;
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);
    }

    /// <summary>
    /// check if this attack is evaded
    /// </summary>
    /// <returns></returns>
    private bool IsAttackEvaded()
    {
        return Random.Range(0, 100) < stats.GetEvasion();
    }

    protected void ReduceHp(float damage)
    {
        entityVfx?.PlayerOnDamageVfx();
        currentHp -= damage;
        UpdateHealthBar();

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
    /// update the heath bar slider UI
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.value = currentHp / stats.GetMaxHealth();
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
        return damage / stats.GetMaxHealth() > heavyDamegeThreshold;
    }
}
