using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private Entity entity;
    private EntityVFX entityVfx;
    private EntityStats stats;

    [SerializeField] private float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("Health regen")]
    [SerializeField] protected float regenInterval = 1;
    [SerializeField] protected bool canRegenerateHealth = true;

    [Header("On Damage Knockback")] 
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockBackDuration = 0.5f;

    [Header("On Heavy Damage")] 
    [SerializeField] private float heavyDamegeThreshold = 0.3f; // Percentage of health you should lose to consider damage as heavy

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();

        currentHealth = stats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
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

        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        return true;
    }

    /// <summary>
    /// check if this attack is evaded
    /// </summary>
    /// <returns></returns>
    private bool IsAttackEvaded()
    {
        return Random.Range(0, 100) < stats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
        {
            return;
        }

        float regenAmount = stats.resource.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
        {
            return;
        }

        float newHealth = currentHealth + healAmount;
        float maxHealth = stats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityVfx?.PlayerOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
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

        healthBar.value = currentHealth / stats.GetMaxHealth();
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);
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
