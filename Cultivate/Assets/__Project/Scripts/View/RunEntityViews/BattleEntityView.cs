
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleEntityView : LegacySimpleView
{
    public TMP_Text NameText;
    public ListView FieldView;
    public ListView FormationList;
    
    [SerializeField] public RectTransform FieldViewTransform;
    [SerializeField] private RectTransform FieldViewShowPivot;
    [SerializeField] private RectTransform FieldViewHidePivot;
    [SerializeField] private CanvasGroup FieldViewCanvasGroup;

    [SerializeField] private Transform Anchor;
    private PrefabEntry CurrPrefabEntry;
    private GameObject Model;

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

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";
        
        SetModel(entity.GetEntry().GetUIEntityModelPrefabEntry());

        FieldView.Sync();
        FormationList.Sync();
    }

    private void SetModel(PrefabEntry targetPrefabEntry)
    {
        if (CurrPrefabEntry == targetPrefabEntry)
            return;
        
        if (Model != null)
            Destroy(Model);

        CurrPrefabEntry = targetPrefabEntry;
        Model = Instantiate(CurrPrefabEntry.Prefab, Anchor);
    }

    public Tween ShowTween()
        => DOTween.Sequence().AppendInterval(0.3f)
            .Append(FieldViewTransform.DOAnchorPos(FieldViewShowPivot.anchoredPosition, 0.15f)
                .From(FieldViewHidePivot.anchoredPosition).SetEase(Ease.OutQuad))
            .Join(FieldViewCanvasGroup.DOFade(1, 0.15f)
                .From(0).SetEase(Ease.OutQuad));

    public Tween HideTween()
        => DOTween.Sequence();
}
