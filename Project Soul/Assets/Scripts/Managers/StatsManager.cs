using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StatsManager : MonoBehaviour  //This class provides save system integrity and can be used to access most useful info without referencing one billion managers.
{
    //TODO: Convert stuff to protected internal.
    //The type or member can be accessed by any code in the assembly in which it is declared, or from within a derived class in another assembly.
    //We use this to let changes happen only from Game Manager which derives from this class.
    public Vector3 playerPosition = Vector3.one; //test
    public Quaternion playerRotation;

    public GameManager.WayPoint lastVisitedWaypoint;
    public GameManager.WayPoint[] unlockedWaypoints;

    //TODO: Add more vars + Create Inventory Class.
    [Header("Character Stats")]

    [Header("Player Vitals")]
    public int hp = 100;
    public int fp = 100;
    public int stamina = 100;
    public float equipLoad = 20;
    public float poise = 20;
    public int itemDiscover = 111;

    [Header("Attack Power")]
    public int R_weapon_1 = 51;
    public int R_weapon_2 = 51;
    public int R_weapon_3 = 51;
    public int L_weapon_1 = 51;
    public int L_weapon_2 = 51;
    public int L_weapon_3 = 51;

    [Header("Defence")]
    public int physical = 87;
    public int vs_strike = 87;
    public int vs_slash = 87;
    public int vs_thrust = 87;
    public int magic_res = 30;
    public int fire_res = 30;
    public int lighting_res = 30;
    public int dark_res = 30;

    [Header("Resistances")]
    public int bleed = 100;
    public int poison = 100;
    public int frost = 100;
    public int curse = 100;

    public int attunementSlots = 0;
    
    [Header("Attributes")]
    public int level = 1;
    public int souls = 0;
    public int vigor = 11;
    public int attunement = 11;
    public int endurance = 11;
    public int vitality = 11;
    public int strength = 11;
    public int dexterity = 11;
    public int intelligence = 11;
    public int faith = 11;
    public int luck = 11;


    //SAVE-LOAD OPS
    public bool InitSave() //Start the saving process
    {
        //ToDo some weird stuff to lock everything - Then call Save method.
        return true;
    }
    //GETTERS
    public Vector3 GetPlayerPos()
    {
        return playerPosition;
    }
    public Quaternion GetPlayerRot()
    {
        return playerRotation;
    }
    public GameManager.WayPoint GetLastVisitedWaypoint()
    {
        return lastVisitedWaypoint;
    }
}
