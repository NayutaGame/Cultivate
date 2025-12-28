
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TitlePanel : Panel
{
    public CLButton ContinueButton;
    public CLButton StartRunButton;
    public CLButton StartPrologueButton;
    public CLButton SettingsButton;
    public CLButton ExitButton;
    
    public CLButton EntityEditorButton;
    public CLButton SkillBrowserButton;
    public CLButton BuffBrowserButton;
    public CLButton AchievementBrowserButton;
    public CLButton UnlockNextDifficultyButton;
    public CLButton DeleteProfileButton;
    public CLButton UnlockEverythingButton;

    public CLButton QQButton;
    public CLButton SteamButton;
    public CLButton FeedbackButton;
    public CLButton DiscordButton;

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

        List<CLButton> ButtonList = new List<CLButton>
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
            QQButton,
            SteamButton,
            FeedbackButton,
            DiscordButton,
        };

        List<Action<InteractBehaviour, PointerEventData>> ActionList = new List<Action<InteractBehaviour, PointerEventData>>
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
            OpenQQ,
            OpenSteam,
            OpenFeedback,
            OpenDiscord,
        };

        for (int i = 0; i < ButtonList.Count; i++)
            ButtonList[i].LeftClickNeuron.Join(ActionList[i]);
    }

    public override void Refresh()
    {
        Profile currProfile = AppManager.Instance.ProfileManager.GetCurrProfile();
        bool firstRun = !currProfile.IsFirstRunFinished();
        bool hasValidSave = currProfile.HasValidSave();
        
        ContinueButton.gameObject.SetActive(hasValidSave);
        StartRunButton.gameObject.SetActive(!firstRun);
        StartPrologueButton.gameObject.SetActive(true);

        bool audienceIsPlayer = AppManager.Instance.AudienceIsPlayer();
        
        EntityEditorButton.gameObject.SetActive(!audienceIsPlayer);
        SkillBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        BuffBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        AchievementBrowserButton.gameObject.SetActive(!audienceIsPlayer);
        UnlockNextDifficultyButton.gameObject.SetActive(!audienceIsPlayer);
        DeleteProfileButton.gameObject.SetActive(!audienceIsPlayer);
        UnlockEverythingButton.gameObject.SetActive(!audienceIsPlayer);
    }

    private void TryInformPlayer()
    {
        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        if (!profile.HasCorruptedSave())
            return;
        
        CanvasManager.Instance.ShowDialog(
            message: "由于游戏更新，导致上次游戏的存档过时。",
            onConfirm: profile.RepairCorruptedEnvironment
        );
    }

    private void OnEnable()
    {
        AppManager.Instance.ClearEscStack();
        AppManager.Instance.PushEscFunc(ExitGame);
        CameraManager.Instance.SetParallaxEnabled(true);
        Refresh();
    }

    private void OnDisable()
    {
        CameraManager.Instance.SetParallaxEnabled(false);
        AppManager.Instance.PopEscFunc();
    }

    private void FirstRun()
    {
        AppManager.Instance.Push(AppStateMachine.RUN, RunConfig.FirstRun());
    }

    private void Continue(InteractBehaviour ib, PointerEventData d)
    {
        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        RunEnvironment environment = profile.Environment;
        Assert.IsTrue(environment != null);
        AppManager.Instance.Push(AppStateMachine.RUN, environment);
    }

    private void StartRun(InteractBehaviour ib, PointerEventData d)
    {
        OpenRunConfigPanel();
    }

    private void StartPrologue(InteractBehaviour ib, PointerEventData d)
    {
        FirstRun();
    }

    private async UniTask OpenRunConfigPanel()
    {
        await GetAnimator().SetStateAsync(0);
        await CanvasManager.Instance.AppCanvas.RunConfigPanel.GetAnimator().SetStateAsync(1);
    }

    private void OpenMenu(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Push(AppStateMachine.MENU);
    }

    private void ExitGame()
    {
        AppManager.ExitGame();
    }

    private void ExitGame(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.ExitGame();
    }

    private void OpenEntityEditorPanel(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.EntityEditorPanel.Show();
    }

    private void OpenSkillBrowserPanel(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.SkillBrowserPanel.Show();
    }

    private void OpenBuffBrowserPanel(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.BuffBrowserPanel.Show();
    }

    private void OpenAchievementBrowserPanel(InteractBehaviour ib, PointerEventData d)
    {
        CanvasManager.Instance.AppCanvas.AchievementBrowserPanel.Show();
    }

    private void TryUnlockNextDifficulty(InteractBehaviour ib, PointerEventData d)
    {
        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        profile.SetFirstRunFinished(true);
        profile.TryUnlockNextDifficulty();
        Refresh();
    }

    private void DeleteProfile(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.ProfileManager.DeleteProfile();
        Refresh();
    }

    private void UnlockEverything(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.ProfileManager.GetCurrProfile().UnlockEverything();
        Refresh();
    }

    private void OpenQQ(InteractBehaviour ib, PointerEventData d)
    {
        string url = "https://qm.qq.com/cgi-bin/qm/qr?k=E0AbYkxHbJo5LAmTkhiTAweJr94I0pp1&jump_from=webapi&authKey=KXCcV8e7xxCfspWV0u2PbPP8IuJcovoyi7EFIGvDgoBD3JTjC9DPXQi2IGDnp9Du";
        Application.OpenURL(url);
    }

    private void OpenSteam(InteractBehaviour ib, PointerEventData d)
    {
        string url = "https://store.steampowered.com/app/2125490/_/";
        Application.OpenURL(url);
    }

    private void OpenFeedback(InteractBehaviour ib, PointerEventData d)
    {
        string url = "https://docs.qq.com/form/page/DTUtwSG9Vd2tpaWdT";
        Application.OpenURL(url);
    }

    private void OpenDiscord(InteractBehaviour ib, PointerEventData d)
    {
        string url = "https://discord.gg/RPtJgjhX";
        Application.OpenURL(url);
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(() => TitleModel.SetActive(true))
            .AppendCallback(Refresh)
            .AppendCallback(TryInformPlayer)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0));

    public override Tween HideTweenWithCurtain()
        => DOTween.Sequence()
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(IDLE))
            .AppendCallback(() => TitleModel.SetActive(false))
            .AppendCallback(() => gameObject.SetActive(false));
}
