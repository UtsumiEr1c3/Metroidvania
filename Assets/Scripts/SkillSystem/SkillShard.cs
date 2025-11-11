using UnityEngine;

public class SkillShard : SkillBase
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2f;

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObjectShard>().SetupShard(detonateTime);
    }
}
