using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("General details")] 
    [SerializeField] private SkillType skillType;
    [SerializeField] private SkillUpgradeType upgradeType;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected void Awake()
    {
        lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrade(SkillUpgradeType upgrade)
    {
        upgradeType = upgrade;
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
