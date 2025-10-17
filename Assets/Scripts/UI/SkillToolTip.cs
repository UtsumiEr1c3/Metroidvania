using TMPro;
using UnityEngine;

public class SkillToolTip : ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    public override void ShowToolTip(bool isShow, RectTransform targetRect)
    {
        base.ShowToolTip(isShow, targetRect);
    }

    public void ShowToolTip(bool isShow, RectTransform targetRect, SkillDataSO skillData)
    {
        base.ShowToolTip(isShow, targetRect);

        skillName.text = skillData.displayName;
        skillDescription.text = skillData.description;
        skillRequirements.text = "Requirements: \n" + " - " + skillData.cost + " skill point.";
    }
}
