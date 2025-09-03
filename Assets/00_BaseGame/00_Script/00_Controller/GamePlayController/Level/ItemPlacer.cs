using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public struct ItemLevelData
{
    public Item itemPrefab;
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
    [Header("Elemenet")]
    [SerializeField] private List<ItemLevelData> lsItemDatas = new();
    [SerializeField] private BoxCollider spawnZone;
    [SerializeField] private int seed;
    
    
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
        while (transform.childCount > 0)
        {
            var t = transform.GetChild(0);
            t.SetParent(null);
            DestroyImmediate(t.gameObject);
        }
        Random.InitState(seed);
        for (int i = 0; i < lsItemDatas.Count; i++)
        {
            var itemDataClone =  lsItemDatas[i];
            for (int j = 0; j < itemDataClone.amount; j++)
            {
                Vector3 spawnPosition = GetSpawnPosition();
                Item itemInstance = PrefabUtility.InstantiatePrefab(itemDataClone.itemPrefab,transform) as Item;
                itemInstance.transform.position = spawnPosition;
                itemInstance.transform.rotation = Quaternion.Euler(Random.onUnitSphere * 360);
            }
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
