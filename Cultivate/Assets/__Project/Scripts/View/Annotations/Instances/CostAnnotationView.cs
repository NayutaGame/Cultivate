
using System.Text;
using TMPro;
using UnityEngine;

public class CostAnnotationView : AnnotationView
{
    [SerializeField] private CostView CostView;
    [SerializeField] private TMP_Text Description;
    
    public override Vector3 GetCriticalDisplacement(AnnotationAlignmentDetails d)
    {
        if (d is ImageAnnotationAlignmentDetails)
        {
            return CostView.GetRect().position - GetRect().position;
        }
        
        return Vector3.zero;
    }
    
    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        AnnotationDetails d = Get<AnnotationDetails>();
        CostView.SetAddress(d.Address);
    }

    public override void Refresh()
    {
        base.Refresh();
        
        AnnotationDetails d = Get<AnnotationDetails>();
        AnnotatableCost cost = d.Address.Get<AnnotatableCost>();
        CostDescription costDescription = cost.GetLiteralCostDescription(cost.GetJingJie());

        Description.text = GetDescriptionString(costDescription);

        CostView.Refresh();
    }

    private string GetDescriptionString(CostDescription costDescription)
    {
        StringBuilder sb = new();
        switch (costDescription.Type)
        {
            case CostType.Empty:
                sb.Append("不需要任何消耗，这张牌可以直接使用。");
                break;
            case CostType.Mana:
                sb.Append($"蓝色标志代表需要消耗{costDescription.Value}灵气。");
                break;
            case CostType.Health:
                sb.Append($"红色标志代表需要消耗{costDescription.Value}生命值。");
                break;
            case CostType.Channel:
                sb.Append($"绿色标志代表需要花{costDescription.Value}回合吟唱。");
                break;
            case CostType.Armor:
                sb.Append($"黄色标志代表需要消耗{costDescription.Value}护甲值。");
                break;
        }
        sb.AppendLine();
        switch (costDescription.State)
        {
            case CostState.Reduced:
                sb.Append("数字是绿色的代表实际使用的时候节省了资源。");
                break;
            case CostState.Shortage:
                sb.Append("数字是红色的代表尝试使用时资源不足。");
                break;
        }

        return sb.ToString();
    }
}
