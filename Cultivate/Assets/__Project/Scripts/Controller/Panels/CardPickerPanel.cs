
using System;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    [SerializeField] public ListView Requirements;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button ConfirmButton;
    
    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        Requirements.SetAddress(_address.Append(".Requirements"));
        Requirements.DropNeuron.Join(MoveSkill);
        Requirements.BeginDragNeuron.Join(DragBeginRunSkill);
        Requirements.EndDragNeuron.Join(DragEndRunSkill);
        Requirements.DroppingNeuron.Join(DragEndRunSkill);
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);
    }

    public override void Refresh()
    {
        CardPickerCell d = _address.Get<CardPickerCell>();
        Requirements.Sync();
        TitleText.text = d.GetTitleText();
        ContentText.text = d.GetDetailedText();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.SubmitFromHandNeuron.Add(SubmitFromHandStaging);
        RunManager.Instance.Environment.SubmitFromFieldNeuron.Add(SubmitFromFieldStaging);
        RunManager.Instance.Environment.WithdrawToHandNeuron.Add(WithdrawToHandStaging);
        RunManager.Instance.Environment.WithdrawToFieldNeuron.Add(WithdrawToFieldStaging);
        RunManager.Instance.Environment.RequirementSwapNeuron.Add(RequirementSwapStaging);
        
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Add(HighlightQualifiers);
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Add(UnhighlightQualifiers);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.SubmitFromHandNeuron.Remove(SubmitFromHandStaging);
        RunManager.Instance.Environment.SubmitFromFieldNeuron.Remove(SubmitFromFieldStaging);
        RunManager.Instance.Environment.WithdrawToHandNeuron.Remove(WithdrawToHandStaging);
        RunManager.Instance.Environment.WithdrawToFieldNeuron.Remove(WithdrawToFieldStaging);
        RunManager.Instance.Environment.RequirementSwapNeuron.Remove(RequirementSwapStaging);
        
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Remove(HighlightQualifiers);
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Remove(UnhighlightQualifiers);
    }

    private void DragBeginRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        RequirementSlot slot = ib.Get<RequirementSlot>();
        RunManager.Instance.Environment.DragBeginRunSkill.Invoke(slot.Skill);
    }

    private void DragEndRunSkill(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.DragEndRunSkill.Invoke();
    }
    
    private void ConfirmSelections()
    {
        RunManager.Instance.Environment.ConfirmDeckSelectionsProcedure();
    }
    
    private void HighlightQualifiers(Predicate<RunSkill> pred)
    {
        Requirements.TraversalActive().Do(Highlight);

        void Highlight(SlotView view)
        {
            RequirementSlot slot = view.Get<RequirementSlot>();
            if (slot == null || slot.Skill == null || !pred(slot.Skill))
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(true);
        }
    }

    private void UnhighlightQualifiers()
    {
        Requirements.TraversalActive().Do(Unhighlight);

        void Unhighlight(SlotView view)
        {
            RequirementSlot slot = view.Get<RequirementSlot>();
            if (slot == null)
                return;
            view.GetContentView().GetBehaviour<HighlightBehaviour>().SetHighlight(false);
        }
    }

    #region MoveSkillRelated

    private void MoveSkill(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        IDeckIndex fromIndex = GetDeckIndex(from);
        IDeckIndex toIndex = GetDeckIndex(to);
        if (fromIndex == null || toIndex == null)
            return;

        RunManager.Instance.Environment.MoveSkillProcedure(fromIndex, toIndex);
    }

    private IDeckIndex GetDeckIndex(InteractBehaviour ib)
    {
        object obj = ib.Get<object>();
        if (obj is RunSkill runSkill)
            return runSkill.ToDeckIndex();
        
        if (obj is SkillSlot skillSlot)
            return skillSlot.ToDeckIndex();
        
        if (obj is RequirementSlot requirementSlot)
            return requirementSlot.ToDeckIndex();

        return null;
    }
    
    public SlotView SkillItemFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.Region == SkillRegion.Requirement)
            return Requirements.ViewFromIndex(deckIndex.Index);

        throw new NotImplementedException();
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

    #endregion
}
