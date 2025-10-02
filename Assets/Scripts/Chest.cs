using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour, IDamagable
{
    private Animator anim;
    private Rigidbody2D rb;
    private EntityVFX vfx;

    [Header("Open Details")] [SerializeField]
    private Vector2 knockback;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        vfx = GetComponentInChildren<EntityVFX>();
    }

    public void TakeDamage(float damage, Transform damageDealer)
    {
        anim.SetBool("isOpen", true);
        rb.linearVelocity = knockback;
        vfx.PlayerOnDamageVfx();

        rb.angularVelocity = Random.Range(-200f, 200f);
    }
}
