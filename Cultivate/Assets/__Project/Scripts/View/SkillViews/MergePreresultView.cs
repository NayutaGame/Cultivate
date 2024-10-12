
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class MergePreresultView : SimpleView
{
    private MergePreresult _mergePreresult;
    private Animator _animator;
    
    [SerializeField] private Image CardImage;
    [SerializeField] private TMP_Text CostText;
    [SerializeField] private Image CostIcon;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Image JingJieImage;
    [SerializeField] private Image EffectImage;

    [SerializeField] private TMP_Text MergeTypeText;
    [SerializeField] private TMP_Text ErrorMessage;
    
    // private bool _highlight;
    // private Tween _highlightHandle;
    // private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");
    //
    // private Material _outlineMaterial;
    // private Material _dissolveMaterial;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();
    
        // if (EffectImage != null)
        // {
        //     EffectImage.material = Instantiate(EffectImage.material);
        //     _outlineMaterial = EffectImage.materialForRendering;
        // }

        _animator ??= InitAnimator();
    }

    private Animator InitAnimator()
    {
        // 0 for hide, 1 for show, 2 for success merge
        Animator animator = new(3);
        animator[0, 1] = ShowTween;
        animator[-1, 0] = HideTween;
        animator[-1, 2] = SuccessTween;
        return animator;
    }
    
    public MergePreresult GetMergePreresult() => _mergePreresult;
    public async UniTask SetMergePreresultAsync(int state, MergePreresult mergePreresult)
    {
        _mergePreresult = mergePreresult;

        switch (state)
        {
            case 0:
                _animator.SetStateAsync(0);
                return;
                break;
            case 1:
                _animator.SetStateAsync(1);
                break;
            case 2:
                await _animator.SetStateAsync(2);
                _animator.SetStateAsync(0);
                return;
                break;
        }
        
        Refresh();
    }
    
    public override void Refresh()
    {
        base.Refresh();
        
        SetSpriteFromMergePreresult();
        SetCostDescriptionFromMergePreresult();
        SetNameFromMergePreresult();
        SetDescriptionFromMergePreresult();
        // SetSkillTypeCompositeFromMergePreresult();
        SetJingJieSpriteFromMergePreresult();
        SetMergeTypeTextFromMergePreresult();
        SetErrorMessageFromMergePreresult();
    }

    public Tween ShowTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasGroup.DOFade(1, 0.3f))
            .Join(DOTween.Sequence()
                .Append(RectTransform.DOScale(1f, 0.3f)))
                .Append(RectTransform.DOScale(0.9f, 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo));

    public Tween HideTween()
        => DOTween.Sequence()
            .Append(CanvasGroup.DOFade(0, 0.3f))
            .Join(RectTransform.DOScale(0.6f, 0.3f))
            .AppendCallback(() => gameObject.SetActive(false));

    public Tween SuccessTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasGroup.DOFade(1, 0.15f))
            .Join(RectTransform.DOScale(1f, 0.15f).SetEase(Ease.InQuad))
            // .AppendInterval(0.1f)
            .Append(RectTransform.DOScale(1.2f, 0.15f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo))
            .AppendInterval(0.1f);
    
    // public void SetHighlight(bool highlight)
    // {
    //     _highlight = highlight;
    //     
    //     _highlightHandle?.Kill();
    //     _highlightHandle = DOTween.To(GetOutlineFade, SetOutlineFade, _highlight ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad);
    //     _highlightHandle.SetAutoKill().Restart();
    // }
    //
    // private float GetOutlineFade() => _outlineMaterial.GetFloat(OuterOutlineFade);
    // private void SetOutlineFade(float value) => _outlineMaterial.SetFloat(OuterOutlineFade, value);
    
    protected virtual void SetSpriteFromMergePreresult()
    {
        if (!_mergePreresult.Valid)
        {
            CardImage.sprite = Encyclopedia.SpriteCategory["无法合成"].Sprite;
            return;
        }
        
        if (_mergePreresult.ResultEntry != null)
        {
            CardImage.sprite = _mergePreresult.ResultEntry.GetSprite();
            return;
        }

        if (_mergePreresult.ResultWuXing != null)
        {
            WuXing wuXing = _mergePreresult.ResultWuXing.Value;
            CardImage.sprite = Encyclopedia.SpriteCategory[$"{wuXing._name}合成"].Sprite;
            return;
        }

        CardImage.sprite = Encyclopedia.SpriteCategory["可以合成"].Sprite;
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
        switch (costDescription.Type)
        {
            case CostDescription.CostType.Empty:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[0];
                CostText.text = "";
                break;
            case CostDescription.CostType.Mana:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[1];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Health:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[2];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Channel:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[3];
                CostText.text = costDescription.Value.ToString();
                break;
            case CostDescription.CostType.Armor:
                CostIcon.sprite = CanvasManager.Instance.CostIconSprites[4];
                CostText.text = costDescription.Value.ToString();
                break;
        }

        int? i = null;
        switch (costDescription.State)
        {
            case CostResult.CostState.Unwritten:
                i = 0;
                break;
            case CostResult.CostState.Normal:
                i = 1;
                break;
            case CostResult.CostState.Reduced:
                i = 2;
                break;
            case CostResult.CostState.Shortage:
                i = 3;
                break;
        }

        if (i.HasValue)
        {
            CostText.color = CanvasManager.Instance.CostColors[i.Value];
        }
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

    protected virtual void SetMergeTypeTextFromMergePreresult()
    {
        MergeTypeText.text = _mergePreresult.MergeType;
    }

    protected virtual void SetErrorMessageFromMergePreresult()
    {
        ErrorMessage.text = _mergePreresult.Valid ? "" : _mergePreresult.ErrorMessage;
    }
}
