using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A collection of all the buffs and debuffs 
 * currently deployable in the game.
 */

public class StatusEffectDatabase : MonoBehaviour
{
    public List<Buff> buffs = new List<Buff>();
    public List<Debuff> debuffs = new List<Debuff>();

    public void Awake()
    {
        BuildDatabase();
    }
    
    public Buff GetBuff(int id)
    {
        return buffs.Find(buff => buff._id == id);
    }
    public Buff GetBuff(string buffName)
    {
        return buffs.Find(buff => buff._name == buffName);
    }
    public Debuff GetDebuff(int id)
    {
        return debuffs.Find(debuff => debuff._id == id);
    }
    public Debuff GetDebuff(string debuffName)
    {
        return debuffs.Find(debuff => debuff._name == debuffName);
    }
  
    /* NOTES:
     * 1)Need buff/debuff modification type Type.Flat/Percent  
     * Type might be added in the string. e.g. Fire_dot_Flat, 20 for 20 dmg over the duration
     * or Fire_dot_Perc, 20 for 20% hp dmg over the duration.
     * TODOODODODODO
     * 2)Tick damage???
     * 
     */
    void BuildDatabase()
    {
        buffs = new List<Buff>
        {
                new Buff(0,"God's Strength","Inhuman power that provides strength.",false, 10f,
                new Dictionary<string, int>
                {
                    { "Strength", 10},
                    { "Vs_Strike", 20}
                }),
                new Buff(1, "Profficiency","Greatly boosts user's combat expertise.",false, 20f,
                new Dictionary<string, int>
                {
                    { "Dexterity", 15 },
                    { "Luck", 10}
                })
        };
        debuffs = new List<Debuff>
        {
                  new Debuff(0,"Ignite", "Sets the target on fire",true, 10f,
                  new Dictionary<string,int>
                  {
                      { "Fire_dot", 20 }, //damage equals the max duration full damage (reduction and tick rate not defined here)
                      { "Faith", -10 }
                  }),
                  new Debuff(1,"Cripple", "Slows down the target.",false, 7f,
                  new Dictionary<string, int>
                  {
                      {"MoveSpeed", -50}
                  })
        };
    }
}
