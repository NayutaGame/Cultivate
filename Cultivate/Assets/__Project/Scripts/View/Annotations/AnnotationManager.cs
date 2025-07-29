
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
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

        if (!IsOrderValid(d))
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

    private bool IsOrderValid(AnnotationDetails d)
    {
        bool noAnnotationOpened = Annotations.GetCount() == 0;
        if (noAnnotationOpened)
            return true;

        RectTransform lastAnnotationViewRect = Annotations.LastView().GetContentView().GetRect();
        
        bool isChildOfLastView = d.InvokerRectTransform.IsChildOf(lastAnnotationViewRect);
        if (isChildOfLastView)
            return true;

        return false;
    }

    private bool CanShow(AnnotationDetails d)
    {
        if (d.AnnotationViewType == AnnotationViewType.CycleAnnotation)
            return true;
        return d.Address.Get<Annotatable>().CanShowAnnotation();
    }

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

    public static Neuron AnnotationOpened = new();
    public static Neuron AnnotationClosed = new();

    public void EnqueueAnnotation(AnnotationDetails d)
    {
        _annotationStack.Add(d);
        Annotations.AddItem();
        Canvas.ForceUpdateCanvases();
        Align();
        RegisterCoverForSecondLast();

        if (Annotations.GetCount() > 0)
            AnnotationOpened.Invoke();
    }

    public void DequeueAnnotation()
    {
        UnregisterCoverForSecondLast();
        _annotationStack.RemoveLast();
        Annotations.RemoveLast();

        if (Annotations.GetCount() == 0)
            AnnotationClosed.Invoke();
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
        AnnotationView annotationView = slotView.GetContentView().GetComponent<AnnotationView>();

        Vector3 displacement = annotationView.GetCriticalDisplacement(d.AnnotationAlignmentDetails);
        
        Vector3 targetPosition = d.AnnotationAlignmentDetails.GetCenterPosition() - displacement;
        targetPosition = ClampToScreenBounds(slotView.GetContentView().GetRect(), targetPosition);
        
        slotView.GetRect().position = targetPosition;
        slotView.GetAnimator().SetState(SlotView.IDLE);

        annotationView.DidAlign();
    }

    private Vector3 ClampToScreenBounds(RectTransform rectTransform, Vector3 targetPosition)
    {
        Vector2 targetPosition_UI = CanvasManager.Instance.World2UI(targetPosition);
        Vector2 screenSize_UI = new Vector2(Screen.width, Screen.height);
        
        // 获取RectTransform在屏幕上的实际边界
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        // 计算从当前位置到目标位置的偏移量
        Vector3 currentPosition = rectTransform.position;
        Vector3 offset = targetPosition - currentPosition;
        
        // 将世界坐标的四个角转换为屏幕坐标，并应用偏移量
        Vector2[] screenCorners = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            // 应用偏移量到corner位置
            Vector3 adjustedCorner = corners[i] + offset;
            screenCorners[i] = CanvasManager.Instance.World2UI(adjustedCorner);
        }
        
        // 计算annotation在屏幕上的边界
        float minX = Mathf.Min(screenCorners[0].x, screenCorners[2].x);
        float maxX = Mathf.Max(screenCorners[0].x, screenCorners[2].x);
        float minY = Mathf.Min(screenCorners[0].y, screenCorners[2].y);
        float maxY = Mathf.Max(screenCorners[0].y, screenCorners[2].y);
        
        float annotationWidth = maxX - minX;
        float annotationHeight = maxY - minY;
        
        // 计算调整后的目标位置
        Vector2 adjustedPosition_UI = targetPosition_UI;
        
        // 调整X坐标
        if (minX < 0)
        {
            adjustedPosition_UI.x = targetPosition_UI.x - minX;
        }
        else if (maxX > screenSize_UI.x)
        {
            adjustedPosition_UI.x = targetPosition_UI.x - (maxX - screenSize_UI.x);
        }
        
        // 调整Y坐标
        if (minY < 0)
        {
            adjustedPosition_UI.y = targetPosition_UI.y - minY;
        }
        else if (maxY > screenSize_UI.y)
        {
            adjustedPosition_UI.y = targetPosition_UI.y - (maxY - screenSize_UI.y);
        }
        
        return CanvasManager.Instance.UI2World(adjustedPosition_UI);
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
