
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform point;
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
                for (int i = 0; i < 3; i++)
                {
                    if (group.Value[i] != null)
                    {
                        ItemAnimation(group.Value[i]);
                    }
                }
                
                Debug.Log($"Destroyed 3 items of type: {group.Key}");
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
            item.DestroyItem();
        });
    }
}
