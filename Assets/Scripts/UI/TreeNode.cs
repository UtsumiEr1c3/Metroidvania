using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private SkillTree skillTree;
    private TreeConnectHandler connectHandler;

    [Header("Unlock details")] 
    public TreeNode[] neededNodes;
    public TreeNode[] conflictNodes;
    /// <summary>
    /// if this skill is unlocked
    /// </summary>
    public bool isUnlocked;

    /// <summary>
    /// when another road in current tree is unlocked, node on this tree is locked
    /// </summary>
    public bool isLocked;

    [Header("Skill details")]
    public SkillDataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private int skillCost;
    [SerializeField] private Image skillIcon;
    private readonly string lockedColorHex = "#6E6E6E";
    private Color lastColor;


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<SkillTree>();
        connectHandler = GetComponent<TreeConnectHandler>();

        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    private void OnDisable()
    {
        if (isLocked)
        {
            UpdateIconColor(GetColorByHex(lockedColorHex));
        }

        if (!isUnlocked)
        {
            UpdateIconColor(Color.white);
        }
    }

    public void Refund()
    {
        if (isUnlocked)
        {
            skillTree.AddSkillPoint(skillData.cost);
        }

        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        connectHandler.UnlockConnectionImage(false);
    }

    /// <summary>
    /// unlock the skill tree node
    /// </summary>
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConfictNodes();

        skillTree.RemoveSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(true);
    }

    /// <summary>
    /// check if this skill tree node can be unlocked
    /// </summary>
    /// <returns></returns>
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
        {
            return false;
        }

        if (skillTree.HasEnoughSkillPoints(skillData.cost) == false)
        {
            return false;
        }

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
            {
                return false;
            }
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }

        return true;
    }

    private void LockConfictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
        }
    }

    /// <summary>
    /// change color of icon, and save last color
    /// </summary>
    /// <param name="color"></param>
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
        else if (isLocked)
        {
            ui.skillToolTip.LockedSkillEffect();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);

        if (isUnlocked == false || isLocked == false)
        {
            ToggleNodeHighlight(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);

        if (isUnlocked == false || isLocked == false)
        {
            ToggleNodeHighlight(false);
        }
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * 0.9f;
        highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;
        UpdateIconColor(colorToApply);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    private void OnValidate()
    {
        if (skillData == null)
        {
            return;
        }

        skillName = skillData.displayName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
