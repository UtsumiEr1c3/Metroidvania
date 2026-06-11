using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillShard : SkillBase
{
    private SkillObjectShard currentShard;

    [SerializeField] 
    private GameObject shardPrefab;

    /// <summary>
    /// Time in seconds before the shard explodes after being created.
    /// </summary>
    [SerializeField] 
    [Tooltip("Time in seconds before the shard explodes after being created.")]
    private float detonateTime = 2f;

    [Header("Moving Shard Upgrade")]
    [SerializeField]
    private float shardSpeed = 7f;

    [Header("Multicast Shard Upgrade")]
    [SerializeField]
    private int maxCharges = 3;

    [SerializeField]
    private int currentCharges = 0;

    [SerializeField]
    private bool isRecharging = false;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
        {
            return;
        }

        if (IsUnlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }

        if (IsUnlocked(SkillUpgradeType.ShardMoveToEnemy))
        {
            HandleShardMoving();
        }

        if (IsUnlocked(SkillUpgradeType.ShardMultiCast))
        {
            HandleShardMulticast();
        }
    }

    private void HandleShardMulticast()
    {
        if (currentCharges <= 0)
        {
            return;
        }

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
        {
            StartRecharging().Forget();
        }
    }
    
    private async UniTask StartRecharging()
    {
        isRecharging = true;
        while (isRecharging && currentCharges < maxCharges)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(cooldown));
            currentCharges++;
        }

        isRecharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    /// <summary>
    /// Handles the regular shard creation when the skill is used without any upgrades.
    /// </summary>
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    /// <summary>
    /// Creates a new shard instance at the skill's position.
    /// </summary>
    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObjectShard>();
        currentShard.SetupShard(detonateTime);
    }
}
