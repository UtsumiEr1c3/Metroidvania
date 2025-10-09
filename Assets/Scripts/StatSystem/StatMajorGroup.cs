using UnityEngine;
using System;

[Serializable]
public class StatMajorGroup
{
    /// <summary>
    /// +1 Physical Damage per point, 0.5% Crit Power per point
    /// </summary>
    public Stat strength;

    /// <summary>
    /// 0.5% Evasion per point, 0.3% Crit Chance per point
    /// </summary>
    public Stat agility;

    /// <summary>
    /// +1 Magical Damage per point, 0.5% Elemental resistance per point
    /// </summary>
    public Stat intelligence;

    /// <summary>
    /// +5 Max Health per point, +1 Armor per point
    /// </summary>
    public Stat vitality;
}
