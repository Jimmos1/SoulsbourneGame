using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemNikos
{
    public int id;
    public ItemType type;
    public string name;
    public string description;
    public GameObject prefab;
    public Sprite image;

    [SerializeField]
    public List<ItemAttribute> attributes = new List<ItemAttribute>();

    public ItemNikos()
    {

    }

    public ItemNikos (int id, ItemType type, string name, string description, GameObject prefab, Sprite image, List<ItemAttribute> attributes)
    {
        this.id = id;
        this.type = type;
        this.name = name;
        this.description = description;
        this.prefab = prefab;
        this.image = image;
        this.attributes = attributes;
    }

    public ItemNikos GetCopy()
    {
        return (ItemNikos)this.MemberwiseClone();
    }
}