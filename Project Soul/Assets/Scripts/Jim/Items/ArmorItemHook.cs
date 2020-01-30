using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorItemHook : MonoBehaviour
{
    public ArmorItemType armorItemType;
    public SkinnedMeshRenderer meshRenderer;
    public Mesh defaultMesh;
    public Material defaultMaterial;

    public void Init()
    {
        ArmorManager armorManager = GetComponentInParent<ArmorManager>();
        armorManager.RegisterArmorHook(this);
    }

    internal void LoadArmorItem(ArmorItem armorItem)
    {
        meshRenderer.sharedMesh = armorItem.mesh;
        meshRenderer.material = armorItem.armorMaterial;
        meshRenderer.enabled = true;
    }

    internal void UnloadItem()
    {
        if (armorItemType.isDisabledWhenNoItem)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.sharedMesh = defaultMesh;
            meshRenderer.material = defaultMaterial;
        }
    }
}
