using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] private Item item;

    public void Populate(Item item)
    {
        this.item = item;
        item.transform.SetParent(transform);
    }
    public bool IsEmpty()
    {
        return !item;
    }
}