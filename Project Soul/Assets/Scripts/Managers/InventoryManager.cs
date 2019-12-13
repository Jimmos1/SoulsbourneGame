using System;
using System.Collections.Generic;
using UnityEngine;

//very early copy pasta state
public class InventoryManager : MonoBehaviour
{
    // Public
    public GameObject rightHandWeapon;
    public bool hasLeftHandWeapon;
    public GameObject leftHandWeapon;
    //public GameObject parryCollider;

    public List<ItemNikos> inventoryItems;

    // Private
    private ItemDatabase itemDatabase;
    private ItemAttributeDatabase itemAttributeDatabase;


    [Serializable]
    public class Weapon
    {
        public List<Action> actions;
        public List<Action> two_handedActions;
        public GameObject weaponModel;
        //public WeaponHook w_hook; -TODO- Create a weaponhook script we add on prefab weapons to show the animator where to grab the weapon.
    }

    private void Awake()
    {
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        itemAttributeDatabase = Resources.Load<ItemAttributeDatabase>("ItemAttributeDatabase");
        inventoryItems = new List<ItemNikos>();
    }

    public void AddItem(ItemNikos item)
    {
        if (itemDatabase.GetItemByID(item.id) != null)
        {
            inventoryItems.Add(item);

            Debug.Log("Added " + item.name + " to inventory");
            Debug.Log("Attributes:");
            for (int i = 0; i < item.attributes.Count; i++)
                Debug.Log(i+1 + ") " + item.attributes[i].name + ": " + item.attributes[i].value);
        }
    }

    public void AddItem(GameObject gameObject)
    {
        if (itemDatabase.GetItemByName(gameObject.name) != null)
        {
            inventoryItems.Add(itemDatabase.GetItemByName(gameObject.name));

            Debug.Log("Added " + gameObject.name + " to inventory");
            Debug.Log("Attributes:");
            for (int i = 0; i < itemDatabase.GetItemByName(gameObject.name).attributes.Count; i++)
                Debug.Log(i + 1 + ") " + itemDatabase.GetItemByName(gameObject.name).attributes[i].name + ": " + itemDatabase.GetItemByName(gameObject.name).attributes[i].value);
        }
    }

    public void RemoveItem(ItemNikos item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);

            Debug.Log("Revomed " + item.name + " to inventory");
        }
    }

    public void RemoveItem(GameObject gameObject)
    {
        if (itemDatabase.GetItemByName(gameObject.name) != null && inventoryItems.Contains(itemDatabase.GetItemByName(gameObject.name)))
        {
            inventoryItems.Remove(itemDatabase.GetItemByName(gameObject.name));

            Debug.Log("Revomed " + gameObject.name + " to inventory");
        }
    }
}