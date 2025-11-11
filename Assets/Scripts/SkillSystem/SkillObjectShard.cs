using System;
using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    [SerializeField] private GameObject vfxPrefab;

    public void SetupShard(float detonationTime)
    {
        Invoke(nameof(Explode), detonationTime);
    }

    private void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() == null)
        {
            return;
        }

        Explode();
    }
}
