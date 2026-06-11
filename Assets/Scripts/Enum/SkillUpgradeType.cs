using UnityEngine;

public enum SkillUpgradeType
{
    None,
    
    // --- Dash tree ---
    Dash,
    DashCloneOnStart,
    DashCloneOnStartAndArrival,
    DashShardOnStart,
    DashShardOnStartAndArrival,

    Shard,
    ShardMoveToEnemy,
    ShardMultiCast,
    ShardTeleport,
    ShardTeleportAndHeal
}
