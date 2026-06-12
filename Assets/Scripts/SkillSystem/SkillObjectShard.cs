using System;
using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    public event Action OnExplode;

    [SerializeField] 
    private GameObject vfxPrefab;

    private Transform target;

    private float speed;

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
        
    public void MoveTowardsClosestTarget(float speed)
    {
        target = GetClosestTarget();
        this.speed = speed;
    }

    public void SetupShard(float detonationTime)
    {
        Invoke(nameof(Explode), detonationTime);
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        OnExplode?.Invoke();

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
