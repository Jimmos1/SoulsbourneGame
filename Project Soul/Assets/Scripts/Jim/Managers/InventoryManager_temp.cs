using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager_temp : MonoBehaviour
{
    WeaponHolderHook leftHook;
    private WeaponItem leftItem;

    public bool isLeftEmpty {
        get {
            return leftItem.isUnarmed;
        }
    }

    public bool isRightEmpty {
        get {
            return rightItem.isUnarmed;
        }
    }

    WeaponHolderHook rightHook;
    private WeaponItem rightItem;

    public WeaponItem unarmedWeapon;
    public WeaponHook unarmedHook;

    public ConsumableItem consumableItem;
    [SerializeField]
    private int usesLeftOnConsumable;

    ConsumableHolder _currentConsumable;
    public bool toRefillConsumable = false;

    Controller controller;

    string[] rh_ids = new string[3];
    string[] lh_ids = new string[3];
    string[] cons_ids = new string[3];

    List<WeaponItem> rh_weapons = new List<WeaponItem>();
    List<WeaponItem> lh_weapons = new List<WeaponItem>();

    private void Update()
    {
        if (toRefillConsumable)
        {
            toRefillConsumable = false;
            _currentConsumable.amount = 5;
            usesLeftOnConsumable = _currentConsumable.amount;
        }
    }

    public void SwitchWeapon(bool isLeft)
    {
        WeaponItem result = (isLeft) ? leftItem : rightItem;
        List<WeaponItem> l = (isLeft) ? lh_weapons : rh_weapons;

        int index = l.IndexOf(result);
        index++;

        if (index > l.Count - 1)
        {
            index = 0;
        }

        controller.LoadWeapon(l[index], isLeft);
        if (l[index].isUnarmed)
        {
            controller.LoadWeapon((isLeft) ? rightItem : leftItem, !isLeft);
        }
    }

    public void Init(PlayerProfile profile, Controller c)
    {
        controller = c;

        WeaponHolderHook[] weaponHolderHooks = GetComponentsInChildren<WeaponHolderHook>();
        foreach (WeaponHolderHook hook in weaponHolderHooks)
        {
            if (hook.isLeftHook)
            {
                leftHook = hook;
            }
            else
            {
                rightHook = hook;
            }
        }

        leftItem = unarmedWeapon;
        rightItem = unarmedWeapon;

        if (consumableItem != null)
            _currentConsumable = CreateConsumableHolder(consumableItem);

        usesLeftOnConsumable = _currentConsumable.amount;

        UpdateReferencesFromProfile(profile);
        controller.InitArmor();
    }

    void UpdateReferencesFromProfile(PlayerProfile profile)
    {
        ResourcesManager rm = Settings.resourcesManager;
        for (int i = 0; i < profile.startingArmor.Length; i++)
        {
            Item item = rm.GetItem(profile.startingArmor[i]);
            if (item is ArmorItem)
            {
                controller.startingArmor.Add((ArmorItem)item);
            }
        }

        rh_weapons.Clear();
        lh_weapons.Clear();

        CreateItemsFromIds(profile.rightHand, ref rh_weapons, rm);
        CreateItemsFromIds(profile.leftHand, ref lh_weapons, rm);
        rh_weapons.Add(unarmedWeapon);
        lh_weapons.Add(unarmedWeapon);

        controller.LoadWeapon(rh_weapons[0], false);
        controller.LoadWeapon(lh_weapons[0], true);
    }

    void CreateItemsFromIds(string[] ids, ref List<WeaponItem> target, ResourcesManager rm)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            if (string.IsNullOrEmpty(ids[i]))
                continue;

            Item item = rm.GetItem(ids[i]);

            if (item is WeaponItem)
            {
                target.Add((WeaponItem)item);
            }
        }
    }

    public ConsumableHolder CreateConsumableHolder(ConsumableItem consumableItem)
    {
        ConsumableHolder ch = new ConsumableHolder();
        ch.consumableBase = consumableItem;
        ch.amount = 5; // set starting uses

        return ch;
    }

    public WeaponHook LoadWeaponOnHook(WeaponItem weaponItem, bool isLeft)
    {
        WeaponHook result = null;
        if (weaponItem.isUnarmed)
        {          
            result = unarmedHook;
        }

        if (isLeft)
        {
            leftItem = weaponItem;
            result = leftHook.LoadWeaponModel(weaponItem);
        }
        else
        {
            rightItem = weaponItem;
            result = rightHook.LoadWeaponModel(weaponItem);
        }

        return result;
    }

    public bool TryToConsumeItem(ref string targetAnim)
    {
        bool retVal = false;

        if (_currentConsumable != null)
        {
            if (_currentConsumable.amount > 0)
            {
                targetAnim = _currentConsumable.consumableBase.consumeAnimation;
            }
            else
            {
                targetAnim = _currentConsumable.consumableBase.emptyAnimation;
            }
            retVal = true;

        }

        return retVal;
    }

    public void ConsumeItemActual()
    {
        if (_currentConsumable != null)
        {
            _currentConsumable.consumableBase.OnConsume();
            _currentConsumable.amount--;
            usesLeftOnConsumable = _currentConsumable.amount;
        }
    }

}
