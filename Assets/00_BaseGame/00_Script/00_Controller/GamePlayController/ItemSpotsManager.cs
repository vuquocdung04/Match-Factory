using System.Collections.Generic;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [SerializeField] private Transform itemSpotParent;
    [SerializeField] private ItemSpot[] itemSpots;
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private bool isBusy;
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
            Debug.Log("Item spot is busy");
            return;
        }

        if (!IsFreeSpotAvailable())
        {
            Debug.LogWarning("No item spot available");
        }

        isBusy = true;
        HandleItemClicked(item);
    }

    private void HandleItemClicked(Item item)
    {
        if (itemMergeDataDictionary.ContainsKey(item.ItemName))
        {
            Debug.Log("Merge data found for: " + item.name);
            HandleItemMergeFound(item);
        }
        else
            MoveItemToFirstSpot(item);
    }

    private void HandleItemMergeFound(Item item)
    {
    }

    private void MoveItemToFirstSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();
        if (!targetSpot)
        {
            Debug.LogError("Target spot is null");
            return;
        }

        CreateItemMergeData(item);

        targetSpot.Populate(item);
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        item.DisableShadow();
        item.DisablePhysic();

        HandleFirstItemReachedSpot(item);
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
        for (int i = 0; i < itemSpots.Length; i++)
        {
            if (itemSpots[i].IsEmpty()) return itemSpots[i];
        }

        return null;
    }

    private void StoreSpots()
    {
        itemSpots = new ItemSpot[itemSpotParent.childCount];
        for (int i = 0; i < itemSpotParent.childCount; i++)
        {
            itemSpots[i] = itemSpotParent.GetChild(i).GetComponent<ItemSpot>();
        }
    }

    private bool IsFreeSpotAvailable()
    {
        for (int i = 0; i < itemSpotParent.childCount; i++)
        {
            if (itemSpots[i].IsEmpty()) return true;
        }

        return false;
    }
}