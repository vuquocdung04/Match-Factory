using EventDispatcher;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private ItemLevelData[] goals;

    public void Init()
    {
        OnLevelSpawned();
        ItemSpotsManager.itemPickedUp += OnItemPickedUp;
        this.RegisterListener(EventID.ITEM_VACUUMED, OnItemVacuumed);
    }

    private void OnDestroy()
    {
        ItemSpotsManager.itemPickedUp -= OnItemPickedUp;
        this.RemoveListener(EventID.ITEM_VACUUMED, OnItemVacuumed);
    }

    private void OnItemPickedUp(Item item)
    {
        ProcessItemGoal(item.ItemName, "Picked up");
    }

    private void OnItemVacuumed(object param)
    {
        if (param is EItemName itemName)
        {
            ProcessItemGoal(itemName, "Vacuumed");
        }
    }

    public void OnItemSpringed(EItemName itemNameParam)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].itemName == itemNameParam)
            {
                goals[i].amount++;
                GoalCard.onGoalUpdated?.Invoke(goals[i].itemName, goals[i].amount);
            }
        }
    }

    // Method chung xử lý logic goal
    private void ProcessItemGoal(EItemName itemName, string actionType = "")
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].itemName != itemName) continue;

            goals[i].amount--;
            Debug.Log($"{actionType}: {itemName}, Remaining: {goals[i].amount}");

            GoalCard.onGoalUpdated?.Invoke(goals[i].itemName, goals[i].amount);

            if (goals[i].amount <= 0)
            {
                GoalCard.onDone?.Invoke(goals[i].itemName);
                CompleteGoal(i);
            }

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
        Debug.Log("Goals count: " + goals.Length);

        // Log chi tiết để debug
        for (int i = 0; i < goals.Length; i++)
        {
            Debug.Log($"Goal {i}: {goals[i].itemName}, Amount: {goals[i].amount}");
        }
    }
}