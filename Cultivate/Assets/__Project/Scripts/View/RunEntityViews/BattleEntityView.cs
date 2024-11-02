
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleEntityView : SimpleView
{
    public TMP_Text NameText;
    public ListView SkillList;
    public ListView FormationList;
    
    [SerializeField] public RectTransform SkillListTransform;
    [SerializeField] private RectTransform SkillListShowPivot;
    [SerializeField] private RectTransform SkillListHidePivot;
    [SerializeField] private CanvasGroup SkillListCanvasGroup;

    [SerializeField] private Transform Anchor;
    private PrefabEntry CurrPrefabEntry;
    private GameObject Model;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        
        SkillList.SetAddress(GetAddress().Append(".Slots"));
        FormationList.SetAddress(GetAddress().Append(".ActiveFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        IEntity entity = Get<IEntity>();

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";
        
        RefreshModel(entity);

        SkillList.Refresh();
        FormationList.Refresh();
    }

    private void RefreshModel(IEntity entity)
    {
        PrefabEntry targetPrefabEntry = entity.GetEntry().GetUIEntityModelPrefabEntry();
        if (CurrPrefabEntry == targetPrefabEntry)
            return;
        
        if (Model != null)
            Destroy(Model);

        CurrPrefabEntry = targetPrefabEntry;
        Model = Instantiate(CurrPrefabEntry.Prefab, Anchor);
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
