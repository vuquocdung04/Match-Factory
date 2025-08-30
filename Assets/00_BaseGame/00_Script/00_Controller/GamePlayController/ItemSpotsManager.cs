
using _00_BaseGame._00_Script._00_Controller.GamePlayController;
using UnityEngine;

public class ItemSpotsManager : MonoBehaviour
{
    [SerializeField] private Transform itemSpot;
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private void Awake()
    {
        InputManager.itemClicked += OnItemClicked;
    }

    private void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    private void OnItemClicked(Item item)
    {
        item.transform.SetParent(itemSpot);
        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.GetComponent<Item>().DisableShadow();
        item.GetComponent<Item>().DisablePhysic();
    }
}
