using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources Manager")]
public class ResourcesManager : ScriptableObject
{
    public Item[] allItems;

    [System.NonSerialized]
    Dictionary<string, Item> itemsDict = new Dictionary<string, Item>();

    public void Init()
    {
        for (int i = 0; i < allItems.Length; i++)
        {
            itemsDict.Add(allItems[i].name, allItems[i]);
        }
    }

    public Item GetItem(string id)
    {
        Item retVal = null;
        itemsDict.TryGetValue(id, out retVal);
        return retVal;
    }
}
