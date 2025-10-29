using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFXAutoController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1f;
    [Space]
    [SerializeField] private bool shouldRandomOffset = true;
    [SerializeField] private bool shouldRandomRotation = true;

    [Header("Fade effect")] 
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Random Rotation")] 
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;

    [Header("Random Position")] 
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
        {
            StartCoroutine(FadeCo());
        }
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;
        while (targetColor.a > 0)
        {
            targetColor.a -= (fadeSpeed * Time.deltaTime);
            sr.color = targetColor;
            yield return null;
        }

        sr.color = targetColor;
    }

    /// <summary>
    /// give random position to visual effect
    /// </summary>
    private void ApplyRandomOffset()
    {
        if (shouldRandomOffset == false)
        {
            return;
        }

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);
        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }

    /// <summary>
    /// give random rotation to vfx
    /// </summary>
    private void ApplyRandomRotation()
    {
        if (shouldRandomRotation == false)
        {
            return;
        }

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0, 0, zRotation);
    }
}
