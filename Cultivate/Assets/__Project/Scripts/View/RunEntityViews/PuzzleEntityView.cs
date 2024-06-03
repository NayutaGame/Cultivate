
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PuzzleEntityView : SimpleView
{
    public TMP_Text NameText;
    public ListView FieldView;
    public ListView FormationListView;
    
    [SerializeField] public RectTransform _fieldTransform;
    [SerializeField] private RectTransform FieldShowPivot;
    [SerializeField] private RectTransform FieldHidePivot;
    [SerializeField] private CanvasGroup FieldCanvasGroup;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        FieldView.SetAddress(GetAddress().Append(".Slots"));
        FormationListView.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        EntityModel entity = Get<EntityModel>();

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";

        FieldView.Refresh();
        FormationListView.Refresh();
    }

    public Tween ShowAnimation()
        => DOTween.Sequence().AppendInterval(0.3f)
            .Append(_fieldTransform.DOAnchorPos(FieldShowPivot.anchoredPosition, 0.15f)
                .From(FieldHidePivot.anchoredPosition).SetEase(Ease.OutQuad))
            .Join(FieldCanvasGroup.DOFade(1, 0.15f)
                .From(0).SetEase(Ease.OutQuad));

    public Tween HideAnimation()
        => DOTween.Sequence();
}
