
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GenerateLevelItem generateLevelItem;
    private int levelIndex;
    public void Init()
    {
        LoadData();
    }

    private void LoadData()
    {
        levelIndex = UseProfile.CurrentLevel;
    }

    private void SaveData()
    {
        UseProfile.CurrentLevel = levelIndex;
    }

    public ItemLevelData[] GetGoals()
    {
        return generateLevelItem.GetGoal();
    }
}