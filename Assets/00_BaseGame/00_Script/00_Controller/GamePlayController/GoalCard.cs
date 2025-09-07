using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private Transform backTransform;
    [SerializeField] private int amount;
    public static System.Action<EItemName,int> onGoalUpdated; 
    private EItemName itemName;
    
    public void Init()
    {
        // Đăng ký sự kiện
        onGoalUpdated += HandleGoalUpdated;
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện
        onGoalUpdated -= HandleGoalUpdated;
    }
    public void SetInfoGoalCard(Sprite iconSprite, int amountParam, EItemName itemName)
    {
        icon.sprite = iconSprite;
        icon.SetNativeSize();
        amount = amountParam;
        this.itemName = itemName;
        txtAmount.text = amountParam.ToString();
    }
    private void HandleGoalUpdated(EItemName updatedItemName, int newAmount)
    {
        if (updatedItemName == itemName)
        {
            amount = newAmount;
            txtAmount.text = amount.ToString();
        }
    }
    
}
