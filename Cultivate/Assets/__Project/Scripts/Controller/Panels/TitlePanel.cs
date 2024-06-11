
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : Panel
{
    public Button StartRunButton;
    public Button SettingsButton;
    public Button ExitButton;

    [SerializeField] private RectTransform _layer1RT;
    [SerializeField] private CanvasGroup _layer1CanvasGroup;
    [SerializeField] private RectTransform _layer2RT;
    [SerializeField] private CanvasGroup _layer2CanvasGroup;
    [SerializeField] private RectTransform _layer3RT;
    [SerializeField] private Image _layer3curtain;

    protected override void InitStateMachine()
    {
        SM = new(3);
        // 0 for hide, 1 for title, 2 for run config
        SM[0, 1] = ShowTween;
        SM[1, 2] = TitleToRunConfigTween;
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
    }

    private void StartRun()
    {
        SetStateAsync(2);
        // CanvasManager.Instance.AppCanvas.OpenRunConfigPanel();
    }

    private Tween TitleToRunConfigTween()
    {
        return DOTween.Sequence()
            .Join(_layer1RT.DOScale(2, 0.3f).From(1))
            .Join(_layer1CanvasGroup.DOFade(0, 0.3f).From(1))
            .Join(_layer2RT.DOScale(1, 0.3f).From(0.8f))
            .Join(_layer2CanvasGroup.DOFade(1, 0.3f).From(0))
            .Join(_layer3RT.DOScale(1.2f, 0.3f).From(1))
            .Join(_layer3curtain.DOFade(0.5f, 0.3f).From(1));
    }

    private void RunConfigToTitle()
    {
        /*
         * 
         */
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
            .Append(CanvasManager.Instance.Curtain.SetStateTween(0));
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence().SetAutoKill()
            .Append(CanvasManager.Instance.Curtain.SetStateTween(1))
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
