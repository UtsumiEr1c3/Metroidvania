using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public SkillToolTip skillToolTip;
    public SkillTree skillTree;
    private bool skillTreeEnabled;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<SkillToolTip>();
        skillTree = GetComponentInChildren<SkillTree>(true);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }
}
