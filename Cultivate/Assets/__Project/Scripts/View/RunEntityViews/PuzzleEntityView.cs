
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleEntityView : SimpleView
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
        
        EntityModel entity = Get<EntityModel>();

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
        RunSkill toEquip = from.GetSimpleView().Get<RunSkill>();
        SkillSlot slot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.EquipProcedure(out bool isReplace, toEquip, slot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal());

        EquipAnimation(from, to, isReplace);
        
        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private static void EquipAnimation(InteractBehaviour from, InteractBehaviour to, bool isReplace)
    {
        ExtraBehaviourPivot fromPivot = from.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        ExtraBehaviourPivot toPivot = to.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        
        // From: if ths slot has skill, To Display -> From Idle
        if (isReplace)
        {
            if (fromPivot != null)
                fromPivot.AnimateState(toPivot.GetDisplayTransform(), fromPivot.IdleTransform);
        }
        
        // Ghost
        ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
        ghost.Hide();
        
        // To: Ghost Display -> To Idle
        toPivot.AnimateState(ghost.GetDisplayTransform(), toPivot.IdleTransform);

        AudioManager.Play("CardPlacement");
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        RunEnvironment env = RunManager.Instance.Environment;
        SkillSlot fromSlot = from.GetSimpleView().Get<SkillSlot>();
        SkillSlot toSlot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.SwapProcedure(out bool isReplace, fromSlot, toSlot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal());

        SwapAnimation(from, to, isReplace);

        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private static void SwapAnimation(InteractBehaviour from, InteractBehaviour to, bool isReplace)
    {
        ExtraBehaviourPivot fromPivot = from.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        ExtraBehaviourPivot toPivot = to.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        
        // From: if ths slot has skill, To Display -> From Idle
        if (isReplace)
        {
            if (fromPivot != null)
                fromPivot.AnimateState(toPivot.GetDisplayTransform(), fromPivot.IdleTransform);
        }
        
        // Ghost
        ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
        ghost.Hide();
        
        // To: Ghost Display -> To Idle
        toPivot.AnimateState(ghost.GetDisplayTransform(), toPivot.IdleTransform);

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
