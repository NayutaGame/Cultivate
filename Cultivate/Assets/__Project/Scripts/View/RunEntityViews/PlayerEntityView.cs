
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
        
        EntityModel entity = Get<EntityModel>();

        SkillList.Refresh();
        FormationList.Refresh();
    }

    public void Sync()
    {
        SkillList.Sync();
        FormationList.Sync();
    }

    #region IInteractable

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        if (!(from is HandSkillInteractBehaviour))
            return;
        RunEnvironment env = RunManager.Instance.Environment;
        RunSkill toEquip = from.GetSimpleView().Get<RunSkill>();
        SkillSlot slot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.EquipProcedure(toEquip, slot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal());
        
        {
            // Equip Animation
            ExtraBehaviourGhost ghost = from.GetCLView().GetExtraBehaviour<ExtraBehaviourGhost>();
            ExtraBehaviourPivot pivot = to.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            ghost.FromDrop();
            pivot.AnimateState(ghost.GetDisplayTransform(), pivot.IdleTransform);

            AudioManager.Play("CardPlacement");
        }
        
        Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }

    private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData eventData)
    {
        if (!(from is FieldSlotInteractBehaviour))
            return;

        RunEnvironment env = RunManager.Instance.Environment;
        SkillSlot fromSlot = from.GetSimpleView().Get<SkillSlot>();
        SkillSlot toSlot = to.GetSimpleView().Get<SkillSlot>();
        bool success = env.SwapProcedure(fromSlot, toSlot);
        if (!success)
            return;
        
        env.ReceiveSignalProcedure(new FieldChangedSignal());

        {
            // Swap Animation
            eventData.pointerDrag = null;
            to.OnEndDrag(eventData);
            from.OnEndDrag(eventData);
            // from.ComplexView.AnimateBehaviour.SetStartAndPivot(to.ComplexView.PivotBehaviour.IdlePivot, from.ComplexView.PivotBehaviour.IdlePivot);

            AudioManager.Play("CardPlacement");
        }

        Refresh();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.CardPickerPanel.ClearAllSelections();
        CanvasManager.Instance.RunCanvas.RunPanelCollection.Refresh();
    }
    
    #endregion
}
