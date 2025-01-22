
using CLLibrary;
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

    public override void AwakeFunction()
    {
        base.AwakeFunction();

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

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Dialog Panel");
        animator[0, 1] = EnterIdle;
        animator[1, 1] = SelfTransitionTween;
        animator[-1, 0] = EnterHide;
        
        animator.SetState(0);
        return animator;
    }

    public override void Refresh()
    {
        DialogPanelDescriptor d = _address.Get<DialogPanelDescriptor>();

        TitleText.text = d.GetTitleText();

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

    private Neuron<SelectOptionDetails> SelectOptionEvent = new();
    
    private void OnEnable()
    {
        SelectOptionEvent.Add(RunManager.Instance.Environment.SelectOptionProcedure);
        RunManager.Instance.Environment.SelectOptionNeuron.Add(SelectOptionStaging);
    }

    private void OnDisable()
    {
        SelectOptionEvent.Remove(RunManager.Instance.Environment.SelectOptionProcedure);
        RunManager.Instance.Environment.SelectOptionNeuron.Remove(SelectOptionStaging);
    }

    private void SelectOption(int selectedIndex)
    {
        SelectOptionDetails details = new(selectedIndex);
        SelectOptionEvent.Invoke(details);
    }

    private void SelectOption0() => SelectOption(0);
    private void SelectOption1() => SelectOption(1);
    private void SelectOption2() => SelectOption(2);
    private void SelectOption3() => SelectOption(3);
    
    public void SelectOptionStaging(SelectOptionDetails d)
    {
        // AudioManager.Play("CardPlacement");

        Buttons.Do(b => b.interactable = false);

        // CanvasManager.Instance.RunCanvas.GetAnimationQueue().QueueAnimation(GetAnimator().TweenFromSetState(2));
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0))
            .Append(TweenAnimation.Show(GetRect(), Vector2.zero, CanvasGroup))
            .Join(TweenAnimation.Show(TitleTransform, TitleIdlePivot.anchoredPosition, TitleCanvasGroup))
            .Join(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup).SetDelay(0.05f))
            .Join(TweenAnimation.Show(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.10f))
            .AppendCallback(() => Buttons.Do(b => b.interactable = true));

    public Tween SelfTransitionTween()
        => DOTween.Sequence()
            .Append(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup))
            .Join(TweenAnimation.Hide(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup))
            .AppendCallback(Refresh)
            .Append(TweenAnimation.Show(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup))
            .Join(TweenAnimation.Show(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.05f));

    public override Tween EnterHide()
        => DOTween.Sequence()
            .Append(TweenAnimation.Hide(TitleTransform, TitleIdlePivot.anchoredPosition, TitleCanvasGroup))
            .Join(TweenAnimation.Hide(DetailedTextTransform, DetailedTextIdlePivot.anchoredPosition, DetailedTextCanvasGroup).SetDelay(0.05f))
            .Join(TweenAnimation.Hide(OptionsTransform, OptionsIdlePivot.anchoredPosition, OptionsCanvasGroup).SetDelay(0.10f))
            .Join(TweenAnimation.Hide(GetRect(), Vector2.zero, CanvasGroup).SetDelay(0.15f))
            .AppendCallback(() => gameObject.SetActive(false));
}
