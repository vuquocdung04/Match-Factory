
using System.Collections.Generic;

public struct ItemMergeData
{
    public string itemName;
    public List<Item> lsItems;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.name;
        lsItems = new List<Item>();
        lsItems.Add(firstItem);
    }
}