using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "LevelData", order = 0)]
public class LevelDataSO : ScriptableObject
{
    public List<LevelConfiguration> lsDataLevels;
    
    // Hàm helper để lấy LevelConfiguration theo index
    private LevelConfiguration GetLevelConfiguration(int index)
    {
        foreach (var levelConfiguration in lsDataLevels)
            if(levelConfiguration.currentLevel == index) return levelConfiguration;
        return null;
    }
    
    // Hàm helper để lấy goals của level cụ thể
    public ItemLevelData[] GetGoals(int levelIndex)
    {
        var levelConfig = GetLevelConfiguration(levelIndex);
        List<ItemLevelData> goals = new List<ItemLevelData>();
        foreach (var data in levelConfig.lsLevels)
        {
            if(data.isGoal)
                goals.Add(data);
        }
        return goals.ToArray();
    }
    
    // Hàm helper để lấy tất cả items của level cụ thể
    public List<ItemLevelData> GetLevelItems(int levelIndex)
    {
        var levelConfig = GetLevelConfiguration(levelIndex);
        return levelConfig?.lsLevels ?? new List<ItemLevelData>();
    }
}

[System.Serializable]
public class LevelConfiguration
{
    public int currentLevel = 1;
    public List<ItemLevelData> lsLevels = new List<ItemLevelData>();
}

[System.Serializable]
public struct ItemLevelData
{
    public EItemName itemName;
    [ValueDropdown("GetAvailableColors")]
    public EItemColor color;
    public bool isGoal;
    [PropertyRange(3, 99)]
    [OnValueChanged("SnapToMultipleOfThree")]
    public int amount;
    
    // Method cung cấp danh sách màu cho dropdown - dùng default colors
    private ValueDropdownList<EItemColor> GetAvailableColors()
    {
        // Hiển thị tất cả màu - sẽ filter trong runtime
        var dropdown = new ValueDropdownList<EItemColor>();
        foreach (EItemColor color in System.Enum.GetValues(typeof(EItemColor)))
        {
            dropdown.Add(color.ToString(), color);
        }
        return dropdown;
    }
    
    private void SnapToMultipleOfThree()
    {
        amount = Mathf.RoundToInt(amount / 3f) * 3;
        amount = Mathf.Clamp(amount, 3, 99);
    }
}
