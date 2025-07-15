
using System;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class BattlePanel : Panel
{
    [SerializeField] private BattleEntityView EnemyView;
    
    [SerializeField] private TMP_Text HomeHealth;
    [SerializeField] private PropagatePointer HomePropagatePointer;
    [SerializeField] private RectTransform HomeHealthTransform;
    
    [SerializeField] private TMP_Text AwayHealth;
    [SerializeField] private PropagatePointer AwayPropagatePointer;
    [SerializeField] private RectTransform AwayHealthTransform;
    
    [SerializeField] public CombatButton CombatButton;
    [SerializeField] private GameObject VictoryStamp;

    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    [SerializeField] public Color WinColor;
    [SerializeField] public Color LoseColor;

    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));

        _index = 0;
        CombatButton.Configure(_index);
        CombatButton.LeftClickNeuron.Join(Combat);
        CombatButton.RightClickNeuron.Join(NextCombatAction);
    }
    
    public static readonly int DEFAULT = -1;
    public new static readonly int HIDE = 0;
    public new static readonly int IDLE = 1;
    public static readonly int HOVER = 2;

    protected override Animator InitAnimator()
    {
        // 0 for hide, 1 for show
        Animator animator = new(2, "Battle Panel");
        animator[HIDE, IDLE] = EnterIdle;
        animator[IDLE, IDLE] = SelfTransitionTween;
        animator[DEFAULT, HIDE] = EnterHide;
        
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

    private void PointerEnterHomeHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerEnter(HomeHealthTransform, d, $"开始战斗时气血上限为{RunManager.Instance.Environment.Home.GetHealth()}");
    }

    private void PointerExitHomeHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerExit(d);
    }

    private void PointerEnterAwayHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerEnter(AwayHealthTransform, d, $"开始战斗时气血上限为{RunManager.Instance.Environment.Away.GetHealth()}");
    }

    private void PointerExitAwayHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerExit(d);
    }

    private Dictionary<string, Sprite> ReactionDict;

    private void OnEnable()
    {
        ReactionDict ??= new Dictionary<string, Sprite>()
            { { RunEntity.NORMAL_KEY, null }, { RunEntity.SMIRK_KEY, Encyclopedia.SpriteCategory["Smirk"].Sprite }, { RunEntity.AFRAID_KEY, Encyclopedia.SpriteCategory["Afraid"].Sprite }, };
        
        HomePropagatePointer._onPointerEnter = PointerEnterHomeHealth;
        HomePropagatePointer._onPointerExit = PointerExitHomeHealth;
        AwayPropagatePointer._onPointerEnter = PointerEnterAwayHealth;
        AwayPropagatePointer._onPointerExit = PointerExitAwayHealth;

        RunManager.Instance.Environment.FieldChangedNeuron.Add(RefreshEnemy);
        RunManager.Instance.Environment.FieldChangedNeuron.Add(RefreshOperationPanel);
    }

    private void OnDisable()
    {
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.BeginDragNeuron.Remove(ReactionFromBeginDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.EndDragNeuron.Remove(ReactionFromEndDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DragNeuron.Remove(ReactionFromDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DropNeuron.Remove(ReactionFromDrop);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.BeginDragNeuron.Remove(ReactionFromBeginDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.EndDragNeuron.Remove(ReactionFromEndDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.DragNeuron.Remove(ReactionFromDrag);
        
        HomePropagatePointer._onPointerEnter -= PointerEnterHomeHealth;
        HomePropagatePointer._onPointerExit -= PointerExitHomeHealth;
        AwayPropagatePointer._onPointerEnter -= PointerEnterAwayHealth;
        AwayPropagatePointer._onPointerExit -= PointerExitAwayHealth;
        
        RunManager.Instance.Environment.FieldChangedNeuron.Remove(RefreshEnemy);
        RunManager.Instance.Environment.FieldChangedNeuron.Remove(RefreshOperationPanel);
    }

    private Action[] CombatActions = new Action[] { CombatNormal, CombatOnlyAnimation, CombatOnlyResult, };
    private int _index;

    private void Combat(PointerEventData eventData)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new ClickCombatSignal());
        CanvasManager.Instance.RefreshGuide();
        CombatActions[_index]();
    }

    private void NextCombatAction(PointerEventData d)
    {
        _index++;
        if (_index >= CombatActions.Length)
            _index = 0;
        CombatButton.IconPlaceHolder.sprite = CombatButton.Icons[_index];
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
    }

    private void SetVictory(bool victory)
    {
        VictoryStamp.SetActive(victory);
        HomeHealth.color = victory ? WinColor : LoseColor;
        CombatButton.SetAttractive(victory);
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
        JingJie jingJie = RunManager.Instance.Environment.JingJie;
        AudioEntry audio = Encyclopedia.AudioFromJingJie(jingJie);
        AudioManager.Play(audio);
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
