
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : Panel
{
    public Button StartRunButton;
    public Button SettingsButton;
    public Button ExitButton;

    [SerializeField] private RunConfigPanel RunConfigPanel;

    [SerializeField] private RectTransform _layer1RT;
    [SerializeField] private CanvasGroup _layer1CanvasGroup;
    [SerializeField] private RectTransform _layer2RT;
    [SerializeField] private CanvasGroup _layer2CanvasGroup;
    [SerializeField] private RectTransform _layer3RT;
    [SerializeField] private Image _layer3curtain;

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for title, 2 for run config
        Animator animator = new(3, "Title Panel");
        animator[0, 1] = ShowTween;
        animator[1, 2] = TitleToRunConfigTween;
        animator[2, 1] = RunConfigToTitleTween;
        animator[-1, 0] = HideTween;
        return animator;
    }

    public override void Configure()
    {
        base.Configure();

        StartRunButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        StartRunButton.onClick.AddListener(StartRun);
        SettingsButton.onClick.AddListener(OpenMenu);
        ExitButton.onClick.AddListener(ExitGame);
        
        RunConfigPanel.Configure();
    }

    private void StartRun()
    {
        Animator.SetStateAsync(2);
        // CanvasManager.Instance.AppCanvas.OpenRunConfigPanel();
    }

    private Tween TitleToRunConfigTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => _layer1CanvasGroup.interactable = false)
            .AppendCallback(() => _layer2CanvasGroup.gameObject.SetActive(true))
            .Join(_layer1RT.DOScale(2, 0.3f))
            .Join(_layer1CanvasGroup.DOFade(0, 0.3f))
            .Join(_layer2RT.DOScale(1, 0.3f))
            .Join(_layer2CanvasGroup.DOFade(1, 0.3f))
            .Join(_layer3RT.DOScale(1.2f, 0.3f))
            .Join(_layer3curtain.DOFade(0.5f, 0.3f))
            .AppendCallback(() => _layer1CanvasGroup.gameObject.SetActive(false))
            .AppendCallback(() => _layer2CanvasGroup.interactable = true)
            ;
    }

    private Tween RunConfigToTitleTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => _layer2CanvasGroup.interactable = false)
            .AppendCallback(() => _layer1CanvasGroup.gameObject.SetActive(true))
            .Join(_layer1RT.DOScale(1, 0.3f))
            .Join(_layer1CanvasGroup.DOFade(1, 0.3f))
            .Join(_layer2RT.DOScale(0.8f, 0.3f))
            .Join(_layer2CanvasGroup.DOFade(0, 0.3f))
            .Join(_layer3RT.DOScale(1, 0.3f))
            .Join(_layer3curtain.DOFade(0, 0.3f))
            .AppendCallback(() => _layer2CanvasGroup.gameObject.SetActive(false))
            .AppendCallback(() => _layer1CanvasGroup.interactable = true)
            ;
    }

    private void OpenMenu()
    {
        AppManager.Push(new MenuAppS());
    }

    private void ExitGame()
    {
        AppManager.ExitGame();
    }

    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.Animator.SetStateTween(0));
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(CanvasManager.Instance.Curtain.Animator.SetStateTween(1))
            .Append(RunConfigToTitleTween())
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
