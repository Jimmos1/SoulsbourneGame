using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//very early copy pasta state
public class InventoryManager : MonoBehaviour
{

    public GameObject rightHandWeapon;
    public bool hasLeftHandWeapon;
    public GameObject leftHandWeapon;
    //public GameObject parryCollider;


    [Serializable]
    public class Weapon
    {
        public List<Action> actions;
        public List<Action> two_handedActions;
        public GameObject weaponModel;
        //public WeaponHook w_hook; -TODO- Create a weaponhook script we add on prefab weapons to show the animator where to grab the weapon.
    }
}
