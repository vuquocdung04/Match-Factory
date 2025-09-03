using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable]
public struct ItemLevelData
{
    public Item itemPrefab;
    
    [PropertyRange(3, 99)]
    [OnValueChanged("SnapToMultipleOfThree")]
    public int amount;
    
    private void SnapToMultipleOfThree()
    {
        amount = Mathf.RoundToInt(amount / 3f) * 3;
        amount = Mathf.Clamp(amount, 3, 99);
    }
}

public class ItemPlacer : MonoBehaviour
{
    [Header("Elemenet")]
    [SerializeField] private List<ItemLevelData> lsItemDatas = new();
}
