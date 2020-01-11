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
    private UIManager uiManager;


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
        uiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UIManager>();
        inventoryItems = new List<ItemNikos>();
    }

    public void AddItem(ItemNikos item)
    {
        if (itemDatabase.GetItemByID(item.id) != null)
        {
            inventoryItems.Add(item);

            // UI
            if (item.type == ItemType.Weapon)
                uiManager.AddUIItem("Weapons", item.id);
            else if (item.type == ItemType.ChestArmor || item.type == ItemType.HandArmor ||
                item.type == ItemType.HeadArmor || item.type == ItemType.ShoeArmor ||
                item.type == ItemType.TrouserArmor || item.type == ItemType.Shield)
                uiManager.AddUIItem("Armors", item.id);
            else if (item.type == ItemType.Consumable)
                uiManager.AddUIItem("Consumables", item.id);
            else if (item.type == ItemType.None)
                uiManager.AddUIItem("Keys", item.id);

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

            // UI
            if (itemDatabase.GetItemByName(gameObject.name).type == ItemType.Weapon)
                uiManager.AddUIItem("Weapons", itemDatabase.GetItemByName(gameObject.name).id);
            else if (itemDatabase.GetItemByName(gameObject.name).type == ItemType.ChestArmor ||
                itemDatabase.GetItemByName(gameObject.name).type == ItemType.HandArmor ||
                itemDatabase.GetItemByName(gameObject.name).type == ItemType.HeadArmor ||
                itemDatabase.GetItemByName(gameObject.name).type == ItemType.ShoeArmor ||
                itemDatabase.GetItemByName(gameObject.name).type == ItemType.TrouserArmor ||
                itemDatabase.GetItemByName(gameObject.name).type == ItemType.Shield)
                uiManager.AddUIItem("Armors", itemDatabase.GetItemByName(gameObject.name).id);
            else if (itemDatabase.GetItemByName(gameObject.name).type == ItemType.Consumable)
                uiManager.AddUIItem("Consumables", itemDatabase.GetItemByName(gameObject.name).id);
            else if (itemDatabase.GetItemByName(gameObject.name).type == ItemType.None)
                uiManager.AddUIItem("Keys", itemDatabase.GetItemByName(gameObject.name).id);

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