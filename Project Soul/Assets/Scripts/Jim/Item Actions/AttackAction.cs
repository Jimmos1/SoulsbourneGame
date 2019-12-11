using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Attack Action")]
public class AttackAction : ItemAction
{
    public override void ExecuteAction(ItemActionContainer ic, CharacterStateManager cs)
    {
        string targetAnim = "punch 1";

        if (ic.animIndex < ic.animName.Length && ic.animIndex > -1)
        {
            if (!string.IsNullOrEmpty(ic.animName[ic.animIndex]))
                targetAnim = ic.animName[ic.animIndex];
        }

        //if (string.IsNullOrEmpty(targetAnim)) //failsafe
        //{
        //    Debug.LogWarning("Target Animation is null or empty, assigning default value");
        //    targetAnim = "punch 1";
        //}

        cs.AssignCurrentWeapon((WeaponItem)ic.itemActual, ic);
        cs.PlayTargetAnimation(targetAnim, true, ic.isMirrored);

        ic.animIndex++;
        //if (ic.animIndex > ic.animName.Length - 1)
        //{
        //    ic.animIndex = 0;
        //    cs.canDoCombo = false;
        //}
    }
}
