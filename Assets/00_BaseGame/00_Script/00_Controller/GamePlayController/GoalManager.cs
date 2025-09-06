using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private ItemLevelData[] goals;

    public void Init()
    {
        OnLevelSpawned();
        ItemSpotsManager.itemPickedUp += OnItemPickedUp;
    }

    private void OnDestroy()
    {
        ItemSpotsManager.itemPickedUp -= OnItemPickedUp;
    }

    private void OnItemPickedUp(Item item)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (!goals[i].itemName.Equals(item.ItemName))
                continue;

            goals[i].amount--;
            Debug.Log(goals[i].amount);
            if (goals[i].amount <= 0)
                CompleteGoal(i);
            break;
        }
    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log("Goal completed: " + goals[goalIndex].itemName);
        CheckForLevelComplete();
    }

    private void CheckForLevelComplete()
    {
        bool allCompleted = true;
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].amount > 0)
            {
                allCompleted = false;
                break;
            }
        }
        
        if (allCompleted)
        {
            Debug.Log("Level Complete");
            // Gọi event level complete nếu cần
        }
    }

    private void OnLevelSpawned()
    {
        var newGoals = GamePlayController.Instance.levelManager.GetGoals();
        goals = newGoals;
        Debug.Log("Goals count: " + goals.Length); // ← RỒI MỚI DÙNG
    
        // Log chi tiết để debug
        for (int i = 0; i < goals.Length; i++)
        {
            Debug.Log($"Goal {i}: {goals[i].itemName}, Amount: {goals[i].amount}");
        }
    }
}