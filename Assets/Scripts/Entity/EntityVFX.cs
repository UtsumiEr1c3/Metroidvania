using System;
using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On Damage VFX")] 
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Doing Damage VFX")] 
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;

    [Header("Element colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    [SerializeField] private Color electrifyVfx = Color.yellow;
    private Color originalHitVfxColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
        {
            StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
        }

        if (element == ElementType.Fire)
        {
            StartCoroutine(PlayStatusVfxCo(duration, burnVfx));
        }

        if (element == ElementType.Lightning)
        {
            StartCoroutine(PlayStatusVfxCo(duration, electrifyVfx));
        }
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCo(float dutation, Color effectColor)
    {
        float tickInterval = 0.2f;
        float timeHasPassed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.8f;

        bool isToggle = false;

        while (timeHasPassed < dutation)
        {
            sr.color = isToggle ? lightColor : darkColor;
            isToggle = !isToggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.Ice)
        {
            hitVfxColor = chillVfx;
        }

        if (element == ElementType.None)
        {
            hitVfxColor = originalHitVfxColor;
        }
    }

    /// <summary>
    /// create hit effect when get hit
    /// </summary>
    /// <param name="target"></param>
    /// <param name="isCrit"></param>
    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        if (isCrit == false)
        {
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
        }

        if (entity.facingDir == -1 && isCrit)
        {
            vfx.transform.Rotate(0, 180, 0);
        }
    }

    public void PlayerOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
        {
            StopCoroutine(OnDamageVfxCo());
        }
        StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
