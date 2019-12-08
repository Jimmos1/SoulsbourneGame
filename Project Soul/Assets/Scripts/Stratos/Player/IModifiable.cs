using System.Collections;
using System.Collections.Generic;
/**
 * Collect the status effect modification 
 * for this object.
 */
using UnityEngine;

/*
 * TO BE DEPLOYED ON ALL COMBAT-READY UNITS
 * ON THE PLAYER-AI STATS CLASS ATTACHED TO THEM.
 */
public interface IModifiable 
{
    Buff[] GetActiveBuffs();
    Debuff[] GetActiveDebuffs();
    void ApplyBuff(string buffName, float duration);

    void ApplyBuff(int buffID);

    void ApplyDebuff(string debuffName, float duration);

    void ApplyDebuff(int debuffID);
}
