
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : Panel
{
    [SerializeField] private CanvasGroup CanvasGroup;
    
    [SerializeField] private RectTransform TitleIdlePivot;
    [SerializeField] private CanvasGroup TitleCanvasGroup;
    [SerializeField] private RectTransform TitleTransform;
    [SerializeField] private TMP_Text TitleText;

    [SerializeField] private RectTransform DetailedTextIdlePivot;
    [SerializeField] private CanvasGroup DetailedTextCanvasGroup;
    [SerializeField] private RectTransform DetailedTextTransform;
    [SerializeField] private TMP_Text DetailedText;

    [SerializeField] private RectTransform OptionsIdlePivot;
    [SerializeField] private CanvasGroup OptionsCanvasGroup;
    [SerializeField] private RectTransform OptionsTransform;
    [SerializeField] private Button[] Buttons;
    [SerializeField] private TMP_Text[] Texts;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        Buttons[0].onClick.RemoveAllListeners();
        Buttons[1].onClick.RemoveAllListeners();
        Buttons[2].onClick.RemoveAllListeners();
        Buttons[3].onClick.RemoveAllListeners();

        Buttons[0].onClick.AddListener(SelectOption0);
        Buttons[1].onClick.AddListener(SelectOption1);
        Buttons[2].onClick.AddListener(SelectOption2);
        Buttons[3].onClick.AddListener(SelectOption3);
    }

    protected override void InitStateMachine()
    {
        SM = new(2);
        // 0 for hide, 1 for show
        SM[0, 1] = ShowTween;
        SM[1, 1] = SelfTransitionTween;
        SM[-1, 0] = HideTween;
        
        SetState(0);
    }

    public override void Refresh()
    {
        base.Refresh();

        DialogPanelDescriptor d = _address.Get<DialogPanelDescriptor>();

        DetailedText.text = d.GetDetailedText();

        for (int i = 0; i < Buttons.Length; i++)
        {
            // bool active = i < d.GetOptionsCount() && !RunManager.Instance.Environment.Map.Choosing;
            bool active = i < d.GetOptionsCount();
            Buttons[i].gameObject.SetActive(active);
            if(!active)
                continue;

            Buttons[i].interactable = d.GetOption(i).CanSelect();
            Texts[i].text = d.GetOption(i).Text;
        }
    }

    private void SelectedOption(int i)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedOptionSignal(i));
        if (RunManager.Instance.Environment == null)
            return;
        
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
    }

    private void SelectOption0() => SelectedOption(0);
    private void SelectOption1() => SelectedOption(1);
    private void SelectOption2() => SelectedOption(2);
    private void SelectOption3() => SelectedOption(3);
    
    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(TweenAnimation.Show(RectTransform, Vector2.zero, CanvasGroup))
            .Join(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleCanvasGroup))
            .Join(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup).SetDelay(0.05f))
            .Join(TweenAnimation.Show(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.10f))
            ;
    }

    public Tween SelfTransitionTween()
    {
        return DOTween.Sequence()
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup))
            .Join(TweenAnimation.Hide(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup))
            .AppendCallback(Refresh)
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup))
            .Join(TweenAnimation.Show(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.05f));
    }
    
    public override Tween HideTween()
    {
        return DOTween.Sequence()
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleCanvasGroup))
            .Join(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup).SetDelay(0.05f))
            .Join(TweenAnimation.Hide(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.10f))
            .Join(TweenAnimation.Hide(RectTransform, Vector2.zero, CanvasGroup).SetDelay(0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
