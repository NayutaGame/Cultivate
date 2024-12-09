
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
    [SerializeField] public ListView SkillList;

    private Address _address;

    public override void AwakeFunction()
    {
        _address = new Address("Run.Environment.ActivePanel");
        SkillList.SetAddress(_address.Append(".Skills"));
        SkillList.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillList.LeftClickNeuron.Join(PickDiscoveredSkill);
        base.AwakeFunction();
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for idle, 2 for selected
        Animator animator = new(3, "Discover Panel");
        animator[-1, 0] = EnterHide;
        animator[0, 1] = EnterIdle;
        animator[1, 1] = SelfTransitionTween;
        animator[2, 1] = SelfTransitionTween;
        animator[-1, 2] = EnterSelected;
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

    public void LayoutRebuild()
    {
        (SkillList as AnimatedListView).RefreshPivots();
    }

    private Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillEvent = new();
    
    private void OnEnable()
    {
        PickDiscoveredSkillEvent.Add(RunManager.Instance.Environment.PickDiscoveredSkillProcedure);
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Add(PickDiscoveredSkillStaging);
    }

    private void OnDisable()
    {
        PickDiscoveredSkillEvent.Remove(RunManager.Instance.Environment.PickDiscoveredSkillProcedure);
        RunManager.Instance.Environment.PickDiscoveredSkillNeuron.Remove(PickDiscoveredSkillStaging);
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void PickDiscoveredSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        SkillEntryDescriptor skill = ib.Get<SkillEntryDescriptor>();
        int pickedIndex = SkillList.IndexFromView(ib.GetView()).Value;
        PickDiscoveredSkillDetails details = new(skill, pickedIndex);
        PickDiscoveredSkillEvent.Invoke(details);
    }
    
    public void PickDiscoveredSkillStaging(PickDiscoveredSkillDetails d)
    {
        // AudioManager.Play("CardPlacement");

        SkillList.TraversalActive().Do(v => v.GetInteractBehaviour().SetInteractable(false));
        CanvasManager.Instance.SkillAnnotation.PointerExit();
        
        int pickedIndex = d.PickedIndex;
        DelegatingView discoveredView = SkillList.ViewFromIndex(pickedIndex) as DelegatingView;
        RectTransform rect = discoveredView.GetDelegatedView().GetRect();

        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.AddItem();
        DelegatingView view = CanvasManager.Instance.RunCanvas.DeckPanel.LatestSkillItem() as DelegatingView;
        view.SetMoveFromRectToIdle(rect);
        
        discoveredView.GetAnimator().SetState(0);

        CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueAnimation(GetAnimator().TweenFromSetState(2));
    }

    public override Tween EnterHide()
        => DOTween.Sequence()
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DescriptionText))
            .AppendCallback(() => gameObject.SetActive(false));

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(TraversalSetHide)
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0)) // move to pair with show curtain
            .Append(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DescriptionText))
            .Append(TraversalEnterIdle());

    public Tween SelfTransitionTween()
        => DOTween.Sequence()
            .Append(EnterSelected())
            .AppendCallback(SkillList.Refresh)
            .AppendCallback(Refresh)
            .Append(TraversalEnterIdle());

    public Tween EnterSelected()
        // dissolve of skills
        => DOTween.Sequence().Append(TraversalEnterHide());

    public void TraversalSetHide()
    {
        SkillList.TraversalActive().Do(item =>
        {
            item.GetAnimator().SetState(0);
        });
    }

    public Tween TraversalEnterIdle()
    {
        Sequence seq = DOTween.Sequence();
        SkillList.TraversalActive().Do(item =>
        {
            seq.Append(item.GetAnimator().TweenFromSetState(1));
            seq.AppendCallback(() => item.GetInteractBehaviour().SetInteractable(true));
        });
        return seq;
    }

    public Tween TraversalEnterHide()
    {
        Sequence seq = DOTween.Sequence();
        SkillList.TraversalActive().Do(item =>
        {
            seq.AppendCallback(() => item.GetInteractBehaviour().SetInteractable(false));
        });
        SkillList.TraversalActive().Do(item =>
        {
            seq.Join(item.GetAnimator().TweenFromSetState(0));
        });
        return seq;
    }
}
