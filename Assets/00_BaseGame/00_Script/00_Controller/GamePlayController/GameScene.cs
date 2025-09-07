
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GoalCardManager  goalCardManager;
    public void Init()
    {
        goalCardManager.SetGoalCards();
    }
}
