using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;

    [SerializeField] private SkillDataSO skillData;
    [SerializeField] private string skillName;

    /// <summary>
    /// the icon of this skill
    /// </summary>
    [SerializeField] private Image skillIcon;

    /// <summary>
    /// the color when the skill is locked
    /// </summary>
    private string lockedColorHex = "#6E6E6E";

    /// <summary>
    /// save the last color before change
    /// </summary>
    private Color lastColor;

    /// <summary>
    /// if this skill is unlocked
    /// </summary>
    public bool isUnlocked;

    /// <summary>
    /// when another road in current tree is unlocked, node on this tree is locked
    /// </summary>
    public bool isLocked;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        UpdateIconColor(GetColorByHex(lockedColorHex));
    }

    /// <summary>
    /// unlock the skill tree node
    /// </summary>
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
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

        return true;
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
        else
        {
            Debug.Log("Cannot be unlocked");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, skillData);
        if (isUnlocked == false)
        {
            UpdateIconColor(Color.white * 0.9f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);
        if (isUnlocked == false)
        {
            UpdateIconColor(lastColor);
        }
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
        gameObject.name = "UI_TreeNode - " + skillData.displayName;
    }
}
