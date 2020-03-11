using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHook : MonoBehaviour, IInteractable
{
    //add ID for unique item
    public Item targetItem;
    public bool isThisALeftHandItem = false;
    public InteractionType typeOfInteraction;

    public InteractionType GetInteractionType()
    {
        return typeOfInteraction;
    }

    public void OnInteract(InputControl inp)
    {
        if (targetItem is WeaponItem)
        {
            if (isThisALeftHandItem)
            {
                if (!inp.controller.inventoryManager.lh_weapons.Contains((WeaponItem)targetItem))
                {
                    inp.controller.LoadWeapon(targetItem, true);
                    TempUI.singleton.UpdateQuickSlotForItem(targetItem, true);

                    inp.controller.inventoryManager.lh_weapons.Add((WeaponItem)targetItem);
                }
            }
            else
            {
                if (!inp.controller.inventoryManager.rh_weapons.Contains((WeaponItem)targetItem))
                {
                    inp.controller.LoadWeapon(targetItem, false);
                    TempUI.singleton.UpdateQuickSlotForItem(targetItem, false);

                    inp.controller.inventoryManager.rh_weapons.Add((WeaponItem)targetItem);
                }
            }
        }
        else if (targetItem is ArmorItem)
        {
            inp.controller.LoadArmor(targetItem);
        }
        //TODO ConsumableItem
        gameObject.SetActive(false);
    }
}
