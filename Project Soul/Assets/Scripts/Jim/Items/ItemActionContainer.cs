using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemActionContainer
{
    public string animName;
    public string altAnimName;
    public AttackInputs attackInput;
    public bool isMirrored;
    public bool isTwoHanded;
    public Item itemActual;
    public WeaponHook weaponHook;
    public int damage = 10;
    public float staminaCost = 20f;
    public int manaCost = 0;
    public bool overrideReactAnim;
    public string reactAnim;
    public bool canParry;
    public bool canBackstab;

}
