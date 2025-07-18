
using System;
using System.Collections.Generic;
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
        _handle?.Kill();
        _handle = DOTween.Sequence()
            .AppendInterval(d.FirstCounter)
            .AppendCallback(() => ShowCounter(d))
            .Append(DOTween.To(GetFillAmount, SetFillAmount, 0, d.SecondCounter).SetEase(Ease.Linear))
            .AppendCallback(() => FinishCounter(d));
        _handle.SetAutoKill();
        _handle.Restart();
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
        Vector3[] fourCornersArray = new Vector3[4];
        GetTargetRect(d).GetWorldCorners(fourCornersArray);
        ProgressCircle.transform.position = fourCornersArray[2];
        ProgressCircle.fillAmount = 1;
        ProgressCircle.gameObject.SetActive(true);
    }

    private RectTransform GetTargetRect(AnnotationDetails d)
    {
        if (d.View is SlotView slotView)
            return slotView.GetContentView().GetRect();
        return d.View.GetRect();
    }

    private void HideCounter()
    {
        ProgressCircle.gameObject.SetActive(false);
    }

    private void FinishCounter(AnnotationDetails d)
    {
        ProgressCircle.gameObject.SetActive(false);
        EnqueueAnnotation(d);
        EnableBackground();
    }

    public void EnqueueAnnotation(AnnotationDetails d)
    {
        _annotationStack.Add(d);
        Annotations.AddItem();
        Register();
    }

    public void DequeueAnnotation()
    {
        Unregister();
        _annotationStack.RemoveLast();
        Annotations.RemoveLast();
    }

    private void DequeueUntilLevel(int dequeueIndex)
    {
        while (dequeueIndex < Annotations.GetCount() - 1)
            DequeueAnnotation();
        
        bool nothingLeft = dequeueIndex == -1;
        if (nothingLeft)
            DisableBackground();
    }

    private void EnableBackground()
    {
        Background.gameObject.SetActive(true);
        InteractBehaviour backgroundIb = Background.GetInteractBehaviour();
        backgroundIb.PointerEnterNeuron.Join(PointerEnter);
    }

    private void DisableBackground()
    {
        Background.gameObject.SetActive(false);
        InteractBehaviour backgroundIb = Background.GetInteractBehaviour();
        backgroundIb.PointerEnterNeuron.Remove(PointerEnter);
    }

    private void Register()
    {
        AnnotationDetails d = _annotationStack.GetLast();
        SlotView slotView = Annotations.LastView();
        XView contentView = slotView.GetContentView();
        RectTransform targetRect = GetTargetRect(d);
        RectTransform annotationRect = slotView.GetRect();

        Vector3 displacement = contentView.GetComponent<AnnotationView>().GetCriticalDisplacement();
        annotationRect.position = targetRect.position - displacement;
        slotView.GetAnimator().SetState(SlotView.IDLE);
        
        InteractBehaviour slotIb = slotView.GetInteractBehaviour();
        slotIb.PointerEnterNeuron.Join(PointerEnter);
    }

    private void Unregister()
    {
        AnnotationDetails d = _annotationStack.GetLast();
        SlotView slotView = Annotations.LastView();

        InteractBehaviour slotIb = slotView.GetInteractBehaviour();
        slotIb.PointerEnterNeuron.Remove(PointerEnter);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        DequeueUntilLevel(GetDequeueIndex(ib));
    }

    private int GetDequeueIndex(InteractBehaviour ib)
    {
        for (int i = 0; i < Annotations.GetCount(); i++)
        {
            SlotView slotView = Annotations.ViewFromIndex(i);
            if (slotView.GetInteractBehaviour() == ib)
                return i;
        }

        return -1;
    }
}
