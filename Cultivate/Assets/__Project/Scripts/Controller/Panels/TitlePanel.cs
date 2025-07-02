
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TitlePanel : Panel
{
    public XButton ContinueButton;
    public XButton StartRunButton;
    public XButton StartPrologueButton;
    public XButton SettingsButton;
    public XButton ExitButton;
    
    public XButton EntityEditorButton;
    public XButton SkillBrowserButton;
    public XButton BuffBrowserButton;
    public XButton AchievementBrowserButton;
    public XButton UnlockNextDifficultyButton;
    public XButton DeleteProfileButton;
    public XButton UnlockEverythingButton;

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

        List<XButton> ButtonList = new List<XButton>
        {
            ContinueButton,
            StartRunButton,
            StartPrologueButton,
            SettingsButton,
            ExitButton,
            EntityEditorButton,
            SkillBrowserButton,
            BuffBrowserButton,
            AchievementBrowserButton,
            UnlockNextDifficultyButton,
            DeleteProfileButton,
            UnlockEverythingButton,
        };

        List<UnityAction> ActionList = new List<UnityAction>
        {
            Continue,
            StartRun,
            StartPrologue,
            OpenMenu,
            ExitGame,
            OpenEntityEditorPanel,
            OpenSkillBrowserPanel,
            OpenBuffBrowserPanel,
            OpenAchievementBrowserPanel,
            TryUnlockNextDifficulty,
            DeleteProfile,
            UnlockEverything,
        };

        for (int i = 0; i < ButtonList.Count; i++)
        {
            ButtonList[i]._button.onClick.RemoveAllListeners();
            ButtonList[i]._button.onClick.AddListener(ActionList[i]);
            ButtonList[i]._button.onClick.AddListener(AudioManager.PlayButtonPress);
            ButtonList[i]._propagatePointerEnter._onPointerEnter = AudioManager.PlayButtonHover;
        }
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
        BuffBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        AchievementBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        UnlockNextDifficultyButton.gameObject.SetActive(!audienceIsPlayer);
        DeleteProfileButton.gameObject.SetActive(!audienceIsPlayer);
        UnlockEverythingButton.gameObject.SetActive(!audienceIsPlayer);
    }

    private void OnEnable()
    {
        AppManager.Instance.ClearEscStack();
        AppManager.Instance.PushEscFunc(ExitGame);
    }

    private void OnDisable()
    {
        AppManager.Instance.PopEscFunc();
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

    private void OpenMenu()
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }

    private void ExitGame()
    {
        AppManager.ExitGame();
    }

    private void OpenEntityEditorPanel()
    {
        CanvasManager.Instance.AppCanvas.EntityEditorPanel.Show();
    }

    private void OpenSkillBrowserPanel()
    {
        CanvasManager.Instance.AppCanvas.SkillBrowserPanel.Show();
    }

    private void OpenBuffBrowserPanel()
    {
        CanvasManager.Instance.AppCanvas.BuffBrowserPanel.Show();
    }

    private void OpenAchievementBrowserPanel()
    {
        CanvasManager.Instance.AppCanvas.AchievementBrowserPanel.Show();
    }

    private void TryUnlockNextDifficulty()
    {
        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        profile.SetFirstRunFinished(true);
        profile.TryUnlockNextDifficulty();
        Refresh();
    }

    private void DeleteProfile()
    {
        AppManager.Instance.ProfileManager.NewProfileProcedure();
        Refresh();
    }

    private void UnlockEverything()
    {
        AppManager.Instance.ProfileManager.UnlockEverythingProcedure();
        Refresh();
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
