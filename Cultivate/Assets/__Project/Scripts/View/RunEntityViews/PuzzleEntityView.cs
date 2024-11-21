
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleEntityView : XView
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
        SkillList.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillList.DropNeuron.Join(Equip, Swap);
        
        FormationList.SetAddress(GetAddress().Append(".ShowingFormations"));
    }

    public override void Refresh()
    {
        base.Refresh();
        
        IEntity entity = Get<IEntity>();

        NameText.text = $"{entity.GetJingJie()} {entity.GetEntry().GetName()}";

        SkillList.Refresh();
        FormationList.Refresh();
    }

    #region IInteractable

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill toEquip = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        bool success = env.EquipProcedure(out bool isReplace, toEquip, slot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal(DeckIndex.FromHand(), slot.ToDeckIndex()));

        EquipStaging(from, to, isReplace);
        
        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private static void EquipStaging(InteractBehaviour from, InteractBehaviour to, bool isReplace)
    {
        XBehaviourPivot fromPivot = from.GetBehaviour<XBehaviourPivot>();
        XBehaviourPivot toPivot = to.GetBehaviour<XBehaviourPivot>();
        
        // From: if ths slot has skill, To Display -> From Idle
        if (isReplace)
        {
            if (fromPivot != null)
                fromPivot.RectTransformToIdle(toPivot.GetDisplayTransform());
        }
        
        // Ghost
        XBehaviourGhost ghost = from.GetBehaviour<XBehaviourGhost>();
        
        // To: Ghost Display -> To Idle
        toPivot.RectTransformToIdle(ghost.GetDisplayTransform());

        AudioManager.Play("CardPlacement");
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        RunEnvironment env = RunManager.Instance.Environment;
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        bool success = env.SwapProcedure(out bool isReplace, fromSlot, toSlot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal(fromSlot.ToDeckIndex(), toSlot.ToDeckIndex()));

        SwapStaging(from, to, isReplace);

        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private static void SwapStaging(InteractBehaviour from, InteractBehaviour to, bool isReplace)
    {
        XBehaviourPivot fromPivot = from.GetBehaviour<XBehaviourPivot>();
        XBehaviourPivot toPivot = to.GetBehaviour<XBehaviourPivot>();
        
        // From: if ths slot has skill, To Display -> From Idle
        if (isReplace)
        {
            if (fromPivot != null)
                fromPivot.RectTransformToIdle(toPivot.GetDisplayTransform());
        }
        
        // Ghost
        XBehaviourGhost ghost = from.GetBehaviour<XBehaviourGhost>();
        
        // To: Ghost Display -> To Idle
        toPivot.RectTransformToIdle(ghost.GetDisplayTransform());

        AudioManager.Play("CardPlacement");
    }

    #endregion

    public Tween ShowAnimation()
        => DOTween.Sequence().AppendInterval(0.3f)
            .Append(SkillListTransform.DOAnchorPos(SkillListShowPivot.anchoredPosition, 0.15f)
                .From(SkillListHidePivot.anchoredPosition).SetEase(Ease.OutQuad))
            .Join(SkillListCanvasGroup.DOFade(1, 0.15f)
                .From(0).SetEase(Ease.OutQuad));

    public Tween HideAnimation()
        => DOTween.Sequence();
}
