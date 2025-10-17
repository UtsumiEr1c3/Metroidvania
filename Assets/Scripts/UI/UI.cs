using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    public SkillToolTip skillToolTip;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<SkillToolTip>();
    }
}
