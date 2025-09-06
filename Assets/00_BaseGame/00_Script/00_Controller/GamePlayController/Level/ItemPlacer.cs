using System;
using System.Collections;
using System.Collections.Generic;
using _00_BaseGame._00_Script._00_Controller.Datas;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct ItemLevelData
{
    public ItemCollectionSO itemCollection;
    public EItemName itemName;
    public EItemColor color;
    public bool isGoal;
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
    [Header("Elements")]
    [SerializeField] private List<ItemLevelData> lsItemDatas = new();
    [SerializeField] private BoxCollider spawnZone;
    [SerializeField] private int seed;

    private void Start()
    {
        GenerateItem();
    }

    public ItemLevelData[] GetGoal()
    {
        List<ItemLevelData> goals = new List<ItemLevelData>();
        foreach (var data in this.lsItemDatas)
        {
            if(data.isGoal)
                goals.Add(data);
        }
        return goals.ToArray();
    }
    
    
    [Button("Generate Item", ButtonSizes.Large)]
    private void GenerateItem()
    {
        // Xóa các item cũ
        while (transform.childCount > 0)
        {
            var t = transform.GetChild(0);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }
        
        Random.InitState(seed);
        
        for (int i = 0; i < lsItemDatas.Count; i++)
        {
            var itemDataClone = lsItemDatas[i];
            
            // Lấy thông tin item từ collection
            var itemData = itemDataClone.itemCollection.GetItemByName(itemDataClone.itemName);
            if (itemData == null)
            {
                Debug.LogError($"Item {itemDataClone.itemName} not found in collection {itemDataClone.itemCollection.name}");
                continue;
            }
            
            Item itemPrefab = itemData.Value.itemPrfab;
            Texture2D targetTexture = null;
            
            // Tìm texture tương ứng với màu
            foreach (var textureInfo in itemData.Value.textureInfos)
            {
                if (textureInfo.color == itemDataClone.color)
                {
                    targetTexture = textureInfo.texture;
                    break;
                }
            }
            
            if (targetTexture == null)
            {
                Debug.LogError($"Texture for color {itemDataClone.color} not found for item {itemDataClone.itemName}");
                continue;
            }
            
            // Sinh các item
            for (int j = 0; j < itemDataClone.amount; j++)
            {
                Vector3 spawnPosition = GetSpawnPosition();
                Item itemInstance = PrefabUtility.InstantiatePrefab(itemPrefab, transform) as Item;
                
                if (itemInstance != null)
                {
                    itemInstance.transform.position = spawnPosition;
                    itemInstance.transform.rotation = Quaternion.Euler(Random.onUnitSphere * 360);
                    
                    // Set material với texture tương ứng
                    SetItemMaterial(itemInstance, itemDataClone.itemCollection.sharedMaterial, targetTexture);
                }
            }
        }
    }

    private void SetItemMaterial(Item item, Material sharedMaterial, Texture2D texture)
    {
        if (item != null && sharedMaterial != null && texture != null)
        {
            Material newMaterial = new Material(sharedMaterial);
            newMaterial.mainTexture = texture;
            item.SetMaterialDefault(newMaterial);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float x = Random.Range(-spawnZone.size.x / 2, spawnZone.size.x / 2);
        float y = Random.Range(-spawnZone.size.y / 2, spawnZone.size.y / 2);
        float z = Random.Range(-spawnZone.size.z / 2, spawnZone.size.z / 2);
        
        var localPosition = spawnZone.center + new Vector3(x, y, z);
        var spawnPosition = transform.TransformPoint(localPosition);
        return spawnPosition;
    }
}