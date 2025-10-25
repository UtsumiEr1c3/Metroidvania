using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("General details")] 
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected void Awake()
    {
        lastTimeUsed -= cooldown;
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
