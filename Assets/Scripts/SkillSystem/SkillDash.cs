using UnityEngine;

public class SkillDash : SkillBase
{
    public void OnStartEffect()
    {
        if (IsUnlocked(SkillUpgradeType.DashCloneOnStart) || IsUnlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsUnlocked(SkillUpgradeType.DashShardOnStart) || IsUnlocked(SkillUpgradeType.DashShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    public void OnEndEffect()
    {
        if (IsUnlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsUnlocked(SkillUpgradeType.DashShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        Debug.Log("Shard");
    }

    private void CreateClone()
    {
        Debug.Log("Time Echo");
    }
}
