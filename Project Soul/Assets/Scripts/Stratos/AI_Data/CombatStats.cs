using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//v2.3
public class CombatStats : MonoBehaviour
{
    /*
     * This class provides a variety of defensive and offensive statistics that can be modified for every agent.
     * Commented values are for testing-reminder.
     */
    public int healthPoints; //100

    [Header("Offense")]
    public int strength; //15

    [Header("Defence")]
    public int physicalDef;  //87
    public int magic_res; //30

    [Header("Resistances")]
    public int bleed; //100
    public int poison; //100

    public bool isDisabled = false; //TODO: CHANGE FROM GOAP CORE IN RUNTIME.
    public bool isSlowed = false;

    public bool isAware = true; //TODO: SET TO FALSE IN LATER VERSION AND ENABLE IT WITH ISAWARE SCRIPT ON GOAPCORE

    private void Start()
    {
        strength = 15;
        physicalDef = 87;
        magic_res = 30;
        bleed = 100;
        poison = 100;
        healthPoints = this.GetComponent<GoapCore>().GetCurrentHealth(); //unnecessary in this version.
    }
    /*
     * In dark souls we have multiple types of dmg taking place in single attacks but for this attempt we will 
     * use a single damage type in all cases.
     */
    public int CalculateFinalDamageGiven(int damage, string damageType)
    {
        int finalDamage = 0;
        switch (damageType)
        {
            case "Physical":
                finalDamage = (int)(damage + damage * PhysicalDamageIncrement());
                //HERE APPLY EXTRA INCREMENT FROM MODIFIERS
                break;
            case "Fire":
                break;
            case "Poison":
                break;
            default:
                finalDamage = (int)(damage + damage * PhysicalDamageIncrement()); 
                break;
        }
        return finalDamage;
    }
    public float PhysicalDamageIncrement()
    {
        if (strength >= 15)
        {
            return 0.2f; //TODODODODO
        }
        else
        {
            return 0.1f;
        }
    }
    public int CalculateFinalDamageTaken(int damage, string damageType)
    {
        int finalDamage = 0;

        switch (damageType)
        {
            case "Physical":
                finalDamage =(int)(damage - damage * PhysicalDamageReduction());
                //HERE APPLY EXTRA REDUCTION FROM MODIFIERS
                break;
            case "Fire": 
                break;
            case "Poison": 
                break;
            default: 
                finalDamage = (int)(damage - damage * PhysicalDamageReduction()); 
                break; 
        }
        return finalDamage;
    }
    public float PhysicalDamageReduction()
    {
        if (physicalDef < 100)
        {
            float physicalReduction;
            healthPoints = this.GetComponent<GoapCore>().GetCurrentHealth(); //Cheat
            if (healthPoints <= 10) //This is gamification decision so Agent always dies when low health.
            {
                physicalReduction = 0f;
            }
            else
            {
                physicalReduction = 0.2f; //basic shit
            }
            
            return physicalReduction;
        }
        else //physical >= 100
        {
            float physicalReduction = 0.3f;
            return physicalReduction;
        }
    }
    /* DAMAGE FORMULA NOTES:
     * 1)VS_Monsters
     * Final Damage = Base Damage * (370/ (370+ Resist After Reduction)) * Crit Multiplier
     * 
     * 2)VS_Players
     * Final Damage = (Base Damage - Resist After Reduction) * (370 / (370 + Resist After Reduction)) * Crit Multiplier
     * 
     * 3)Physical Base Damage = Total Attack Damage * Skill Multiplier + Skill Bonus Damage
     *   Magic Base Damage = Total Ability Power * Skill Multiplier + Skill Bonus Damage
     * 
     * Team Liquid Dark Souls formula analysis:
     * https://tl.net/blogs/396507-dark-souls-stats-i-damage-formula-and-analysis
     * 
     * When Atk < Def, Damage = 0.4*(Atk^3/ Def^2) - 0.09*(Atk^2/ Def)+0.1*Atk
     * When Atk >= Def, Damage = Atk - 0.79* Def*e^(-0.27* Def/Atk)
     * 
     * double finalD = 0.4 * (damage ^ 3 / physical ^ 2) - 0.09 * (damage ^ 2 / physical) + 0.1 * damage;
     * double finalD = damage - 0.79 * physical * 2.7 ^ (-0.27 * physical / damage); 
     * 
     * Dark Souls 1 items spreadsheet:
     * https://docs.google.com/spreadsheets/d/1UvEYcB9Yh09OHRxhpz374x6NwsnP_vhl1rJVhdfaEqo/edit#gid=571886058
     */
}
