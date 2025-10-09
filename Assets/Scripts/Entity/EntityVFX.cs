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

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
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
