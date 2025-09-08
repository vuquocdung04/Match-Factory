
using System;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float fanMagnitude = 10;
    [SerializeField] private Animator animator;
    private void OnMouseDown()
    {
        PlayingAnimation();
    }
    
    private void PlayingAnimation()
    {
        animator.Play("Activate");
        FanPowerUp();
    }

    public void FanPowerUp()
    {
        List<Item> lsItems = GamePlayController.Instance.levelManager.GetItems();
        foreach (var item in lsItems)
        {
            item.ApplyRandomForce(fanMagnitude);
        }
    }
}
