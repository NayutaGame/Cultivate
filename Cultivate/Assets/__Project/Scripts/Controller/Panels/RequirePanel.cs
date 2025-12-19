
using System;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RequirePanel : Panel
{
    [SerializeField] public ListView Requirements;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private CLButtonPatternA SubmitButton;
    
    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        Requirements.SetAddress(_address.Append(".Requirements"));
        Requirements.NeuronBundle.DropNeuron.Join(MoveSkill);
        Requirements.NeuronBundle.BeginDragNeuron.Join(DragBeginRunSkill);
        Requirements.NeuronBundle.EndDragNeuron.Join(DragEndRunSkill);
        Requirements.NeuronBundle.DroppingNeuron.Join(DragEndRunSkill);
    }

    public override void Refresh()
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        RequireCell cell = cellAdapter.AsCell() as RequireCell;
        
        Requirements.Sync();
        ContentText.text = cell.GetDetailedText();
    }

    private void OnEnable()
    {
        SubmitButton.LeftClickNeuron.Add(ConfirmSelections);
        RunManager.Instance.Environment.SubmitFromHandNeuron.Add(SubmitFromHandStaging);
        RunManager.Instance.Environment.SubmitFromFieldNeuron.Add(SubmitFromFieldStaging);
        RunManager.Instance.Environment.WithdrawToHandNeuron.Add(WithdrawToHandStaging);
        RunManager.Instance.Environment.WithdrawToFieldNeuron.Add(WithdrawToFieldStaging);
        RunManager.Instance.Environment.RequirementSwapNeuron.Add(RequirementSwapStaging);
        
        RunManager.Instance.Environment.SkillMovedNeuron.Add(RefreshContentText);
    }

    private void OnDisable()
    {
        SubmitButton.LeftClickNeuron.Remove(ConfirmSelections);
        RunManager.Instance.Environment.SubmitFromHandNeuron.Remove(SubmitFromHandStaging);
        RunManager.Instance.Environment.SubmitFromFieldNeuron.Remove(SubmitFromFieldStaging);
        RunManager.Instance.Environment.WithdrawToHandNeuron.Remove(WithdrawToHandStaging);
        RunManager.Instance.Environment.WithdrawToFieldNeuron.Remove(WithdrawToFieldStaging);
        RunManager.Instance.Environment.RequirementSwapNeuron.Remove(RequirementSwapStaging);
        
        RunManager.Instance.Environment.SkillMovedNeuron.Remove(RefreshContentText);
    }

    private void RefreshContentText(SkillMovedDetails d)
    {
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        RequireCell cell = cellAdapter.AsCell() as RequireCell;
        ContentText.text = cell.GetDetailedText();
    }

    private void DragBeginRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        RequirementSlot slot = ib.Get<RequirementSlot>();
        if (!slot.IsOccupied())
            return;
        CanvasManager.Instance.RunCanvas.DragBeginRunSkill.Invoke(slot.Skill);
    }

    private void DragEndRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.RunCanvas.DragEndRunSkill.Invoke();
    }
    
    private void ConfirmSelections(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.ConfirmDeckSelectionsProcedure();
    }

    #region MoveSkillRelated

    private void MoveSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = CanvasManager.Instance.RunCanvas.GetDeckIndex(from);
        IDeckIndex toIndex = CanvasManager.Instance.RunCanvas.GetDeckIndex(to);
        if (fromIndex == null || toIndex == null)
            return;

        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, toIndex);
    }
    
    private void SubmitFromHandStaging(SubmitFromHandDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        if (d.IsReplace)
        {
            SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            deckPanel.HandView.Modified(d.FromDeckIndex.Index);
        }
        else
        {
            SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.GetAnimator().SetStateAsync(1);
            
            deckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
    }
    
    private void SubmitFromFieldStaging(SubmitFromFieldDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        if (d.IsReplace)
        {
            SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            deckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            Requirements.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            SlotView from = deckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            deckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            Requirements.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
    }
    
    private void WithdrawToHandStaging(WithdrawToHandDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        deckPanel.HandView.AddItem();
        
        SlotView from = SkillItemFromDeckIndex(d.DeckIndex);
        SlotView to = deckPanel.LatestSkillItem();
        
        Requirements.Modified(d.DeckIndex.Index);
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        from.GetAnimator().SetStateAsync(1);
        
        AudioManager.Play("CardPlacement");
    }

    private void WithdrawToFieldStaging(WithdrawToFieldDetails d)
    {
        DeckPanel deckPanel = CanvasManager.Instance.RunCanvas.DeckPanel;
        if (d.IsReplace)
        {
            SlotView from = SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = deckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            Requirements.Modified(d.FromDeckIndex.Index);
            deckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            SlotView from = SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = deckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            Requirements.Modified(d.FromDeckIndex.Index);
            deckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
    }
    
    private void RequirementSwapStaging(RequirementSwapDetails d)
    {
        if (d.IsReplace)
        {
            SlotView from = SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            Requirements.Modified(d.FromDeckIndex.Index);
            Requirements.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            SlotView from = SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            Requirements.Modified(d.FromDeckIndex.Index);
            Requirements.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
    }
    
    private SlotView SkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.Region == SkillRegion.Requirement)
            return Requirements.ViewFromIndex(deckIndex.Index);

        throw new NotImplementedException();
    }

    #endregion
}
