
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private ItemPlacer itemPlacer;

    public ItemLevelData[] GetGoal()
    {
        return itemPlacer.GetGoal();
    }
}
