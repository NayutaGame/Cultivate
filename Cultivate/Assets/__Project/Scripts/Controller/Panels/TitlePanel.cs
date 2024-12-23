
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        ContinueButton.onClick.AddListener(Continue);
        SettingsButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.AddListener(OpenMenu);
        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);
        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(ExitGame);
    }

    public override void Refresh()
    {
        base.Refresh();

        Profile currProfile = AppManager.Instance.ProfileManager.GetCurrProfile();
        ContinueButton.interactable = currProfile.RunEnvironment != null;
    }

    private void FirstTime()
    {
        AppManager.Instance.Push(AppStateMachine.RUN, RunConfigForm.FirstTime());
    }

    private void Continue()
    {
        AppManager.Instance.Push(AppStateMachine.RUN, AppManager.Instance.ProfileManager.GetCurrProfile().RunEnvironment);
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
