
using _00_BaseGame._00_Script._00_Controller.Datas;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateLevelItem : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private LevelDataSO levelDataSO;
    [SerializeField] private ItemCollectionSO itemCollection;
    [SerializeField] private int levelIndex;
    [SerializeField] private BoxCollider spawnZone;
    [SerializeField] private int seed;

    private void Start()
    {
        //levelDataSO = GameController.Instance.dataContains.LevelDataSO;
        GenerateItem();
    }

    public ItemLevelData[] GetGoal()
    {
        return levelDataSO.GetGoals(levelIndex);
    }
    private void GenerateItem()
    {
        //var itemCollection = GameController.Instance.dataContains.ItemCollection;
        // lay item
        var levelItems = levelDataSO.GetLevelItems(levelIndex);
        Random.InitState(seed);
        foreach (var itemDataClone in levelItems)
        {
            // lay thong tin tu collection
            var itemData = itemCollection!.GetItemByName(itemDataClone.itemName);
            var itemPrefab = itemData?.itemPrfab;
            Texture2D targetTexture = null;


            foreach (var textureInfo in itemData?.textureInfos!)
            {
                if (textureInfo.color == itemDataClone.color)
                {
                    targetTexture = textureInfo.texture;   
                    break;
                }
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
                    SetItemMaterial(itemInstance, itemCollection.sharedMaterial, targetTexture);
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