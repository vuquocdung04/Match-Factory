
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private ItemPlacer itemPlacer;

    public ItemLevelData[] GetGoals()
    {
        return itemPlacer.GetGoal();
    }
}
