
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnnotationManager : XView, Addressable
{
    [SerializeField] private XView Background;
    [SerializeField] private ListView Annotations;
    [SerializeField] private Image ProgressCircle;
    
    private ListModel<AnnotationDetails> _annotationStack;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "AnnotationStack",            thisObject => ((AnnotationManager)thisObject).GetAnnotationStack() },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        _annotationStack = new();
        Annotations.SetAddress("Canvas.AnnotationManager.AnnotationStack");
        Annotations.SetPrefabProvider(PrefabProvider);
    }

    private int PrefabProvider(object d)
    {
        AnnotationDetails annotationDetails = d as AnnotationDetails;
        return (int)annotationDetails.AnnotationViewType;
    }

    private ListModel<AnnotationDetails> GetAnnotationStack()
        => _annotationStack;

    private Tween _handle;

    public void TryShowAnnotation(AnnotationDetails d)
    {
        if (!CanShow(d))
            return;

        if (_annotationStack.Count() > 0 && d.ParentAnnotationDetails != _annotationStack.GetLast())
            return;

        if (d.FirstCounter == 0 && d.SecondCounter == 0)
        {
            _handle?.Kill();
            FinishCounter(d);
        }
        else
        {
            _handle?.Kill();
            _handle = DOTween.Sequence()
                .AppendInterval(d.FirstCounter)
                .AppendCallback(() => ShowCounter(d))
                .Append(DOTween.To(GetFillAmount, SetFillAmount, 0, d.SecondCounter).SetEase(Ease.Linear))
                .AppendCallback(() => FinishCounter(d));
            _handle.SetAutoKill();
            _handle.Restart();
        }
    }

    private bool CanShow(AnnotationDetails d)
        => d.Address.Get<Annotatable>().CanShowAnnotation();

    public void StopShowAnnotation(InteractBehaviour ib, PointerEventData d)
        => StopShowAnnotation();
    public void StopShowAnnotation()
    {
        _handle?.Kill();
        HideCounter();
    }

    private float GetFillAmount()
        => ProgressCircle.fillAmount;

    private void SetFillAmount(float value)
        => ProgressCircle.fillAmount = value;

    private void ShowCounter(AnnotationDetails d)
    {
        ProgressCircle.transform.position = d.AnnotationAlignmentDetails.GetProgressCirclePosition();
        ProgressCircle.fillAmount = 1;
        ProgressCircle.gameObject.SetActive(true);
    }

    private void HideCounter()
    {
        ProgressCircle.gameObject.SetActive(false);
    }

    private void FinishCounter(AnnotationDetails d)
    {
        ProgressCircle.gameObject.SetActive(false);
        EnqueueAnnotation(d);
    }

    public void EnqueueAnnotation(AnnotationDetails d)
    {
        _annotationStack.Add(d);
        Annotations.AddItem();
        Align();
        RegisterCoverForSecondLast();
    }

    public void DequeueAnnotation()
    {
        UnregisterCoverForSecondLast();
        _annotationStack.RemoveLast();
        Annotations.RemoveLast();
    }

    private void DequeueUntilLevel(int dequeueIndex)
    {
        while (dequeueIndex < Annotations.GetCount() - 1)
            DequeueAnnotation();
    }

    private void Align()
    {
        AnnotationDetails d = _annotationStack.GetLast();
        SlotView slotView = Annotations.LastView();

        Vector3 displacement = slotView.GetContentView().GetComponent<AnnotationView>().GetCriticalDisplacement(d.AnnotationAlignmentDetails);
        
        slotView.GetRect().position = d.AnnotationAlignmentDetails.GetCenterPosition() - displacement;
        slotView.GetAnimator().SetState(SlotView.IDLE);
    }

    private void RegisterCoverForSecondLast()
    {
        int count = Annotations.GetCount();
        if (count > 1)
        {
            AnnotationView v = Annotations.ViewFromIndex(Annotations.GetCount() - 2).GetContentView() as AnnotationView;
            v.Cover.raycastTarget = true;
            v.CoverIb.PointerEnterNeuron.Join(PointerEnter);
        }
        else
        {
            Background.gameObject.SetActive(true);
            InteractBehaviour backgroundIb = Background.GetInteractBehaviour();
            backgroundIb.PointerEnterNeuron.Join(PointerEnter);
        }
    }

    private void UnregisterCoverForSecondLast()
    {
        int count = Annotations.GetCount();
        if (count > 1)
        {
            AnnotationView v = Annotations.ViewFromIndex(Annotations.GetCount() - 2).GetContentView() as AnnotationView;
            v.Cover.raycastTarget = false;
            v.CoverIb.PointerEnterNeuron.Remove(PointerEnter);
        }
        else
        {
            Background.gameObject.SetActive(false);
            InteractBehaviour backgroundIb = Background.GetInteractBehaviour();
            backgroundIb.PointerEnterNeuron.Remove(PointerEnter);
        }
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        DequeueUntilLevel(GetDequeueIndex(ib));
    }

    private int GetDequeueIndex(InteractBehaviour ib)
    {
        return Annotations.TraversalActive().FirstIdx(slotView =>
            (slotView.GetContentView() as AnnotationView).CoverIb == ib) ?? -1;
    }
}
