using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHook : MonoBehaviour, IInteractable
{
    //add ID for unique item
    public Item targetItem;
    public InteractionType typeOfInteraction;

    public InteractionType GetInteractionType()
    {
        return typeOfInteraction;
    }

    public void OnInteract(InputControl inp)
    {
        if(targetItem is WeaponItem)
        {
            inp.controller.LoadWeapon(targetItem, false);
        }
        else if(targetItem is ArmorItem)
        {
            inp.controller.LoadArmor(targetItem);
        }
        //TODO ConsumableItem
        gameObject.SetActive(false);
    }
}
