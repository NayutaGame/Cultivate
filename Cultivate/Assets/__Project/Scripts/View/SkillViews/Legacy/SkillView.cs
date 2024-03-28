
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[SelectionBase]
public class SkillView : SimpleView
{
    [SerializeField] private Image CardImage;
    // [SerializeField] private GameObject ManaCostView;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;

    #region Select

    [SerializeField] private Image SelectionImage;

    private Tween _selectHandle;

    private bool _selected;
    public bool IsSelected() => _selected;
    public void SetSelected(bool selected)
    {
        _selected = selected;

        _selectHandle?.Kill();
        _selectHandle = SelectionImage.DOFade(_selected ? 1 : 0, 0.15f);
        _selectHandle.Restart();
    }

    #endregion

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();

        SetCardImage(skill.GetSprite());
        SetCostDescription(skill.GetCostDescription());
        SetName(skill.GetName());
        SetDescription(skill.GetHighlight());
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetJingJieSprite(skill.GetJingJieSprite());
        SetWuXingSprite(skill.GetWuXingSprite());
    }

    protected virtual void SetCardImage(Sprite sprite)
    {
        CardImage.sprite = sprite != null ? sprite : Encyclopedia.SpriteCategory["Default"].Sprite;
    }

    protected virtual void SetCostDescription(CostDescription costDescription)
    {
        int value = costDescription.Value;
        if (value == 0)
        {
            CostText.text = "";
            return;
        }

        switch (costDescription.Type)
        {
            case CostDescription.CostType.Mana:
                CostText.text = $"{value}灵";
                break;
            case CostDescription.CostType.Health:
                CostText.text = $"{value}血";
                break;
            case CostDescription.CostType.Channel:
                CostText.text = $"{value}时";
                break;
            case CostDescription.CostType.Armor:
                CostText.text = $"{value}甲";
                break;
        }
        
        Color color = CanvasManager.Instance.ManaCostColors[0];
        switch (costDescription.State)
        {
            case CostResult.CostState.Unwritten:
                color = CanvasManager.Instance.ManaCostColors[0];
                break;
            case CostResult.CostState.Normal:
                color = CanvasManager.Instance.ManaCostColors[1];
                break;
            case CostResult.CostState.Reduced:
                color = CanvasManager.Instance.ManaCostColors[2];
                break;
            case CostResult.CostState.Shortage:
                color = CanvasManager.Instance.ManaCostColors[3];
                break;
        }
    
        CostText.color = color;
    }

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }

    protected virtual void SetDescription(string description)
    {
        DescriptionText.text = description;
    }

    protected virtual void SetSkillTypeComposite(SkillTypeComposite skillTypeComposite)
    {
        // List<SkillType> skillTypes = skillTypeComposite.ContainedSkillTypes.FirstN(TypeViews.Length).ToList();
        //
        // for (int i = 0; i < skillTypes.Count; i++)
        // {
        //     TypeViews[i].SetActive(true);
        //     TypeTexts[i].text = skillTypes[i].ToString();
        // }
        //
        // for (int i = skillTypes.Count; i < TypeViews.Length; i++)
        // {
        //     TypeViews[i].SetActive(false);
        // }
    }

    protected virtual void SetJingJieSprite(Sprite jingJieSprite)
    {
        JingJieImage.sprite = jingJieSprite;
    }

    protected virtual void SetWuXingSprite(Sprite wuXingSprite)
    {
        if (wuXingSprite != null)
        {
            WuXingImage.sprite = wuXingSprite;
            WuXingImage.enabled = true;
        }
        else
        {
            WuXingImage.enabled = false;
        }
    }
}
