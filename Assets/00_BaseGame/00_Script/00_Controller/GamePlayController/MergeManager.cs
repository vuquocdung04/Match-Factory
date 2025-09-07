
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    [Header("Go Up Setting"), Space(5)]
    [SerializeField] private float goUpDistance;
    [SerializeField] private float goUpDuration;
    [SerializeField] private Ease goUpEase;
    
    [Header("Smash Setting"), Space(5)]
    [SerializeField] private float smashDuration;
    [SerializeField] private Ease smashEase;
    
    [Header("Effect"), Space(5)]
    [SerializeField] private ParticleSystem mergeParticles;
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
                callback = () => SmashItems(lsItems);
            
            lsItems[i].transform.DOMove(targetPos, goUpDuration).SetEase(goUpEase).OnComplete(delegate
            {
                callback?.Invoke();
            });
        }
    }

    private void SmashItems(List<Item> lsItems)
    {
        // Sort the items from left to right
        lsItems.Sort((a,b) => a.transform.position.x.CompareTo(b.transform.position.x));
        // 0 move right , 2 move left
        float targetX =  lsItems[1].transform.position.x;
        lsItems[0].transform.DOMoveX(targetX, smashDuration).SetEase(smashEase).OnComplete(delegate
        {
            FinalizeMerge(lsItems);
        });
        lsItems[2].transform.DOMoveX(targetX, smashDuration).SetEase(smashEase);

    }

    private void FinalizeMerge(List<Item> lsItems)
    {
        for (int i = 0; i < lsItems.Count; i++)
        {
            lsItems[i].DestroyItem();
        }
        
        ParticleSystem particles = Instantiate(mergeParticles, lsItems[1].transform.position, Quaternion.identity, transform);
        particles.Play();
    }
}
