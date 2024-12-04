
using System;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscoverSkillPanel : Panel
{
    [SerializeField] private RectTransform TitleTransform;
    [SerializeField] private RectTransform TitleIdlePivot;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private RectTransform DetailedTextTransform;
    [SerializeField] private RectTransform DetailedTextIdlePivot;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private LegacyListView SkillList;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        SkillList.SetAddress(_address.Append(".Skills"));
        SkillList.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillList.LeftClickNeuron.Join(PickDiscoveredSkill);
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Discover Panel");
        animator[0, 1] = ShowTween;
        animator[-1, 0] = HideTween;
        animator[1, 1] = SelfTransitionTween;
        
        animator.SetState(0);
        return animator;
    }

    public override void Refresh()
    {
        base.Refresh();

        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        TitleText.text = d.GetTitleText();
        DescriptionText.text = d.GetDescriptionText();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Add(PickDiscoveredSkillStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Remove(PickDiscoveredSkillStaging);
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void PickDiscoveredSkill(LegacyInteractBehaviour ib, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();
        SkillEntryDescriptor skill = ib.GetSimpleView().Get<SkillEntryDescriptor>();

        Signal signal = new PickDiscoveredSkillSignal(d.GetIndexOfSkill(skill));
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
        CanvasManager.Instance.SkillAnnotation.PointerExit();
    }

    private void PickDiscoveredSkillStaging(PickDiscoveredSkillDetails d)
    {
        int pickedIndex = d.PickedIndex;
        LegacyInteractBehaviour discoverIB = SkillList.ActivePool[pickedIndex].GetInteractBehaviour();
        LegacyInteractBehaviour cardIB = CanvasManager.Instance.RunCanvas.SkillInteractBehaviourFromDeckIndex(d.DeckIndex);

        CanvasManager.Instance.RunCanvas.PickDiscoveredSkillStaging(cardIB, discoverIB);
        
        LegacyPivotBehaviour pivotBehaviour = discoverIB.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
        if (pivotBehaviour != null)
            pivotBehaviour.Disappear();
    }

    public override Tween ShowTween()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.Animator.SetStateTween(0))
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(TraversalSetDisappear)
            .Append(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DescriptionText))
            .AppendCallback(TraversalPlayAppearAnimation);

    public override Tween HideTween()
        => DOTween.Sequence()
            // make skills non interactable
            // dissolve of skills
            .AppendCallback(TraversalPlayDisappearAnimation)
            .AppendInterval(0.15f)
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DescriptionText))
            .AppendCallback(() => gameObject.SetActive(false));

    public Tween SelfTransitionTween()
        => DOTween.Sequence()
            .AppendCallback(TraversalPlayDisappearAnimation)
            .AppendInterval(0.15f)
            .AppendCallback(SkillList.Refresh)
            .AppendCallback(Refresh)
            .AppendCallback(TraversalPlayAppearAnimation);

    public void TraversalSetDisappear()
    {
        SkillList.TraversalActive().Do(item =>
        {
            LegacyPivotBehaviour pivotBehaviour = item.GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
                pivotBehaviour.Animator.SetState(0);
        });
    }

    public void TraversalPlayAppearAnimation()
    {
        SkillList.TraversalActive().Do(item =>
        {
            LegacyPivotBehaviour pivotBehaviour = item.GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
                pivotBehaviour.PlayAppearAnimation();
        });
    }

    public void TraversalPlayDisappearAnimation()
    {
        SkillList.TraversalActive().Do(item =>
        {
            LegacyPivotBehaviour pivotBehaviour = item.GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
                pivotBehaviour.PlayDisappearAnimation();
        });
    }
}
