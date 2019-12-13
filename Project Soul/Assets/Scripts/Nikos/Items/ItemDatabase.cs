using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemNikos> items = new List<ItemNikos>();

    public ItemNikos GetItemByID(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
                return items[i].GetCopy();
        }
        return null;
    }

    public ItemNikos GetItemByName(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name.ToLower().Equals(name.ToLower()))
                return items[i].GetCopy();
        }
        return null;
    }
}