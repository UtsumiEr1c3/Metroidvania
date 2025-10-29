using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class SkillDataSO : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill description")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
}
