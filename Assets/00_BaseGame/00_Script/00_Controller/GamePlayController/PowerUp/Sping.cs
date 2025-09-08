using System;
using DG.Tweening;
using UnityEngine;

public class Sping : MonoBehaviour
{
    private void OnMouseDown()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(delegate
        {
            transform.DOScale(Vector3.one, 0.2f);
        });
        SpringPowerUp();
    }

    private void SpringPowerUp()
    {
        var itemSpot = GamePlayController.Instance.itemSpotsManager.GetNearestItemSpotFromLeft();
        if (itemSpot == null)
        {
            Debug.LogError("All itemSpot empty");
            return;
        }
        var item = itemSpot.Item;
        if(item.IsBusy) return;
        GamePlayController.Instance.goalManager.OnItemSpringed(item.ItemName);
        GamePlayController.Instance.itemSpotsManager.RemoveItemFromMergeData(item);
        itemSpot.Clear();
        item.ResetAll();
    }
    
}