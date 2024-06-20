
using UnityEngine.EventSystems;

public class PlayerEntityView : SimpleView
{
    public ListView SkillList;
    public ListView FormationList;

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

        SkillList.Refresh();
        FormationList.Refresh();
    }

    public void Sync()
    {
        SkillList.Sync();
        FormationList.Sync();
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

        EquipStaging(from, to, isReplace);
        
        Refresh();
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private static void EquipStaging(InteractBehaviour from, InteractBehaviour to, bool isReplace)
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

        SwapStaging(from, to, isReplace);

        Refresh();
        CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private static void SwapStaging(InteractBehaviour from, InteractBehaviour to, bool isReplace)
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
}
