using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemActionContainer
{
    public int animIndex;
    public string[] animName;
    public ItemAction itemAction;
    public AttackInputs attackInput;
    public bool isMirrored;
    public bool isTwoHanded;
    public Item itemActual;

    public void ExecuteItemAction(CharacterStateManager characterStateManager)
    {
        itemAction.ExecuteAction(this, characterStateManager);
    }
}
