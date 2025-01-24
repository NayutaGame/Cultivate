
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class TitlePanel : Panel
{
    public Button ContinueButton;
    public Button StartRunButton;
    public Button StartPrologueButton;
    public Button EntityEditorButton;
    public Button SkillBrowserButton;
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
        StartRunButton.onClick.RemoveAllListeners();
        StartRunButton.onClick.AddListener(StartRun);
        StartPrologueButton.onClick.RemoveAllListeners();
        StartPrologueButton.onClick.AddListener(StartPrologue);
        SettingsButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.AddListener(OpenMenu);
        EntityEditorButton.onClick.RemoveAllListeners();
        EntityEditorButton.onClick.AddListener(OpenEntityEditorPanel);
        SkillBrowserButton.onClick.RemoveAllListeners();
        SkillBrowserButton.onClick.AddListener(OpenSkillBrowserPanel);
        ExitButton.onClick.RemoveAllListeners();
        ExitButton.onClick.AddListener(ExitGame);
    }

    public override void Refresh()
    {
        Profile currProfile = AppManager.Instance.ProfileManager.GetCurrProfile();
        bool firstRun = !currProfile.IsFirstRunFinished();
        bool hasSave = currProfile.HasSave();

        if (firstRun)
        {
            ContinueButton.gameObject.SetActive(false);
            StartRunButton.gameObject.SetActive(false);
            StartPrologueButton.gameObject.SetActive(true);
        }
        else if (hasSave)
        {
            ContinueButton.gameObject.SetActive(true);
            StartRunButton.gameObject.SetActive(true);
            StartPrologueButton.gameObject.SetActive(true);
        }
        else
        {
            ContinueButton.gameObject.SetActive(false);
            StartRunButton.gameObject.SetActive(true);
            StartPrologueButton.gameObject.SetActive(true);
        }
    }

    private void FirstRun()
    {
        AppManager.Instance.Push(AppStateMachine.RUN, RunConfig.FirstRun());
    }

    private void Continue()
    {
        AppManager.Instance.Push(AppStateMachine.RUN, AppManager.Instance.ProfileManager.GetCurrProfile().ReadRunEnvironment());
    }

    private void StartRun()
    {
        OpenRunConfigPanel();
    }

    private void StartPrologue()
    {
        FirstRun();
    }

    private async UniTask OpenRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.RunConfigPanel.GetAnimator().SetStateAsync(1);
    }

    private void OpenEntityEditorPanel()
    {
        CanvasManager.Instance.AppCanvas.EntityEditorPanel.Show();
    }

    private void OpenSkillBrowserPanel()
    {
        CanvasManager.Instance.AppCanvas.SkillBrowserPanel.Show();
    }

    private void OpenMenu()
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }

    private void ExitGame()
    {
        AppManager.ExitGame();
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(Refresh)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0));
}
