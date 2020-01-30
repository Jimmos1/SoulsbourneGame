using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemActionContainer
{
    public string animName;
    public ItemAction itemAction;
    public AttackInputs attackInput;
    public bool isMirrored;
    public bool isTwoHanded;
    public Item itemActual;
    public WeaponHook weaponHook;

    public void ExecuteItemAction(Controller controller)
    {
        itemAction.ExecuteAction(this, controller);
    }
}
