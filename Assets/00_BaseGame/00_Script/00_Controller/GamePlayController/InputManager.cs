using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using _00_BaseGame._00_Script._00_Controller.GamePlayController;

public class InputManager : MonoBehaviour
{
    public static Action<Item> itemClicked;
    [SerializeField] private Material outlineMaterial;
    private Item currentItem;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    private void DeselectCurrentItem()
    {
        if(currentItem)
            currentItem.Deselect();
        
        currentItem = null;
    }
    
    private void HandleMouseUp()
    {
        if(!currentItem) return;
        currentItem.Deselect();
        itemClicked?.Invoke(currentItem);
        currentItem = null;
    }

    private void HandleDrag()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit,100);
        if (!hit.collider)
        {
            DeselectCurrentItem();
            currentItem = null;
            return;
        }

        if (!hit.collider.transform.parent.TryGetComponent(out Item item))
        {
            DeselectCurrentItem();
            currentItem = null;
            return;
        }
        DeselectCurrentItem();
        currentItem = item;
        currentItem.Select(outlineMaterial);
    }
}
