using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemActionContainer
{
    public string animName;
    public AttackInputs attackInput;
    public bool isMirrored;
    public bool isTwoHanded;
    public Item itemActual;
    public WeaponHook weaponHook;
    public int damage = 10;
    public bool overrideReactAnim;
    public string reactAnim;
    public bool canParry;
    public bool canBackstab;

}
