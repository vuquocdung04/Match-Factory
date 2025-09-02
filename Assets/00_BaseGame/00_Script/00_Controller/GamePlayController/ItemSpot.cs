using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Animator animator;

    [SerializeField] private Transform itemParent;
    
    [SerializeField] private Item item;
    public Item Item => item;
    public void Populate(Item item)
    {
        this.item = item;
        item.transform.SetParent(itemParent);
        
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

    public void BumpDown()
    {
        animator.Play("Bump",0,0);
    }
}