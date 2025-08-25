
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class MergePreresultView : XView
{
    private MergeTarget _mergeTarget;
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

    [SerializeField] private CanvasGroup CanvasGroup;
    
    // private bool _highlight;
    // private Tween _highlightHandle;
    // private static readonly int OuterOutlineFade = Shader.PropertyToID("_OuterOutlineFade");
    //
    // private Material _outlineMaterial;
    // private Material _dissolveMaterial;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
    
        // if (EffectImage != null)
        // {
        //     EffectImage.material = Instantiate(EffectImage.material);
        //     _outlineMaterial = EffectImage.materialForRendering;
        // }

        _animator ??= InitAnimator();
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show, 2 for success merge
        Animator animator = new(3, "Merge Preresult View");
        animator[0, 1] = ShowTween;
        animator[-1, 0] = HideTween;
        animator[-1, 2] = SuccessTween;
        return animator;
    }
    
    public MergeTarget GetMergeTarget() => _mergeTarget;
    public async UniTask SetMergeTargetAsync(int state, MergeTarget mergeTarget)
    {
        _mergeTarget = mergeTarget;

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
                .Append(GetRect().DOScale(1f, 0.3f)))
                .Append(GetRect().DOScale(0.9f, 1f).SetEase(Ease.OutQuad).SetLoops(99999, LoopType.Yoyo));

    public Tween HideTween()
        => DOTween.Sequence()
            .Append(CanvasGroup.DOFade(0, 0.3f))
            .Join(GetRect().DOScale(0.6f, 0.3f))
            .AppendCallback(() => gameObject.SetActive(false));

    public Tween SuccessTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasGroup.DOFade(1, 0.15f))
            .Join(GetRect().DOScale(1f, 0.15f).SetEase(Ease.InQuad))
            // .AppendInterval(0.1f)
            .Append(GetRect().DOScale(1.2f, 0.15f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo))
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
        if (!_mergeTarget.Valid)
        {
            CardImage.sprite = Encyclopedia.SpriteCategory.FromName("无法合成").Sprite;
            return;
        }
        
        if (_mergeTarget.ResultEntry != null)
        {
            CardImage.sprite = _mergeTarget.ResultEntry.GetSprite();
            return;
        }

        if (_mergeTarget.ResultWuXing == null || _mergeTarget.ResultWuXing == WuXing.Wu)
        {
            CardImage.sprite = Encyclopedia.SpriteCategory.FromName("可以合成").Sprite;
            return;
        }

        CardImage.sprite = _mergeTarget.ResultWuXing.GetMergeSprite();
    }
    
    protected virtual void SetCostDescriptionFromMergePreresult()
    {
        if (_mergeTarget.ResultEntry == null)
        {
            CostText.text = "";
            CostIcon.gameObject.SetActive(false);
            return;
        }

        CostIcon.gameObject.SetActive(true);
        
        CostDescription costDescription = _mergeTarget.CostDescription;
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
    
    protected virtual void SetNameFromMergePreresult()
    {
        if (_mergeTarget.ResultEntry == null)
        {
            NameText.text = "";
            return;
        }
        
        NameText.text = _mergeTarget.ResultEntry.GetName();
    }
    
    protected virtual void SetDescriptionFromMergePreresult()
    {
        if (_mergeTarget.ResultEntry == null)
        {
            DescriptionText.text = "";
            return;
        }
        
        DescriptionText.text = _mergeTarget.Description.GetHighlightedString();
    }
    
    protected virtual void SetJingJieSpriteFromMergePreresult()
    {
        JingJie jingJie = _mergeTarget.ResultJingJie ?? _mergeTarget.ResultEntry?.LowestJingJie ?? JingJie.LianQi;
        JingJieImage.sprite = CanvasManager.Instance.JingJieSprites[jingJie];
    }

    protected virtual void SetMergeTypeTextFromMergePreresult()
    {
        MergeTypeText.text = _mergeTarget.MergeType;
    }

    protected virtual void SetErrorMessageFromMergePreresult()
    {
        ErrorMessage.text = _mergeTarget.Valid ? "" : _mergeTarget.ErrorMessage;
    }
}
