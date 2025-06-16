
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitlePanel : Panel
{
    public XButton ContinueButton;
    public XButton StartRunButton;
    public XButton StartPrologueButton;
    public XButton EntityEditorButton;
    public XButton SkillBrowserButton;
    public XButton AchievementBrowserButton;
    public XButton SettingsButton;
    public XButton ExitButton;

    public GameObject TitleModel;

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
        
        ContinueButton._button.onClick.RemoveAllListeners();
        StartRunButton._button.onClick.RemoveAllListeners();
        StartPrologueButton._button.onClick.RemoveAllListeners();
        SettingsButton._button.onClick.RemoveAllListeners();
        EntityEditorButton._button.onClick.RemoveAllListeners();
        SkillBrowserButton._button.onClick.RemoveAllListeners();
        AchievementBrowserButton._button.onClick.RemoveAllListeners();
        ExitButton._button.onClick.RemoveAllListeners();
        
        ContinueButton._button.onClick.AddListener(Continue);
        StartRunButton._button.onClick.AddListener(StartRun);
        StartPrologueButton._button.onClick.AddListener(StartPrologue);
        SettingsButton._button.onClick.AddListener(OpenMenu);
        EntityEditorButton._button.onClick.AddListener(OpenEntityEditorPanel);
        SkillBrowserButton._button.onClick.AddListener(OpenSkillBrowserPanel);
        AchievementBrowserButton._button.onClick.AddListener(OpenAchievementBrowserPanel);
        ExitButton._button.onClick.AddListener(ExitGame);
        
        ContinueButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        StartRunButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        StartPrologueButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        SettingsButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        EntityEditorButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        SkillBrowserButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        AchievementBrowserButton._button.onClick.AddListener(AudioManager.PlayButtonPress);
        ExitButton._button.onClick.AddListener(AudioManager.PlayButtonPress);

        ContinueButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        StartRunButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        StartPrologueButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        EntityEditorButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        SkillBrowserButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        AchievementBrowserButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        SettingsButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        ExitButton._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
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

        bool audienceIsPlayer = AppManager.Instance.AudienceIsPlayer();
        
        EntityEditorButton.gameObject.SetActive(!audienceIsPlayer);
        SkillBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        AchievementBrowserButton.gameObject.SetActive(!audienceIsPlayer);
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

    private void OpenAchievementBrowserPanel()
    {
        CanvasManager.Instance.AppCanvas.AchievementBrowserPanel.Show();
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
            .AppendCallback(() => TitleModel.SetActive(true))
            .AppendCallback(Refresh)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0));

    public override Tween HideTweenWithCurtain()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(IDLE))
            .AppendCallback(() => TitleModel.SetActive(false))
            .AppendCallback(() => gameObject.SetActive(false));
}
