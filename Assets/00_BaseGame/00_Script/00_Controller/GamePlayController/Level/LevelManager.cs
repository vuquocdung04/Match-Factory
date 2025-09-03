
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Level[] levels;
    private int levelIndex;
    private Level currentLevel;

    [Header("Action")]
    public static Action<Level> levelSpawned;
    public void Init()
    {
        LoadData();
    }

    private void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        transform.Clear();
        
        int validateLevelIndex = levelIndex % levels.Length;
            
        currentLevel = Instantiate(levels[validateLevelIndex], transform);
        
        levelSpawned?.Invoke(currentLevel);
    }

    private void LoadData()
    {
        levelIndex = UseProfile.CurrentLevel;
    }

    private void SaveData()
    {
        UseProfile.CurrentLevel = levelIndex;
    }
    
    
}
