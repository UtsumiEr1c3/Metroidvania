using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public Player player { get; private set; }

    [Header("General details")]

    [SerializeField] 
    protected SkillType skillType;

    [SerializeField] 
    protected SkillUpgradeType upgradeType;

    [SerializeField] 
    protected float cooldown;

    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        lastTimeUsed -= cooldown;
    }

    public virtual void TryUseSkill()
    {
        
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
        {
            Debug.Log("Skill is not unlocked yet");
            return false;
        }

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

    protected bool IsOnCooldown()
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
