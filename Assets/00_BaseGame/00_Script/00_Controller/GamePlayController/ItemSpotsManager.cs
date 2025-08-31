using System.Collections.Generic;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [SerializeField] private Transform itemSpotParent;
    [SerializeField] private ItemSpot[] spots;
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    [SerializeField] private bool isBusy;
    private Dictionary<EItemName, ItemMergeData> itemMergeDataDictionary = new();

    public void Init()
    {
        InputManager.itemClicked += OnItemClicked;
        StoreSpots();
    }


    private void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    private void OnItemClicked(Item item)
    {
        if (isBusy)
        {
            Debug.Log("ItemSpotManager is busy");
            return;
        }

        if (!IsFreeSpotAvailable())
        {
            Debug.LogWarning("No free spot available");
            return;
        }

        isBusy = true;
        
        HandleItemClicked(item);
    }

    private void HandleItemClicked(Item item)
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName))
            HandleItemMergeDataFound(item);
        else
            MoveItemToFirstFreeSpot(item);
    }

    private void HandleItemMergeDataFound(Item item)
    {
        ItemSpot idealSpot = GetIdealSpotFor(item);
        itemMergeDataDictionary[item.ItemName].Add(item);
        Debug.Log("Wtf");
        TryMoveItemToIdealSpot(item, idealSpot);
    }

    private ItemSpot GetIdealSpotFor(Item item)
    {
        List<Item> lsItems = itemMergeDataDictionary[item.ItemName].lsItems;
        List<ItemSpot> lsItemSpots = new  List<ItemSpot>();

        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItemSpots.Add(lsItems[i].Spot);
        }

        if (lsItemSpots.Count >= 2)
        {
            lsItemSpots.Sort((a,b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }
        int idealSpotIndex = lsItemSpots[0].transform.GetSiblingIndex() + 1;
        return spots[idealSpotIndex];
    }

    private void TryMoveItemToIdealSpot(Item item, ItemSpot idealSpot)
    {
        if (!idealSpot.IsEmpty())
        {
            HandleIdealSpotFull(item, idealSpot);
            Debug.Log("Ideal not empty");
            return;
        }
        MoveItemToSpot(item, idealSpot);
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        
    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot)
    {
        targetSpot.Populate(item);
        
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        
        item.DisableShadow();
        
        item.DisablePhysic();
        
        HandleItemReachedSpot(item);
    }

    private void HandleItemReachedSpot(Item item)
    {
        if (itemMergeDataDictionary[item.ItemName].CanMergeItem())
        {
            MergeItems(itemMergeDataDictionary[item.ItemName]);
        }
        else
        {
            CheckForGameOver();
        }
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> lsItems = itemMergeData.lsItems;
        //Remove from dictionary
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].Spot.Clear();
            Destroy(lsItems[i].gameObject);
        }
        isBusy = false;
    }

    private void MoveItemToFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();
        if (targetSpot == null)
        {
            Debug.LogError("Target spot is null");
            return;
        }

        CreateItemMergeData(item);
        MoveItemToSpot(item,targetSpot);
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (!GetFreeSpot())
            Debug.Log("Game over");
        else
        {
            Debug.LogError("Not Over");
            isBusy = false;
        }
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
        Debug.Log("Item added: " +  item.name + "key to dictionary");
    }

    private ItemSpot GetFreeSpot()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].IsEmpty()) return spots[i];
        }

        return null;
    }

    private void StoreSpots()
    {
        spots = new ItemSpot[itemSpotParent.childCount];
        for (int i = 0; i < itemSpotParent.childCount; i++)
        {
            spots[i] = itemSpotParent.GetChild(i).GetComponent<ItemSpot>();
        }
    }

    private bool IsFreeSpotAvailable()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].IsEmpty()) return true;
        }

        return false;
    }
}