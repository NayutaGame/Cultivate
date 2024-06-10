
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : CurtainPanel
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
        TitleToRunConfig();
        // CanvasManager.Instance.AppCanvas.OpenRunConfigPanel();
    }

    private void TitleToRunConfig()
    {
        DOTween.Sequence()
            .Append(_layer1RT.DOScale(2, 0.3f).From(1))
            .Append(_layer1CanvasGroup.DOFade(0, 0.3f).From(1))
            .Append(_layer2RT.DOScale(1, 0.3f).From(0.8f))
            .Append(_layer2CanvasGroup.DOFade(1, 0.3f).From(0))
            .Append(_layer3RT.DOScale(1.2f, 0.3f).From(1))
            .Append(_layer3curtain.DOFade(0.5f, 0.3f).From(1))
            .SetAutoKill().Restart();
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
}
