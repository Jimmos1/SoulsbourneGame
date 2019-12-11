using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public string oneHanded_anim = "Empty";
    public string twoHanded_anim = "Two Handed";
    public ItemActionContainer[] itemActions;

    [System.NonSerialized]
    public WeaponHook weaponHook;
}
