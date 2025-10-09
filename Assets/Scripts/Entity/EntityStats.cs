using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatMajorGroup major;
    public StatOffenseGroup offense;
    public StatDefenceGroup defence;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 0.3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = Random.Range(0, 100) < critChance;
        float damageResult = isCrit ? totalBaseDamage * critPower : totalBaseDamage;

        return damageResult;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;
        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defence.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * 0.5f;

        float totalEvation = baseEvasion + bonusEvasion;
        float evasionCap = 85;

        float finalEvation = Mathf.Clamp(totalEvation, 0, evasionCap);

        return finalEvation;
    }
}
