
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleEntityView : SimpleView
{
    public TMP_Text NameText;
    public ListView FieldView;
    public ListView FormationListView;
    
    [SerializeField] public RectTransform _fieldTransform;
    [SerializeField] private RectTransform FieldShowPivot;
    [SerializeField] private RectTransform FieldHidePivot;

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
        => DOTween.Sequence()
            .Append(_fieldTransform.DOAnchorPos(FieldShowPivot.anchoredPosition, 0.15f)
                .From(FieldHidePivot.anchoredPosition).SetDelay(0.6f).SetEase(Ease.OutQuad)
            );

    public Tween HideAnimation()
        => DOTween.Sequence();
}
