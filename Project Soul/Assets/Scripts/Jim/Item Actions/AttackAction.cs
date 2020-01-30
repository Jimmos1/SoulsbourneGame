using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Attack Action")]
public class AttackAction : ItemAction
{
    public override void ExecuteAction(ItemActionContainer ic, Controller cs)
    {
        //cs.AssignCurrentWeaponAndAction((WeaponItem)ic.itemActual, ic);
        cs.PlayTargetAnimation(ic.animName, true, ic.isMirrored);
    }
}
