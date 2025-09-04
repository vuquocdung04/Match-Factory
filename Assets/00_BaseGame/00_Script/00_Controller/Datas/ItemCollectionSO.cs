using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace _00_BaseGame._00_Script._00_Controller.Datas
{
    [CreateAssetMenu(fileName = "ItemCollectionSO", menuName = "ItemCollection", order = 0)]
    public class ItemCollectionSO : ScriptableObject
    {
        public Material sharedMaterial;
        
        [Space(5)]
        [ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true)]
        public List<ItemData> items = new List<ItemData>();
    }

    [System.Serializable]
    public struct ItemData
    {
        [FoldoutGroup("Information Base")]
        public ItemType itemType;
        
        [FoldoutGroup("Asset References")]
        public AssetReference objectReference;
        
        [FoldoutGroup("Textures")]
        public List<TextureInfo> textureInfos;
    }

    [System.Serializable]
    public struct TextureInfo
    {
        public string textureID;
        public AssetReferenceTexture2D texture;
    }

    public enum ItemType
    {
        None,
        Ball,
        Chair,
        Food,
        // Thêm các loại khác...
    }
}