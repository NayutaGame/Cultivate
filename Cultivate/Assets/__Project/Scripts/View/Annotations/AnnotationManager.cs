
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnnotationManager : XView, Addressable
{
    private static CategoryDetails[] _categoryDetailsMappings;
    
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
        if (!AnnotationDetailsIsValid(d))
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

    private bool AnnotationDetailsIsValid(AnnotationDetails d)
    {
        bool isCycleAnnotation = d.AnnotationViewType == AnnotationViewType.CycleAnnotation;
        
        Annotatable annotatable = d.Address?.Get<Annotatable>();
        if (annotatable == null && !isCycleAnnotation)
            return false;

        bool canShow = annotatable?.CanShowAnnotation() ?? false;
        if (!canShow && !isCycleAnnotation)
            return false;
        
        if (!IsOrderValid(d))
            return false;

        return true;
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

        if (!AnnotationDetailsIsValid(d))
            return;

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
        
        d.AnnotationBehaviour?.InvokeShowAnnotation.Invoke();

        if (Annotations.GetCount() > 0)
            AnnotationOpened.Invoke();
    }

    public void DequeueAnnotation()
    {
        AnnotationDetails d = _annotationStack.GetLast();
        
        UnregisterCoverForSecondLast();
        _annotationStack.RemoveLast();
        Annotations.RemoveLast();
        
        d.AnnotationBehaviour?.InvokeHideAnnotation.Invoke();

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
        targetPosition = CameraManager.Instance.ClampToScreenBounds(slotView.GetContentView().GetRect(), targetPosition);
        
        slotView.GetRect().position = targetPosition;
        slotView.GetAnimator().SetState(SlotView.IDLE);

        annotationView.DidAlign();
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

    public static CategoryDetails[] CategoryDetailsMappings
    {
        get
        {
            if (_categoryDetailsMappings != null)
                return _categoryDetailsMappings;
        
            _categoryDetailsMappings = new CategoryDetails[]
            {
                new(Encyclopedia.KeywordCategory, AnnotationViewType.TextAnnotation, "KeywordCategory", "keyword"),
                new(Encyclopedia.TagCategory, AnnotationViewType.TagAnnotation, "TagCategory", "tag"),
                new(Encyclopedia.JingJieCategory, AnnotationViewType.JingJieAnnotation, "JingJieCategory", "jingJie"),
                new(Encyclopedia.CharacterCategory, AnnotationViewType.CharacterAnnotation, "CharacterCategory", "character"),
                new(Encyclopedia.BuffCategory, AnnotationViewType.BuffAnnotation, "BuffCategory", "buff"),
                new(Encyclopedia.SkillCategory, AnnotationViewType.SkillAnnotation, "SkillCategory", "skill"),
                new(Encyclopedia.PackCategory, AnnotationViewType.PackAnnotation, "PackCategory", "pack"),
            };

            return _categoryDetailsMappings;
        }
    }
    
    public readonly struct CategoryDetails
    {
        public readonly ICategory<Entry> Category;
        public readonly AnnotationViewType AnnotationViewType;
        public readonly string CategoryName;
        public readonly string CategoryShortName;

        public CategoryDetails(
            ICategory<Entry> category,
            AnnotationViewType annotationViewType,
            string categoryName,
            string categoryShortName)
        {
            Category = category;
            AnnotationViewType = annotationViewType;
            CategoryName = categoryName;
            CategoryShortName = categoryShortName;
        }

        public bool TryInterpret(string linkId, TMP_CharacterInfo criticalCharInfo, Rect alignRect,
            out AnnotationDetails annotationDetails)
        {
            if (!Category.ContainsName(linkId))
            {
                annotationDetails = null;
                return false;
            }

            Entry entry = Category.FromName(linkId);
        
            int characterIndex = entry.GetName().IndexOf(criticalCharInfo.character);
            annotationDetails = new AnnotationDetails(
                AnnotationViewType,
                null,
                new Address($"Encyclopedia.{CategoryName}.Dict.{entry.GetId()}"),
                null,
                0.2f,
                0,
                new CharacterAnnotationAlignmentDetails(characterIndex, alignRect));
            return true;
        }
    }
}
