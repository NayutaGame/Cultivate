
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleEntityView : XView
{
    public ListView FieldView;
    public ListView FormationList;
    
    [SerializeField] public RectTransform FieldViewTransform;
    [SerializeField] private RectTransform FieldViewShowPivot;
    [SerializeField] private RectTransform FieldViewHidePivot;
    [SerializeField] private CanvasGroup FieldViewCanvasGroup;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FormationList.SetAddress(GetAddress().Append(".ActiveFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        IEntity entity = Get<IEntity>();

        if (entity == null)
            return;

        // NameText.text = $"{entity.GetModel().GetName()}";
        
        // SetModel(entity.GetModel().RunModel);

        FieldView.Sync();
        FormationList.Sync();
    }

    public Tween ShowTween()
        => DOTween.Sequence().AppendInterval(0.3f)
            .Append(FieldViewTransform.DOAnchorPos(FieldViewShowPivot.anchoredPosition, 0.15f)
                .From(FieldViewHidePivot.anchoredPosition).SetEase(Ease.OutQuad))
            .Join(FieldViewCanvasGroup.DOFade(1, 0.15f)
                .From(0).SetEase(Ease.OutQuad));

    public Tween HideTween()
        => DOTween.Sequence();

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
    {
        SkillSlot slot = ib.Get<SkillSlot>();
        if (slot.Skill != null)
            AudioManager.Play("CardHover");
    }
}
