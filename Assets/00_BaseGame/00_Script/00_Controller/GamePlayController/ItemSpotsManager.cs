using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [SerializeField] private Transform itemSpotParent;
    [SerializeField] private ItemSpot[] itemSpots;
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

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
        if (!IsFreeSpotAvailable())
        {
            Debug.LogWarning("No item spot available");
        }

        HandleItemClicked(item);

    }

    private void HandleItemClicked(Item item)
    {
        MoveItemToFirstSpot(item);
    }

    private void MoveItemToFirstSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();
        if (!targetSpot)
        {
            Debug.LogError("Target spot is null");
            return;
        }
        targetSpot.Populate(item);
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        item.GetComponent<Item>().DisableShadow();
        item.GetComponent<Item>().DisablePhysic();
    }

    private ItemSpot GetFreeSpot()
    {
        for (int i = 0; i < itemSpots.Length; i++)
        {
            if(itemSpots[i].IsEmpty()) return itemSpots[i];
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