
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
    [SerializeField] private ListView SkillViews;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");
        SkillViews.SetAddress(_address.Append(".Skills"));
        SkillViews.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillViews.LeftClickNeuron.Join(PickSkill);
    }

    protected override void InitStateMachine()
    {
        SM = new(2);
        // 0 for hide, 1 for show
        SM[0, 1] = ShowTween;
        SM[-1, 0] = HideTween;
        
        SetState(0);
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
        // talks to model
        // staging
        
        DiscoverSkillPanelDescriptor d = _address.Get<DiscoverSkillPanelDescriptor>();

        SkillDescriptor skill = ib.GetSimpleView().Get<SkillDescriptor>();
        // TODO: Discover Animation
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedOptionSignal(d.GetIndexOfSkill(skill)));
        PanelS panelS = PanelS.FromPanelDescriptorNullMeansMap(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        
        CanvasManager.Instance.SkillAnnotation.PointerExit(ib, eventData);
    }

    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            // make skills not interactable
            .Append(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedText))
            // dissolve of skills
            // make skills interactable
            ;
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence()
            // make skills not interactable
            // dissolve of skills
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleText))
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedText))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
