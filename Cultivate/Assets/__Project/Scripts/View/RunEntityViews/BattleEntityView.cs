
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleEntityView : SimpleView
{
    public TMP_Text NameText;
    public ListView SkillList;
    public ListView FormationList;
    
    [SerializeField] public RectTransform SkillListTransform;
    [SerializeField] private RectTransform SkillListShowPivot;
    [SerializeField] private RectTransform SkillListHidePivot;
    [SerializeField] private CanvasGroup SkillListCanvasGroup;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        SkillList.SetAddress(GetAddress().Append(".Slots"));
        FormationList.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        EntityModel entity = Get<EntityModel>();

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";

        SkillList.Refresh();
        FormationList.Refresh();
    }

    public Tween ShowTween()
        => DOTween.Sequence().AppendInterval(0.3f)
            .Append(SkillListTransform.DOAnchorPos(SkillListShowPivot.anchoredPosition, 0.15f)
                .From(SkillListHidePivot.anchoredPosition).SetEase(Ease.OutQuad))
            .Join(SkillListCanvasGroup.DOFade(1, 0.15f)
                .From(0).SetEase(Ease.OutQuad));

    public Tween HideTween()
        => DOTween.Sequence();
}
