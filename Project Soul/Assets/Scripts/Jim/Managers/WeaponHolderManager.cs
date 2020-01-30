using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderManager : MonoBehaviour
{
    WeaponHolderHook leftHook;
    public WeaponItem leftItem;

    WeaponHolderHook rightHook;
    public WeaponItem rightItem;

    public void Init()
    {
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
    }

    public WeaponHook LoadWeaponOnHook(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftItem = weaponItem;
            return leftHook.LoadWeaponModel(weaponItem);
        }
        else
        {
            rightItem = weaponItem;
            return rightHook.LoadWeaponModel(weaponItem);
        }
    }
}
