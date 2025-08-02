
using CLLibrary;
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
    [SerializeField] private TMP_Text ExperienceGainText;
    [SerializeField] private TMP_Text ExperienceText;

    [SerializeField] private ListView UnlockList;

    private Tween _handle;
    private ExperienceAnimationDetails _details;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        ReturnButton.onClick.RemoveAllListeners();
        ReturnButton.onClick.AddListener(Return);

        Address address = new Address("Run.Environment.ActivePanel");
        MilestoneList.SetAddress(address.Append(".Milestones"));
        UnlockList.SetAddress(address.Append(".Achievements"));
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
        RunResultCell cell = RunManager.Instance.Environment.GetPanel() as RunResultCell;
        
        if (cell.GetRunOutcome() == RunResult.RunOutcome.Victorious)
        {
            OutcomeText.text = "胜利";
            SpriteEntry winIllustration = Encyclopedia.SpriteCategory.FromName("RunResultIllustrationWin");
            ResultIllustration.sprite = winIllustration.Sprite;
        }
        else
        {
            OutcomeText.text = "失败";
            SpriteEntry loseIllustration = Encyclopedia.SpriteCategory.FromName("RunResultIllustrationLose");
            ResultIllustration.sprite = loseIllustration.Sprite;
        }
        
        CharacterNameText.text = cell.GetCharacterName();
        DifficultyText.text = cell.GetDifficulty();
        PlayTimeText.text = cell.GetPlayTime();
    }

    private Tween MilestoneAnimation()
    {
        InitMilestoneList();
        Sequence seq = DOTween.Sequence();

        MilestoneList.Traversal().Do(slotView => {
            // AudioManager.PlayCardPlacement();
            seq.Append(slotView.GetContentView().GetRect().DOMove(slotView.GetRect().position, 0.15f))
                .Join((slotView.GetContentView() as MilestoneView).CanvasGroup.DOFade(1, 0.15f))
                .AppendCallback(() => slotView.GetAnimator().SetState(SlotView.IDLE));
        });

        seq.AppendInterval(0.2f);

        return seq;
        
        // _animationQueue.QueueAnimation(seq);
    }
    
    private void InitMilestoneList()
    {
        void SetInitialState(SlotView view, Vector3 position)
        {
            view.GetAnimator().SetState(SlotView.FREE);
            view.GetContentView().GetRect().position = position;
            MilestoneView delegatedView = view.GetContentView() as MilestoneView;
            delegatedView.CanvasGroup.alpha = 0;
        }
        
        MilestoneList.Sync();
        MilestoneList.ForceLayoutRebuild();

        MilestoneList.Traversal().Do(slotView =>
        {
            SetInitialState(slotView, slotView.GetRect().position + Vector3.right * 0.5f);
        });
    }

    private Tween UnlockAnimation()
    {
        InitUnlockList();
        Sequence seq = DOTween.Sequence();

        UnlockList.Traversal().Do(slotView => {
            // AudioManager.PlayCardPlacement();
            seq.Append(slotView.GetContentView().GetRect().DOMove(slotView.GetRect().position, 0.15f))
                .Join((slotView.GetContentView() as UnlockIcon).CanvasGroup.DOFade(1, 0.15f))
                .AppendCallback(() => slotView.GetAnimator().SetState(SlotView.IDLE));
        });

        seq.AppendInterval(0.2f);

        return seq;
    }

    private void InitUnlockList()
    {
        void SetInitialState(SlotView view, Vector3 position)
        {
            view.GetAnimator().SetState(SlotView.FREE);
            view.GetContentView().GetRect().position = position;
            UnlockIcon contentView = view.GetContentView() as UnlockIcon;
            contentView.CanvasGroup.alpha = 0;
        }
        
        UnlockList.Sync();
        UnlockList.ForceLayoutRebuild();

        UnlockList.Traversal().Do(slotView =>
        {
            SetInitialState(slotView, slotView.GetRect().position + Vector3.up * 0.25f);
        });
    }

    private Tween ScoringAnimation()
    {
        InitScoring();
        
        const float SPEED = 500;
        float totalTime = _details.ExperienceGain / SPEED;

        _experienceProgress = 0;
        return DOTween.To(Getter, Setter, _details.ExperienceGain, totalTime).SetEase(Ease.Linear);
    }

    private void InitScoring()
    {
        RunResultCell cell = RunManager.Instance.Environment.GetPanel() as RunResultCell;
        _details = new(cell.GetInitialExperience(), cell.GetInitialLevel(), cell.GetExperienceGain());

        LevelText.text = $"{_details.InitialLevel}";
        ExperienceGainText.text = $"{_details.ExperienceGain}";
        ExperienceText.text = $"{_details.InitialExperience}";
    }

    private int _experienceProgress;

    public int Getter()
    {
        return _experienceProgress;
    }

    public void Setter(int value)
    {
        _experienceProgress = value;

        int totalExperience = _details.InitialExperience + _experienceProgress;
        int levelCarry = totalExperience / 1000;
        int totalLevel = _details.InitialLevel + levelCarry;
        int restExperience = totalExperience - levelCarry * 1000;

        LevelText.text = $"{totalLevel}";
        ExperienceGainText.text = $"+{_details.ExperienceGain - _experienceProgress}";
        ExperienceText.text = $"{restExperience}";
    }

    private void Return()
    {
        RunManager.Instance.ReturnToTitle();
    }

    private Tween SetActiveTween()
    {
        gameObject.SetActive(true);
        return DOTween.Sequence();
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .Append(SetActiveTween())
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(HIDE))
            .Append(MilestoneAnimation())
            .Append(UnlockAnimation())
            .Append(ScoringAnimation())
        ;
}
