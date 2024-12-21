
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class TitlePanel : Panel
{
    public Button ContinueButton;
    public Button StartRunButton;
    public Button SettingsButton;
    public Button ExitButton;

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for title
        Animator animator = new(2, "Title Panel");
        animator[0, 1] = EnterIdle;
        animator[-1, 0] = HideTweenWithCurtain;
        return animator;
    }

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        ContinueButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        ContinueButton.onClick.AddListener(Continue);
        StartRunButton.onClick.AddListener(StartRun);
        SettingsButton.onClick.AddListener(OpenMenu);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void FirstTime()
    {
        AppManager.Instance.ProfileManager.RunConfigForm = RunConfigForm.FirstTime();
        AppManager.Instance.Push(AppStateMachine.RUN);
    }

    private void Continue()
    {
    }

    private void StartRun()
    {
        OpenRunConfigPanel();
    }

    private async UniTask OpenRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.RunConfigPanel.GetAnimator().SetStateAsync(1);
    }

    private void OpenMenu()
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }

    private void ExitGame()
    {
        AppManager.ExitGame();
    }
}
