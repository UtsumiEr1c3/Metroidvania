using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("General details")] 
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected void Awake()
    {
        lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (IsOnCooldown())
        {
            Debug.LogWarning("On cooldown");
            return false;
        }

        return true;
    }

    protected bool IsUnlocked(SkillUpgradeType upgradeToCheck)
    {
        return upgradeType == upgradeToCheck;
    }

    private bool IsOnCooldown()
    {
        return Time.time < lastTimeUsed + cooldown;
    }

    public void SetSkillOnCooldown()
    {
        lastTimeUsed = Time.time;
    }

    public void ResetCooldownBy(float cooldownReduction)
    {
        lastTimeUsed += cooldownReduction;
    }

    public void ResetCooldown()
    {
        lastTimeUsed = Time.time;
    }
}
