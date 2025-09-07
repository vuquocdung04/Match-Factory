using DG.Tweening;
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
    public static System.Action<EItemName> onDone;
    private EItemName itemName;
    
    public void Init()
    {
        // Đăng ký sự kiện
        onGoalUpdated += HandleGoalUpdated;
        onDone += HandleGoalCompleted;
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện
        onGoalUpdated -= HandleGoalUpdated;
        onDone -= HandleGoalCompleted;
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
            transform.DOScale(new Vector3(0.9f,0.9f,0.9f), 0.05f).OnComplete(delegate
            {
                transform.localScale = Vector3.one;
            });
        }
    }

    private void HandleGoalCompleted(EItemName completedItemName)
    {
        if (completedItemName != itemName) return;
    
        var sequence = DOTween.Sequence()
            .Append(transform.DORotate(new Vector3(0, -90, 0), 0.3f))
            .AppendCallback(() => backTransform.gameObject.SetActive(true))
            .Append(transform.DORotate(new Vector3(0, -270, 0), 0.3f))
            .AppendCallback(() => backTransform.gameObject.SetActive(false))
            .Append(transform.DORotate(new Vector3(0, -450, 0), 0.3f))
            .AppendCallback(() => backTransform.gameObject.SetActive(true))
            .Append(transform.DORotate(new Vector3(0,-540,0), 0.3f))
            .Append(transform.DOScale(1.1f, 0.5f))
            .Append(transform.DOScale(0, 0.3f))
            .OnComplete(() => gameObject.SetActive(false));
    }
}
