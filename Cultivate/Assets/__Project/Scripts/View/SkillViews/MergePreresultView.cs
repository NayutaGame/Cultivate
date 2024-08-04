
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class MergePreresultView : SimpleView
{
    private MergePreresult _mergePreresult;
    
    public MergePreresult GetMergePreresult() => _mergePreresult;
    public void SetMergePreresult(MergePreresult mergePreresult)
    {
        _mergePreresult = mergePreresult;
        Refresh();
    }
    
    private bool _highlight;
    
    [SerializeField] private Image CardImage;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image WuXingImage;
    [SerializeField] private Image EffectImage;

    [SerializeField] private TMP_Text MergeTypeText;
    [SerializeField] private TMP_Text ErrorMessage;
    
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

        if (_mergePreresult == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        gameObject.SetActive(true);
        
        SetSpriteFromMergePreresult();
        SetCostDescriptionFromMergePreresult();
        SetNameFromMergePreresult();
        SetDescriptionFromMergePreresult();
        SetSkillTypeCompositeFromMergePreresult();
        SetJingJieSpriteFromMergePreresult();
        SetWuXingSpriteFromMergePreresult();
        SetMergeTypeTextFromMergePreresult();
        SetErrorMessageFromMergePreresult();
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
    
    protected virtual void SetSpriteFromMergePreresult()
    {
        if (_mergePreresult.ResultEntry == null)
        {
            CardImage.sprite = _mergePreresult.Valid ? CanvasManager.Instance.MergePreresultValidSprite : CanvasManager.Instance.MergePreresultInvalidSprite;
            return;
        }
        
        CardImage.sprite = _mergePreresult.ResultEntry.GetSprite();
    }
    
    protected virtual void SetCostDescriptionFromMergePreresult()
    {
        if (_mergePreresult.ResultEntry == null)
        {
            CostText.text = "";
            return;
        }

        JingJie jingJie = _mergePreresult.ResultJingJie ?? _mergePreresult.ResultEntry.LowestJingJie;
        CostDescription costDescription = _mergePreresult.ResultEntry.GetCostDescription(jingJie);
        
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
    
    protected virtual void SetNameFromMergePreresult()
    {
        if (_mergePreresult.ResultEntry == null)
        {
            NameText.text = "";
            return;
        }
        
        NameText.text = _mergePreresult.ResultEntry.GetName();
    }
    
    protected virtual void SetDescriptionFromMergePreresult()
    {
        if (_mergePreresult.ResultEntry == null)
        {
            DescriptionText.text = "";
            return;
        }
        
        JingJie jingJie = _mergePreresult.ResultJingJie ?? _mergePreresult.ResultEntry.LowestJingJie;
        DescriptionText.text = _mergePreresult.ResultEntry.GetHighlight(jingJie);
    }
    
    protected virtual void SetSkillTypeCompositeFromMergePreresult()
    {
        if (_mergePreresult.ResultEntry == null)
        {
            return;
        }

        var skillTypeComposite = _mergePreresult.ResultEntry.GetSkillTypeComposite();
        
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
    
    protected virtual void SetJingJieSpriteFromMergePreresult()
    {
        JingJie jingJie = _mergePreresult.ResultJingJie ?? _mergePreresult.ResultEntry?.LowestJingJie ?? JingJie.LianQi;
        JingJieImage.sprite = CanvasManager.Instance.JingJieSprites[jingJie];
    }
    
    protected virtual void SetWuXingSpriteFromMergePreresult()
    {
        WuXing? wuXing = _mergePreresult.ResultWuXing ?? _mergePreresult.ResultEntry?.WuXing;
        Sprite wuXingSprite = CanvasManager.Instance.GetWuXingSprite(wuXing);
        
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

    protected virtual void SetMergeTypeTextFromMergePreresult()
    {
        MergeTypeText.text = _mergePreresult.MergeType;
    }

    protected virtual void SetErrorMessageFromMergePreresult()
    {
        ErrorMessage.text = _mergePreresult.Valid ? "" : _mergePreresult.ErrorMessage;
    }
}
