﻿using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class ObjectBuff : MonoBehaviour
{
    private SpriteRenderer sr;
    private EntityStats statsToModify;

    [Header("Buff details")] 
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private bool canBeUsed = true;

    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBeUsed == false)
        {
            return;
        }

        statsToModify = other.GetComponent<EntityStats>();

        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;
        ApplyBuff(true);


        yield return new WaitForSeconds(duration);

        ApplyBuff(false);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
            {
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            }
            else
            {
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
            }
        }
    }
}
