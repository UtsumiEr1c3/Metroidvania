using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat setup", fileName = "Default Stat Setup")]
public class StatSetupSO : ScriptableObject
{
    [Header("Resources")] 
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offence - Physical Damage")] 
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;

    [Header("Offence - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defence - Physical Damage")]
    public float armor;
    public float evasion;

    [Header("Offence - Elemental Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;
}
