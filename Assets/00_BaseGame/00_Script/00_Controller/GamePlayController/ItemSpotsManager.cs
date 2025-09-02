using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [SerializeField] private Transform itemSpotParent;
    [SerializeField] private ItemSpot[] spots;
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    [SerializeField] private bool isBusy;

    [Header("Animation Setting"), Space(5)] 
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private Ease animationEase = Ease.OutCubic;
    
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
        MoveItemToSpot(item, idealSpot, ()=> HandleItemReachedSpot(item));
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        MoveAllItemsToTheRightFrom(idealSpot,item);
    }

    private void MoveAllItemsToTheRightFrom(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex();
        for (int i = spots.Length - 2; i >= spotIndex; i--)
        {
            // Item at index spotIndex, should go on spot spotIndex + 1
            ItemSpot spot = spots[i];
            
            if(spot.IsEmpty()) continue;
            
            
            Item item = spot.Item;
            
            spot.Clear();

            ItemSpot targetSpot = spots[i + 1];

            if (!targetSpot.IsEmpty())
            {
                Debug.LogWarning("Warning, this should no happen");
                isBusy = false;
                return;
            }
            MoveItemToSpot(item,targetSpot, ()=> HandleItemReachedSpot(item,false));
        }
        MoveItemToSpot(itemToPlace,idealSpot, ()=> HandleItemReachedSpot(itemToPlace));
    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot, Action callback)
    {
        targetSpot.Populate(item);
        
        item.transform.DOLocalMove(itemLocalPositionOnSpot, animationDuration).SetEase(animationEase);
        item.transform.DOScale(itemLocalScaleOnSpot, animationDuration);
        item.transform.DOLocalRotate(Vector3.zero, animationDuration).OnComplete(delegate
        {
            callback?.Invoke();
        });
        
        item.DisableShadow();
        
        item.DisablePhysic();
    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {
        item.Spot.BumpDown();
        if(!checkForMerge) return;
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
        if(itemMergeDataDictionary.Count <= 0) isBusy = false;
        else
            MoveAllItemsToTheLeft(HandleAllItemsMovedToTheLeft);
    }

    private void MoveAllItemsToTheLeft(Action callback)
    {
        bool callbackTriggered = false;
        
        for (int i = 3; i < spots.Length; i++)
        {
            ItemSpot spot = spots[i];
            
            if(spot.IsEmpty()) continue;
            
            Item item = spot.Item;
            
            ItemSpot targetSpot = spots[i - 3];

            if (!targetSpot.IsEmpty())
            {
                isBusy = false;
                return;
            }
            spot.Clear();
            
            callback += ()=> HandleItemReachedSpot(item, false);
            MoveItemToSpot(item, targetSpot,callback);
            
            callbackTriggered = true;
        }

        if (!callbackTriggered)
        {
            callback?.Invoke();
        }
    }

    private void HandleAllItemsMovedToTheLeft()
    {
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
        
        MoveItemToSpot(item,targetSpot, () => HandleFirstItemReachedSpot(item));
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        item.Spot.BumpDown();
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