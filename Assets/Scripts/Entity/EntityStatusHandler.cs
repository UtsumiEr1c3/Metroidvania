using System;
using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity entity;
    private EntityVFX entityVfx;
    private EntityStats stats;
    private ElementType currentEffect = ElementType.None;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(ChillEffectCo(duration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        return currentEffect == ElementType.None;
    }
}
