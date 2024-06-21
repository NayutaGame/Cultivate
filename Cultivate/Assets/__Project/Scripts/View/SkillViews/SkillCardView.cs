
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class SkillCardView : SimpleView
{
    private JingJie _showingJingJie;
    private bool _highlight;
    
    [SerializeField] private Image CardImage;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private Image EffectImage;

    private Material _outlineMaterial;
    // private Material _dissolveMaterial;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        if (EffectImage != null)
        {
            EffectImage.material = Instantiate(EffectImage.material);
            _outlineMaterial = EffectImage.materialForRendering;
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        ISkillModel skill = Get<ISkillModel>();
        SetShowingJingJie(skill.GetJingJie());
    }

    public JingJie GetShowingJingJie()
        => _showingJingJie;

    public void SetShowingJingJie(JingJie jingJie)
    {
        _showingJingJie = jingJie;

        ISkillModel skill = Get<ISkillModel>();
        SetSprite(skill.GetSprite());
        SetCostDescription(skill.GetCostDescription(_showingJingJie));
        SetName(skill.GetName());
        SetDescription(skill.GetHighlight(_showingJingJie));
        SetSkillTypeComposite(skill.GetSkillTypeComposite());
        SetJingJieSprite(skill.GetJingJieSprite(_showingJingJie));
        SetWuXingSprite(skill.GetWuXingSprite());
    }

    private Tween _highlightHandle;
    private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");

    public void SetHighlight(bool highlight)
    {
        _highlight = highlight;
        
        _highlightHandle?.Kill();
        _highlightHandle = DOTween.To(GetOutlineFade, SetOutlineFade, _highlight ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad);
        _highlightHandle.SetAutoKill().Restart();
    }

    private float GetOutlineFade() => _outlineMaterial.GetFloat(OuterOutlineFade);
    private void SetOutlineFade(float value) => _outlineMaterial.SetFloat(OuterOutlineFade, value);

    protected virtual void SetSprite(Sprite sprite)
    {
        CardImage.sprite = sprite;
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
