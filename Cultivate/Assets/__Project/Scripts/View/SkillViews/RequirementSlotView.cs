
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequirementSlotView : ScaleSlotView
{
    [SerializeField] private XView AnnotationProvider;
    [SerializeField] private Image InvalidSign1;
    [SerializeField] private Image InvalidSign2;
    
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        AnnotationProvider.CheckAwake();
        // AnnotationProvider.GetInteractBehaviour().PointerEnterNeuron.Join(InvokeHighlightQualifiers);
        // AnnotationProvider.GetInteractBehaviour().PointerExitNeuron.Join(InvokeUnhighlightQualifiers);
        AnnotationProvider.GetBehaviour<AnnotationBehaviour>().InvokeShowAnnotation.Join(InvokeShowAnnotation);
        AnnotationProvider.GetBehaviour<AnnotationBehaviour>().InvokeHideAnnotation.Join(InvokeHideAnnotation);
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        AnnotationProvider.SetAddress(GetAddress().Append(".Descriptor"));
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.DragBeginRunSkill.Add(MarkInvalidDrop);
        RunManager.Instance.Environment.DragEndRunSkill.Add(UnmarkInvalidDrop);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.DragBeginRunSkill.Remove(MarkInvalidDrop);
        RunManager.Instance.Environment.DragEndRunSkill.Remove(UnmarkInvalidDrop);
    }

    private void InvokeShowAnnotation()
    {
        Predicate<RunSkill> pred = AnnotationProvider.Get<RunSkillQuery>().Matches;
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Invoke(pred);
    }

    private void InvokeHideAnnotation()
    {
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Invoke();
    }

    private Tween _handle1;
    private Tween _handle2;

    public void MarkInvalidDrop(RunSkill skill)
    {
        RequirementSlot slot = Get<RequirementSlot>();
        if (slot.GetQuery().Matches(skill))
            return;
        
        _handle1?.Kill();
        _handle1 = InvalidSign1.DOFade(1, 0.15f);
        _handle1.SetAutoKill().Restart();
        
        _handle2?.Kill();
        _handle2 = InvalidSign2.DOFade(1, 0.15f);
        _handle2.SetAutoKill().Restart();
    }

    public void UnmarkInvalidDrop()
    {
        _handle1?.Kill();
        _handle1 = InvalidSign1.DOFade(0, 0.15f);
        _handle1.SetAutoKill().Restart();
        
        _handle2?.Kill();
        _handle2 = InvalidSign2.DOFade(0, 0.15f);
        _handle2.SetAutoKill().Restart();
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
        Predicate<RunSkill> pred = ib.Get<RunSkillQuery>().Matches;
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Invoke(pred);
    }
    
    private void InvokeUnhighlightQualifiers(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Invoke();
    }
}
