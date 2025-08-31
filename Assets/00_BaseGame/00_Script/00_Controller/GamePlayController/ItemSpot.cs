using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] private Item item;

    public void Populate(Item item)
    {
        this.item = item;
        item.transform.SetParent(transform);
        
        item.AssignSpot(this);
    }
    public bool IsEmpty()
    {
        return item == null;
    }

    public void Clear()
    {
        item = null;
    }
}