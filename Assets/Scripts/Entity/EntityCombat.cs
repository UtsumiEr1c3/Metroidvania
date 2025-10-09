using System;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityVFX vfx;
    private EntityStats stats;

    [Header("Target detection")] 
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    /// <summary>
    /// doing attack logic and play effect
    /// </summary>
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null)
            {
                continue; // skip target
            }

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            bool isTargetGotHit = damagable.TakeDamage(damage, transform);
            if (isTargetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform, isCrit);  
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
