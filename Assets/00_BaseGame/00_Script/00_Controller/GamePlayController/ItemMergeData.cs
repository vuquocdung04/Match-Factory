
using System.Collections.Generic;

public struct ItemMergeData
{
    public EItemName itemName;
    public List<Item> lsItems;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.ItemName;
        lsItems = new List<Item>();
        lsItems.Add(firstItem);
    }

    public void Add(Item item)
    {
        this.lsItems.Add(item);
    }

    public bool CanMergeItem()
    {
        return lsItems.Count >= 3;
    }
}