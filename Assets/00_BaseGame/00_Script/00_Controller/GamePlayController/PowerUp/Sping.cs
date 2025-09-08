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
        var item = itemSpot.Item;
        if(item.IsBusy) return;
        GamePlayController.Instance.itemSpotsManager.RemoveItemFromMergeData(item);
        itemSpot.Clear();
        item.ResetAll();
    }
    
}