
using UnityEngine.UI;

public class TitlePanel : Panel
{
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

        StartRunButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();

        StartRunButton.onClick.AddListener(StartRun);
        SettingsButton.onClick.AddListener(OpenMenu);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void StartRun()
    {
        // Animator.SetStateAsync(2);
        // CanvasManager.Instance.AppCanvas.OpenRunConfigPanel();

        AppManager.Instance.ProfileManager.RunConfigForm = RunConfigForm.FirstTime();
        AppManager.Push(new RunAppS());
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
