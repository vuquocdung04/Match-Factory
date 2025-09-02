
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private float goUpDistance;
    [SerializeField] private float goUpDuration;
    [SerializeField] private Ease goUpEase;
    public void Init()
    {
        ItemSpotsManager.mergeStarted += OnMergeStared;
    }

    private void OnDestroy()
    {
        ItemSpotsManager.mergeStarted -= OnMergeStared;
    }

    private void OnMergeStared(List<Item> lsItems)
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            Vector3 targetPos = lsItems[i].transform.position + lsItems[i].transform.up * goUpDistance;

            Action callback = null;
            if (i == 0)
                callback = () => SmartItems(lsItems);
            
            lsItems[i].transform.DOMove(targetPos, goUpDuration).SetEase(goUpEase).OnComplete(delegate
            {
                callback?.Invoke();
            });
        }
    }

    private void SmartItems(List<Item> lsItems)
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            Destroy(lsItems[i].gameObject);
        }
    }
}
