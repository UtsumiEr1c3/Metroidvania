using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class SkillDataSO : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public SkillUpgradeType upgradeType;

    [Header("Skill description")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
}
