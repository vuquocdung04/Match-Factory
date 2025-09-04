
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    private ItemLevelData[] goals;
    
    public void Init()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
        ItemSpotsManager.itemPickedUp += OnItemPickedUp;
    }
    public void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
        ItemSpotsManager.itemPickedUp -= OnItemPickedUp;
    }

    private void OnItemPickedUp(Item item)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if(!goals[i].itemPrefab.ItemName.Equals(item.ItemName))
                continue;

            goals[i].amount--;

            if (goals[i].amount <= 0)
                CompleteGoal(i);
            break;
        }
    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log("Goal Index : " + goals[goalIndex].itemPrefab.ItemName);
        CheckForLevelComplete();
    }

    private void CheckForLevelComplete()
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].amount > 0)
                return;
        }
        Debug.Log("Level Complete");
    }

    private void OnLevelSpawned(Level level)
    {
        goals = level.GetGoals();
    }
}
