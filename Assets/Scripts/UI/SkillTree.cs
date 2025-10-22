using System;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private TreeConnectHandler[] parentNodes;

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        TreeNode[] skillNodes = GetComponentsInChildren<TreeNode>();

        foreach (var node in skillNodes)
        {
            node.Refund();
        }
    }

    public bool HasEnoughSkillPoints(int cost)
    {
        return skillPoints >= cost;
    }

    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
    }

    public void AddSkillPoint(int points)
    {
        skillPoints += points;
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
