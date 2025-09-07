
using System.Collections.Generic;
using _00_BaseGame._00_Script._00_Controller.Datas;
using UnityEngine;

public class GoalCardManager : MonoBehaviour
{
    public LevelDataSO levelData;
    public ItemCollectionSO  itemCollection;
    public List<GoalCard> lsCards = new();
    
    public void SetGoalCards()
    {
        ItemLevelData[] goals =  levelData.GetGoals(UseProfile.CurrentLevel);

        for (int i = 0; i < lsCards.Count; i++)
        {
            if (i < goals.Length)
            {
                var goal = goals[i];

                var itemData = itemCollection.GetItemByName(goal.itemName);
                Sprite icon = GetIconByColor(itemData.Value, goal.color);

                if (icon != null)
                {
                    lsCards[i].SetInfoGoalCard(icon,goal.amount,goal.itemName);
                    lsCards[i].Init();
                    lsCards[i].gameObject.SetActive(true);
                }
            }
            else
            {
                lsCards[i].gameObject.SetActive(false);
            }
        }
    }

    private Sprite GetIconByColor(ItemData itemData, EItemColor goalColor)
    {
        foreach (var texture in itemData.textureInfos)
        {
            if(texture.color == goalColor) return texture.icon;
        }
        return null;
    }
}
