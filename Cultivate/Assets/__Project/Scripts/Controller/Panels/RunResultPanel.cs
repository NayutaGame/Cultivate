
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

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Dialog Panel");
        animator[0, 1] = EnterIdle;
        animator[-1, 0] = EnterHide;
        
        animator.SetState(0);
        return animator;
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

        Address address = new Address("Run.Environment.ActivePanel");
        MilestoneList.SetAddress(address.Append(".Milestones"));

        _details = new(panelDescriptor.GetInitialExperience(), panelDescriptor.GetInitialLevel(), panelDescriptor.GetExperienceGain());

        // ExperienceSlider.value = _details.InitialExperience;
        ExperienceGainText.text = $"+0";
        LevelText.text = $"{_details.InitialLevel}";

        // UnlockList.SetModel(...);
    }

    private Tween ScoringAnimation()
    {
        const float SPEED = 500;
        float totalTime = _details.ExperienceGain / SPEED;

        _experienceProgress = 0;
        _levelProgress = 0;

        return DOTween.To(Getter, Setter, _details.ExperienceGain, totalTime).SetEase(Ease.Linear);
    }

    private int _experienceProgress;
    private int _levelProgress;

    public int Getter()
    {
        return _experienceProgress;
    }

    public void Setter(int value)
    {
        _experienceProgress = value;

        int total = _details.InitialExperience + _experienceProgress;
        _levelProgress = total / 1000;
        LevelText.text = $"{_details.InitialLevel + _levelProgress}";

        // ExperienceSlider.value = currValue;
        ExperienceGainText.text = $"+{_experienceProgress}";
    }

    private void Return()
    {
        RunManager.Instance.ReturnToTitle();
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE))
            // .Append(ScoringAnimation())
        ;
}
