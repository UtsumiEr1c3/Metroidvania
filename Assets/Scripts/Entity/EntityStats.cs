using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public StatResourceGroup resource;
    public StatMajorGroup major;
    public StatOffenseGroup offense;
    public StatDefenceGroup defence;

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();
        float bonusElementalDamage = major.intelligence.GetValue();

        element = ElementType.Fire;
        float highestDamage = fireDamage;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = (Mathf.Approximately(fireDamage, highestDamage)) ? 0 : fireDamage * 0.5f;
        float bonusIce = (Mathf.Approximately(iceDamage, highestDamage)) ? 0 : iceDamage * 0.5f;
        float bonusLigntning = (Mathf.Approximately(lightningDamage, highestDamage)) ? 0 : lightningDamage * 0.5f;

        float weakerElementsDamage = bonusFire + bonusIce + bonusLigntning;
        float finalDamage = highestDamage + weakerElementsDamage + bonusElementalDamage;

        return finalDamage * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * 0.5f;

        switch (element)
        {
            case ElementType.Fire:
                baseResistance = defence.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defence.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defence.lightningRes.GetValue();
                break;
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;

        return finalResistance;
    }

    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
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

        return damageResult * scaleFactor;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defence.armor.GetValue();
        float bonusArmor = major.vitality.GetValue() * 1f;
        float totalArmor = baseArmor + bonusArmor;

        float reductionMutiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMutiplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.85f; // max mitigation will be capped at 85%

        float finalMinigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMinigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
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

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.maxHealth: return resource.maxHealth;
            case StatType.HealthRegen: return resource.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critChance;
            case StatType.CritPower: return offense.critPower;
            case StatType.ArmorReduction: return offense.armorReduction;
            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defence.armor;
            case StatType.Evasion: return defence.evasion;
            case StatType.IceResistance: return defence.iceRes;
            case StatType.FireResistance: return defence.fireRes;
            case StatType.LightningResistance: return defence.lightningRes;

            default:
                Debug.LogError($"StatType {type} not implement yes.");
                return null;
        }
    }
}
