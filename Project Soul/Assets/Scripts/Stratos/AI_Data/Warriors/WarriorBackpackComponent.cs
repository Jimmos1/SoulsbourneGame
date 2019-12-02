using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Holds resources for the Warrior-type Agent.
 */
public class WarriorBackpackComponent : MonoBehaviour
{
    public GameObject weapon;
    public int numWeaponSlots = 1;
    public int numPotions;
    public string weaponType = "WeaponMelee";
    public string secondaryWeaponType = "";
}
