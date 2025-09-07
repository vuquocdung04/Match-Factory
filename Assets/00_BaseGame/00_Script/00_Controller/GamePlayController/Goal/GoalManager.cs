using EventDispatcher;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private ItemLevelData[] goals;

    public void Init()
    {
        OnLevelSpawned();
        ItemSpotsManager.itemPickedUp += OnItemPickedUp;
        this.RegisterListener(EventID.ITEM_VACUUMED, OnItemVacuumed); // ← Thêm dòng này
    }

    private void OnDestroy()
    {
        ItemSpotsManager.itemPickedUp -= OnItemPickedUp;
        this.RemoveListener(EventID.ITEM_VACUUMED, OnItemVacuumed); // ← Thêm dòng này
    }

    private void OnItemPickedUp(Item item)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (!goals[i].itemName.Equals(item.ItemName))
                continue;

            goals[i].amount--;
            Debug.Log(goals[i].amount);
            
            GoalCard.onGoalUpdated?.Invoke(goals[i].itemName, goals[i].amount);

            if (goals[i].amount <= 0)
            {
                GoalCard.onDone?.Invoke(goals[i].itemName);
                CompleteGoal(i);
            }
            break;
        }
    }

    private void OnItemVacuumed(object param)
    {
        if (param is EItemName itemName)
        {
            for (int i = 0; i < goals.Length; i++)
            {
                if (goals[i].itemName == itemName)
                {
                    goals[i].amount--;
                    Debug.Log($"Vacuumed: {itemName}, Remaining: {goals[i].amount}");
                
                    GoalCard.onGoalUpdated?.Invoke(goals[i].itemName, goals[i].amount);

                    if (goals[i].amount <= 0)
                    {
                        GoalCard.onDone?.Invoke(goals[i].itemName);
                        CompleteGoal(i);
                    }
                    break;
                }
            }
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