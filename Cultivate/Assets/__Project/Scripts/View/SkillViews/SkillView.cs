
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillView : XView
{
    private JingJie _showingJingJie;
    
    [SerializeField] private Image Illustration;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private Image CostIcon;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;

    private HighlightBehaviour _highlightBehaviour;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        _highlightBehaviour = GetBehaviour<HighlightBehaviour>();
    }
    
    public void SetHighlight(bool highlight)
    {
        if (_highlightBehaviour != null)
            _highlightBehaviour.SetHighlight(highlight);
    }

    // private Material _dissolveMaterial;

    public override void Refresh()
    {
        base.Refresh();

        AnnotatableSkill skill = Get<AnnotatableSkill>();
        SetShowingJingJie(skill.GetJingJie());
    }

    public JingJie GetShowingJingJie()
        => _showingJingJie;

    public void SetShowingJingJie(JingJie jingJie)
    {
        _showingJingJie = jingJie;

        AnnotatableSkill skill = Get<AnnotatableSkill>();
        SetSprite(skill.GetSprite());
        SetCostDescription(skill.GetLiteralCostDescription(_showingJingJie));
        SetName(skill.GetName());
        SetDescription(skill.GetDescription(_showingJingJie));
        SetJingJieSprite(skill.GetJingJieSprite(_showingJingJie));
    }

    protected virtual void SetSprite(Sprite sprite)
    {
        Illustration.sprite = sprite;
    }

    protected virtual void SetCostDescription(CostDescription costDescription)
    {
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

    protected virtual void SetName(string name)
    {
        NameText.text = name;
    }

    protected virtual void SetDescription(Description description)
    {
        DescriptionText.text = description.GetHighlightedString();
    }

    protected virtual void SetJingJieSprite(Sprite jingJieSprite)
    {
        JingJieImage.sprite = jingJieSprite;
    }
}
