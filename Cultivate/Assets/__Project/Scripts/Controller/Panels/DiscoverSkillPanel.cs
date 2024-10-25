
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
    [SerializeField] private TMP_Text DetailedText;
    [SerializeField] private ListView SkillList;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        SkillList.SetAddress(_address.Append(".Skills"));
        SkillList.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillList.LeftClickNeuron.Join(PickSkill);
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2);
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
        DetailedText.text = d.GetDetailedText();
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData eventData)
        => AudioManager.Play("CardHover");

    private void PickSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();
        SkillEntryDescriptor skill = ib.GetSimpleView().Get<SkillEntryDescriptor>();
        // staging
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        CanvasManager.Instance.SkillAnnotation.PointerExit(ib, eventData);
    }

    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedText))
            // .AppendCallback(TraversalPlayAppearAnimation)
            ;
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence()
            // make skills non interactable
            // dissolve of skills
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedText))
            .AppendCallback(() => gameObject.SetActive(false));
    }

    public Tween SelfTransitionTween()
    {
        return DOTween.Sequence()
            .AppendCallback(SkillList.Refresh);
    }

    public void TraversalPlayAppearAnimation()
    {
        SkillList.TraversalActive().Do(item =>
        {
            ExtraBehaviourPivot extraBehaviourPivot = item.GetExtraBehaviour<ExtraBehaviourPivot>();
            extraBehaviourPivot.PlayAppearAnimation();
        });
    }
}
