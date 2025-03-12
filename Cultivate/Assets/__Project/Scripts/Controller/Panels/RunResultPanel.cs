
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunResultPanel : Panel
{
    [SerializeField] private Button ReturnButton;
    [SerializeField] private TMP_Text OutcomeText;
    [SerializeField] private Image ResultIllustration;
    
    [SerializeField] private TMP_Text CharacterNameText;
    [SerializeField] private TMP_Text DifficultyText;
    [SerializeField] private TMP_Text PlayTimeText;

    [SerializeField] private ListView MilestoneList;
    
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private TMP_Text ExperienceText;
    [SerializeField] private TMP_Text ExperienceGainText;

    [SerializeField] private ListView UnlockList;

    private Tween _handle;
    private ExperienceAnimationDetails _details;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);
    }
    
    public override void Refresh()
    {
        RunResultPanelDescriptor panelDescriptor = RunManager.Instance.Environment.GetActivePanel() as RunResultPanelDescriptor;
        
        if (panelDescriptor.GetRunOutcome() == RunResult.RunOutcome.Victorious)
        {
            OutcomeText.text = "胜利";
            SpriteEntry winIllustration = "RunResultIllustrationWin";
            ResultIllustration.sprite = winIllustration.Sprite;
        }
        else
        {
            OutcomeText.text = "失败";
            SpriteEntry loseIllustration = "RunResultIllustrationLose";
            ResultIllustration.sprite = loseIllustration.Sprite;
        }
        
        CharacterNameText.text = panelDescriptor.GetCharacterName();
        DifficultyText.text = panelDescriptor.GetDifficulty();
        PlayTimeText.text = panelDescriptor.GetPlayTime();

        // AchievementEntryList.SetModel(...);

        _details = new(panelDescriptor.GetInitialExperience(), panelDescriptor.GetInitialLevel(), panelDescriptor.GetExperienceGain());

        // ExperienceSlider.value = _details.InitialExperience;
        ExperienceGainText.text = $"+0";
        LevelText.text = $"{_details.InitialLevel}";

        // UnlockList.SetModel(...);
    }

    private void PlayAnimation(ExperienceAnimationDetails details)
    {
        const float SPEED = 500;
        float totalTime = details.ExperienceGain / SPEED;

        _experienceProgress = 0;
        _levelProgress = 0;

        _handle?.Kill();
        _handle = DOTween.To(Getter, Setter, details.ExperienceGain, totalTime).SetEase(Ease.Linear);
        _handle.SetAutoKill();
        _handle.Restart();
    }

    private int _experienceProgress = 0;
    private int _levelProgress = 0;

    public int Getter()
    {
        return _experienceProgress;
    }

    public void Setter(int value)
    {
        _experienceProgress = value;

        int currValue = _details.InitialExperience + _experienceProgress;
        if (currValue >= 1000)
        {
            // 升级
            currValue -= 1000;
            _levelProgress++;
            LevelText.text = $"{_details.InitialLevel + _levelProgress}";
        }

        // ExperienceSlider.value = currValue;
        ExperienceGainText.text = $"+{_experienceProgress}";
    }

    private void Return()
    {
        RunManager.Instance.ReturnToTitle();
    }
}
