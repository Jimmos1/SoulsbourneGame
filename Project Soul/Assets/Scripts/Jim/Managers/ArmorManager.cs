using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{
    Dictionary<ArmorItemType, ArmorItemHook> armorHooks = new Dictionary<ArmorItemType, ArmorItemHook>();

    public void Init()
    {
        ArmorItemHook[] armorItemHooks = GetComponentsInChildren<ArmorItemHook>();
        foreach (ArmorItemHook hook in armorItemHooks)
        {
            hook.Init();
        }
    }

    public void RegisterArmorHook(ArmorItemHook armorItemHook)
    {
        if (!armorHooks.ContainsKey(armorItemHook.armorItemType))
        {
            armorHooks.Add(armorItemHook.armorItemType, armorItemHook);
        }
    }

    public void LoadListOfItems(List<ArmorItem> armorItems)
    {
        UnloadAllItems();

        for (int i = 0; i < armorItems.Count; i++)
        {
            LoadItem(armorItems[i]);
        }
    }

    void UnloadAllItems()
    {
        foreach (ArmorItemHook hook in armorHooks.Values)
        {
            hook.UnloadItem();
        }
    }

    ArmorItemHook GetArmorHook(ArmorItemType target)
    {
        armorHooks.TryGetValue(target, out ArmorItemHook retVal);
        return retVal;
    }

    public void LoadItem(ArmorItem armorItem)
    {
        ArmorItemHook itemHook = null;

        if (armorItem == null)
        {         
            return;
        }

        itemHook = GetArmorHook(armorItem.armorType);
        itemHook.LoadArmorItem(armorItem);
    }
}


