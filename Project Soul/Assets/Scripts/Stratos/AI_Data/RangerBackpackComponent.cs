using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Holds resources for the Ranger-type Agent.
 */
public class RangerBackpackComponent : MonoBehaviour
{
    public GameObject weapon;
    public int numWeaponSlots = 2;
    public int numArrows;
    public int numPotions;
    public string weaponType = "WeaponBow";
    public string secondaryWeaponType = "WeaponMelee";
}
