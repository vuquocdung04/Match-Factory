
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GoalCardManager  goalCardManager;
    [SerializeField] private List<Transform> lsPots;
    public void Init()
    {
        goalCardManager.SetGoalCards();
    }

    public float startX = -0.5f;
    public float spacing = 1.0f; // Khoảng cách giữa các pot
    
    [Button("Setup Pot", ButtonSizes.Large)]
    void SetupPot()
    {
        for (int i = 0; i < lsPots.Count; i++)
        {
            Transform pot = lsPots[i];
            // Tính vị trí X dựa trên startX và spacing
            float xPos = startX + (i * spacing);
            pot.localPosition = new Vector3(xPos, pot.localPosition.y, pot.localPosition.z);
        }
    }
}
