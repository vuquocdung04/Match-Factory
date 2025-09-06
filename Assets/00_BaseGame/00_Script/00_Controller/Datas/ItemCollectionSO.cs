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


        public ItemData? GetItemByName(EItemName itemName)
        {
            foreach (var item in this.items)
            {
                if (item.itemName.Equals(itemName)) return item;
            }
            return null;
        }
    }

    [System.Serializable]
    public struct ItemData
    {
        [HorizontalGroup("Split", Width = 0.3f)]
        [VerticalGroup("Split/Left")]
        [LabelWidth(50)]
        [HideLabel]
        public EItemName itemName;
        
        [VerticalGroup("Split/Left")]
        [LabelWidth(50)]
        [HideLabel]
        public Item itemPrfab;
        
        [HorizontalGroup("Split")]
        [VerticalGroup("Split/Right")]
        [ListDrawerSettings(ShowIndexLabels = true)]
        [LabelWidth(100)]
        public List<TextureInfo> textureInfos;
    }

    [System.Serializable]
    public struct TextureInfo
    {
        [HorizontalGroup("TextureSplit", Width = 0.4f)]
        [HideLabel]
        public EItemColor color;
        
        [HorizontalGroup("TextureSplit")]
        [HideLabel]
        public Texture2D texture;
    }
}