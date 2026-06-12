using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillShard : SkillBase
{
    private SkillObjectShard currentShard;

    private EntityHealth playerHealth;

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

    [Header("Teleport Shard Upgrade")]
    [SerializeField]
    private float shardExistDuration = 10f;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField]
    private float savedHealthPercentage;

    protected override void Awake()
    {
        base.Awake();
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<EntityHealth>();
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

        if (IsUnlocked(SkillUpgradeType.ShardTeleport))
        {
            HandleShardTeleport();
        }

        if (IsUnlocked(SkillUpgradeType.ShardTeleportAndHpRewind))
        {
            HandleShardHealthRewind();
        }
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercentage = playerHealth.GetHealthPercentage();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercentage(savedHealthPercentage);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }
    
    private void SwapPlayerAndShard()
    {
        var shardPosition = currentShard.transform.position;
        var playerPosition = player.transform.position;
        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
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
        currentShard.SetupShard(GetDetonateTime());

        if (IsUnlocked(SkillUpgradeType.ShardTeleport) || IsUnlocked(SkillUpgradeType.ShardTeleportAndHpRewind))
        {
            currentShard.OnExplode += HandleShardExplode;
        }
    }

    public float GetDetonateTime()
    {
        if (IsUnlocked(SkillUpgradeType.ShardTeleport) || IsUnlocked(SkillUpgradeType.ShardTeleportAndHpRewind))
        {
            return shardExistDuration;
        }

        return detonateTime;
    }

    private void HandleShardExplode()
    {
        if (currentShard != null)
        {
            currentShard.OnExplode -= HandleShardExplode;
            currentShard = null;
        }

        ForceCooldown();
    }

    private void ForceCooldown()
    {
        if (IsOnCooldown() == false)
        {
            SetSkillOnCooldown();
        }
    }
}
