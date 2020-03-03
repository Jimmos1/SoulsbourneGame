using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiCanvas;

    [SerializeField]
    private GameObject weaponsList, consumablesList, keysList;

    [SerializeField]
    private GameObject uiItemPrefab;

    [SerializeField]
    private GameObject itemStatsName, itemStatsDescription;

    [SerializeField]
    private Transform weaponsListParent, consumablesListParent, keysListParent;

    [SerializeField]
    private GameObject selectedItem;

    [SerializeField]
    private Image healthBarImage;

    private ItemDatabase itemDatabase;
    private InventoryManager inventoryManager;


    private void Awake()
    {
        itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
        inventoryManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    public void SelectedItem()
    {
        selectedItem = EventSystem.current.currentSelectedGameObject.gameObject;
        UpdateItemStats();
    }

    public void CategorySwitch(string categoryName)
    {
        weaponsList.SetActive(false);
        consumablesList.SetActive(false);
        keysList.SetActive(false);

        if (categoryName == "Weapons")
        {
            weaponsList.SetActive(true); 
        }
        else if (categoryName == "Consumables")
        {
            consumablesList.SetActive(true);
        }
        else if (categoryName == "Keys")
        {
            keysList.SetActive(true);
        }
    }

    public void AddUIItem(string categoryName, int itemID)
    {
        GameObject uiItem = Instantiate(uiItemPrefab);
        uiItem.name = itemID.ToString();
        uiItem.GetComponent<Image>().sprite = itemDatabase.items[itemID].image;
        uiItem.SetActive(true);

        if (categoryName == "Weapons")
        {
            uiItem.transform.parent = weaponsListParent.transform;
        }
        else if (categoryName == "Consumables")
        {
            uiItem.transform.parent = consumablesListParent.transform;
        }
        else if (categoryName == "Keys")
        {
            uiItem.transform.parent = keysListParent.transform;
        }

        uiItem.transform.localScale = Vector3.one;
    }

    private void ToggleUI()
    {
        uiCanvas.SetActive(!uiCanvas.activeInHierarchy);

        weaponsList.SetActive(true);
        consumablesList.SetActive(false);
        keysList.SetActive(false);
    }

    private void UpdateItemStats()
    {
        itemStatsName.GetComponent<TextMeshProUGUI>().text = itemDatabase.GetItemByID(int.Parse(selectedItem.name)).name;
        itemStatsDescription.GetComponent<TextMeshProUGUI>().text = itemDatabase.GetItemByID(int.Parse(selectedItem.name)).description;
    }

    public void EquipLeftHand()
    {
        if (selectedItem != null && itemDatabase.GetItemByID(int.Parse(selectedItem.name)).type == ItemType.Weapon)
        {
            inventoryManager.leftHandWeapon = selectedItem.gameObject;
        }
    }

    public void EquipRightHand()
    {
        if (selectedItem != null && itemDatabase.GetItemByID(int.Parse(selectedItem.name)).type == ItemType.Weapon)
        {
            inventoryManager.rightHandWeapon = selectedItem.gameObject;
        }
    }

    public void SetHealth(float healthNormalized)
    {
        healthBarImage.fillAmount = healthNormalized;
    }
}