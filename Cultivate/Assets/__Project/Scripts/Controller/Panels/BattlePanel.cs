
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattlePanel : Panel
{
    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    [SerializeField] public TMP_FontAsset HomeWinFontAsset;
    [SerializeField] public TMP_FontAsset HomeLoseFontAsset;
    [SerializeField] public Color HomeWinColor;
    [SerializeField] public Color HomeLoseColor;
    [SerializeField] public Color AwayWinColor;
    [SerializeField] public Color AwayLoseColor;
    
    [SerializeField] private BattleEntityView EnemyView;
    
    [SerializeField] private TMP_Text HomeHealth;
    [SerializeField] private XView HomeHealthAnnotationProvider;
    [SerializeField] private TMP_Text AwayHealth;
    [SerializeField] private XView AwayHealthAnnotationProvider;

    [SerializeField] public CombatButton CombatButton;
    private Action[] CombatActions = new Action[] { CombatNormal, CombatOnlyAnimation, CombatOnlyResult, };

    private Address _address;
    private bool _isVictory;
    private int _combatActionIndex;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));
        
        CombatButton.Button.LeftClickNeuron.Join(Combat);
        CombatButton.Button.RightClickNeuron.Join(NextCombatAction);
        ResetCombatAction();
        
        HomeHealthAnnotationProvider.SetAddress(new("Run.Environment.Home.HealthDescription"));
        AwayHealthAnnotationProvider.SetAddress(new("Run.Environment.Away.HealthDescription"));
    }

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Battle Panel");
        animator[HIDE, IDLE] = EnterIdle;
        animator[IDLE, IDLE] = SelfTransitionTween;
        animator[ANY, HIDE] = EnterHide;
        
        animator.SetState(HIDE);
        return animator;
    }

    public override void Refresh()
    {
        RefreshEnemy();
        RefreshOperationPanel();
        CanvasManager.Instance.RefreshGuide();
    }

    private void RefreshEnemy()
    {
        EnemyView.Refresh();
    }

    private void RefreshOperationPanel()
    {
        // BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        if (RunManager.Instance.Environment.GetSimulateResult() is { } result)
        {
            HomeHealth.text = result.HomeLeftHp.ToString();
            AwayHealth.text = result.AwayLeftHp.ToString();
            SetVictory(result.Flag == 1);
        }
        else
        {
            HomeHealth.text = "玩家";
            AwayHealth.text = "怪物";
            SetVictory(false);
            
            HomeHealth.alpha = 1f;
            AwayHealth.alpha = 1f;
        }
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.ResimulateNeuron.Add(RefreshEnemy);
        RunManager.Instance.Environment.ResimulateNeuron.Add(RefreshOperationPanel);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.ResimulateNeuron.Remove(RefreshEnemy);
        RunManager.Instance.Environment.ResimulateNeuron.Remove(RefreshOperationPanel);
    }
    
    private void SetVictory(bool victory)
    {
        HomeHealth.font = victory ? HomeWinFontAsset : HomeLoseFontAsset;
        HomeHealth.color = victory ? HomeWinColor : HomeLoseColor;
        AwayHealth.color = victory ? AwayLoseColor : AwayWinColor;
        // CombatButton.SetAttractive(victory);
    }

    private void Combat(InteractBehaviour ib, PointerEventData d)
    {
        RunManager.Instance.Environment.Map.ReceiveSignalProcedure(new ClickCombatSignal());
        CanvasManager.Instance.RefreshGuide();
        CombatActions[_combatActionIndex]();
    }

    private void ResetCombatAction()
    {
        _combatActionIndex = 0;
        CombatButton.IconPlaceHolder.sprite = CombatButton.Icons[_combatActionIndex];
    }
    
    private void NextCombatAction(InteractBehaviour ib, PointerEventData d)
    {
        _combatActionIndex++;
        if (_combatActionIndex >= CombatActions.Length)
            _combatActionIndex = 0;
        CombatButton.IconPlaceHolder.sprite = CombatButton.Icons[_combatActionIndex];
    }

    private static void CombatNormal()
    {
        RunManager.Instance.Environment.CombatNormal();
    }

    private static void CombatOnlyAnimation()
    {
        RunManager.Instance.Environment.CombatOnlyAnimation();
    }

    private static void CombatOnlyResult()
    {
        RunManager.Instance.Environment.CombatOnlyResult();
        CanvasManager.Instance.RefreshGuide();
    }

    private void PlayBattleBGM()
    {
        int index = RandomManager.Range(0, 3);
        string bgm = new string[] { "BGMBoss", "BGMElite1", "BGMElite2" }[index];
        AudioManager.Play(bgm);
    }

    private void PlayJingJieBGM()
    {
        if (RunManager.Instance == null || RunManager.Instance.Environment == null)
            return;
        AudioManager.Play(RunManager.Instance.Environment.Map.JingJie.GetAudio());
    }

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(RefreshEnemy)
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(PlayBattleBGM)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(Panel.HIDE))
            .Append(EnemyView.ShowTween());

    public Tween SelfTransitionTween()
        => DOTween.Sequence()
            .AppendCallback(Refresh);

    public override Tween EnterHide()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayJingJieBGM)
            .Append(EnemyView.HideTween())
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
