using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IModifiable
{
    //All variables on player are public. Stats manager
    public int pHealth;
    public int pFocus;  //focus = mana (used to cast spells and special actions)
    public int pStamina;  // used to perform actions like attacking and defending

    public float equipLoad;
    public float poise; // resistance to stuns/stagger
    public int itemDiscover; //helps with the item drop chance and quality

    //------Defensive Stats------
    public int physicalDef;
    public int vs_Strike; //Strike Damage is one of four physical damage types.It is dealt by most Hammers, Great Hammers, Gauntlets, Catalysts and all Talismans
    public int vs_slash; //Slash Damage is one of four physical damage types and is dealt by curved swords, curved greatswords, katanas, daggers and many halberds.
    public int vs_thrust; //Thrust Damage is one of four physical damage types. It can be dealt by many weapon types and is encountered in many areas of the World.
    
    public int pMagicRes;
    public int pFireRes;
    public int pLightingRes;
    public int pDarkRes;

    public int pBleedRes;
    public int pPoisonRes;
    public int pFrostRes;
    public int pCurseRes;

    public int pAttunementSlots;
    public int pLevel;
    public int pSouls;
    public int pVigor;
    public int pAttunement;
    public int pEndurance;
    public int pVitality;
    public int pStrength;
    public int pDexterity;
    public int pIntelligence;
    public int pFaith;
    public int pLuck;

    private StatsManager statsManager;

    private StatusEffectDatabase statusEffectDatabase;
    private List<Buff> pBuffList;
    private List<Debuff> pDebuffList;

    //private Inventory inventory;
    //GET AND EVALUATE ITEMS 

    public float effectsTickTimer;
    public float defaultEffectsTickTimer = 0.5f; //Tick buff & debuffs
    

    public void Start()
    {
        //setting up refs
        statsManager = (StatsManager)GameObject.FindGameObjectWithTag("Manager").GetComponent(typeof(StatsManager));
        if(statsManager == null)
        {
            Debug.Log("Failed to load Stats Manager on PlayerStats.");
        }

        statusEffectDatabase = (StatusEffectDatabase)this.GetComponent(typeof(StatusEffectDatabase));
        if(statusEffectDatabase == null)
        {
            Debug.Log("Player doesn't have StatusEffectDatabase... Adding one now...");
            statusEffectDatabase = this.gameObject.AddComponent<StatusEffectDatabase>();            
        }

        //TODO: OPS TO LOAD ALL STUFF FROM STATMANAGER.        

        effectsTickTimer = defaultEffectsTickTimer;
    }
    public void Update()
    {
        effectsTickTimer -= Time.deltaTime;
        if(effectsTickTimer<= 0)
        {
            AllBuffsTick();
            AllDebuffsTick();
            //TODO:Maybe send updated stuff on manager and UI.
            effectsTickTimer = defaultEffectsTickTimer;
        }
    }
    
    private void AllBuffsTick() //tick =0.5
    {
        //null list?
        //foreach (Buff buff in pBuffList)
        //{
        //    //tick
        //}
    }
    private void AllDebuffsTick()
    {

    }
    public void ApplyBuff(string buffName, float duration)
    {
        pBuffList.Add(new Buff(statusEffectDatabase.GetBuff(buffName))); //Consider removing new keyword in future patch.
        //BUFF OPS
        //1. Apply its effects
        //2. If it has a dot/hot effect add it to ticks.
        //3. Match it with this script's properties.
    }
    public void ApplyDebuff(string debuffName, float duration)
    {
        pDebuffList.Add(new Debuff(statusEffectDatabase.GetBuff(debuffName)));
    }
    public void ApplyBuff(int buffID)
    {
        pBuffList.Add(new Buff(statusEffectDatabase.GetBuff(buffID)));
    }

    public void ApplyDebuff(int debuffID)
    {
        pDebuffList.Add(new Debuff(statusEffectDatabase.GetBuff(debuffID)));
    }

    public void UpdateAllStats()
    {
        this.pHealth = statsManager.hp;
        //TODO: DO THIS FOR ALL STATS
    }
    public void LoadBuffsFromManager()
    {

    }
    public void LoadDebuffsFromManager()
    {

    }

    public Buff[] GetActiveBuffs()
    {
        throw new System.NotImplementedException();
    }

    public Debuff[] GetActiveDebuffs()
    {
        throw new System.NotImplementedException();
    }

}
