
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RequirementSlotView : SlotView
{
    [SerializeField] private XView AnnotationProvider;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        AnnotationProvider.CheckAwake();
        AnnotationProvider.GetInteractBehaviour().PointerEnterNeuron.Join(InvokeHighlightQualifiers);
        AnnotationProvider.GetInteractBehaviour().PointerExitNeuron.Join(InvokeUnhighlightQualifiers);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        AnnotationProvider.SetAddress(GetAddress().Append(".Descriptor"));
    }

    public override void Refresh()
    {
        base.Refresh();

        // SkillSlot slot = Get<SkillSlot>();
        //
        // bool occupied = slot.IsOccupied();
        // SkillView.gameObject.SetActive(occupied);
        // if (!occupied)
        //     return;
        //
        // SkillView.Refresh();
    }

    private void InvokeHighlightQualifiers(InteractBehaviour ib, PointerEventData d)
    {
        Predicate<RunSkill> pred = ib.Get<RunSkillDescriptor>().Contains;
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Invoke(pred);
    }
    
    private void InvokeUnhighlightQualifiers(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Invoke();
    }
}
