
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostView : XView
{
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private Image CostIcon;
    
    private JingJie _showingJingJie;

    public override void Refresh()
    {
        base.Refresh();

        AnnotatableCost cost = Get<AnnotatableCost>();
        SetShowingJingJie(cost.GetJingJie());
    }

    public void SetShowingJingJie(JingJie jingJie)
    {
        _showingJingJie = jingJie;

        AnnotatableCost cost = Get<AnnotatableCost>();
        SetCostDescription(cost.GetLiteralCostDescription(_showingJingJie));
    }

    private void SetCostDescription(CostDescription costDescription)
    {
        CostIcon.gameObject.SetActive(costDescription.Type != CostType.Empty);
        
        switch (costDescription.Type)
        {
            case CostType.Empty:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[0];
                CostText.text = "";
                break;
            case CostType.Mana:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[1];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostType.Health:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[2];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostType.Channel:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[3];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostType.Armor:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[4];
                CostText.text = costDescription.Value.ToString();
                break;
        }
        
        int? i = null;
        switch (costDescription.State)
        {
            case CostState.Unwritten:
                i = 0;
                break;
            case CostState.Normal:
                i = 1;
                break;
            case CostState.Reduced:
                i = 2;
                break;
            case CostState.Shortage:
                i = 3;
                break;
        }
        
        if (i.HasValue)
        {
            CostText.color = CanvasManager.Instance.CostColors[i.Value];
        }
    }
}