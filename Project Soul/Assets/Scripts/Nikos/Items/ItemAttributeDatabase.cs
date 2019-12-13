using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemAttributeDatabase")]
public class ItemAttributeDatabase : ScriptableObject
{
    [SerializeField]
    public List<ItemAttribute> itemAttributeDatabase = new List<ItemAttribute>();

}