using System.Collections.Generic;
using DG.Tweening;
using EventDispatcher;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform point;
    private List<Item> itemsToDestroy = new List<Item>();
    
    private void OnMouseDown()
    {
        PlayingAnimation();
    }

    private void PlayingAnimation()
    {
        animator.Play("Activate");
    }
    
    public void VacuumPowerUp()
    {
        // Clear list cũ
        itemsToDestroy.Clear();
        
        var levelManager = GamePlayController.Instance.levelManager;
        var lsItems = levelManager.GetItems();
        Dictionary<EItemName, List<Item>> itemGroups = new Dictionary<EItemName, List<Item>>();
        
        foreach (var item in lsItems)
        {
            if(!item.IsGoal) continue;
            if (!itemGroups.ContainsKey(item.ItemName))
            {
                itemGroups[item.ItemName] = new List<Item>();
            }
            itemGroups[item.ItemName].Add(item);
        }
        
        foreach (var group in itemGroups)
        {
            if (group.Value.Count >= 3)
            {
                // LƯU ITEMS VÀO LIST
                for (int i = 0; i < 3; i++)
                {
                    if (group.Value[i] != null)
                    {
                        itemsToDestroy.Add(group.Value[i]); // ← Thêm vào list
                        ItemAnimation(group.Value[i]);
                    }
                }
                Debug.Log($"Found 3 items of type: {group.Key}");
                return; // Chỉ xử lý 1 nhóm
            }
        }
    }
    
    private void ItemAnimation(Item item)
    {
        item.transform.DOMove(point.position, 1f);
        item.transform.DORotate(Vector3.zero, 1f);
        item.transform.DOScale(Vector3.zero, 1f).OnComplete(delegate
        {
            // Gửi event item bị vacuum
            this.PostEvent(EventID.ITEM_VACUUMED, item.ItemName);
            item.DestroyItem();
        });
    }
}