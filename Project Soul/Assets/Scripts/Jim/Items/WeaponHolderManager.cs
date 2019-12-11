using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderManager : MonoBehaviour
{
    public WeaponHolderHook leftHook;
    public WeaponItem leftItem;

    public WeaponHolderHook rightHook;
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

    public void LoadWeaponOnHook(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHook.LoadWeaponModel(weaponItem);
            leftItem = weaponItem;
        }
        else
        {
            rightHook.LoadWeaponModel(weaponItem);
            rightItem = weaponItem;
        }
    }
}
