
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    [SerializeField] private BattleEntityView EnemyView;
    [SerializeField] private ReactionView ReactionView;
    
    [SerializeField] private TMP_Text HomeHealth;
    [SerializeField] private PropagatePointer HomePropagatePointer;
    [SerializeField] private RectTransform HomeHealthTransform;
    
    [SerializeField] private TMP_Text AwayHealth;
    [SerializeField] private PropagatePointer AwayPropagatePointer;
    [SerializeField] private RectTransform AwayHealthTransform;
    
    [SerializeField] public CombatButton CombatButton;
    [SerializeField] private Button SkipButton;
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

        CombatButton.Configure();
        CombatButton.ClickNeuron.Join(Combat);
        
        if (SkipButton != null)
        {
            SkipButton.onClick.RemoveAllListeners();
            SkipButton.onClick.AddListener(Skip);
        }
    }

    public override void Refresh()
    {
        RefreshField();
        
        CanvasManager.Instance.RefreshGuide();
    }

    private void RefreshField()
    {
        EnemyView.Refresh();
        
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
        CanvasManager.Instance.TextHint.PointerEnter(HomeHealthTransform, d, $"开始战斗时气血上限为{RunManager.Instance.Environment.Home.GetFinalHealth()}");
    }

    private void PointerExitHomeHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerExit(d);
    }

    private void PointerEnterAwayHealth(PointerEventData d)
    {
        if (d.dragging) return;
        CanvasManager.Instance.TextHint.PointerEnter(AwayHealthTransform, d, $"开始战斗时气血上限为{RunManager.Instance.Environment.Away.GetFinalHealth()}");
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
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.BeginDragNeuron.Join(ReactionFromBeginDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.EndDragNeuron.Join(ReactionFromEndDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DragNeuron.Join(ReactionFromDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DropNeuron.Join(ReactionFromDrop);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.BeginDragNeuron.Join(ReactionFromBeginDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.EndDragNeuron.Join(ReactionFromEndDrag);
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.DragNeuron.Join(ReactionFromDrag);
        
        HomePropagatePointer._onPointerEnter = PointerEnterHomeHealth;
        HomePropagatePointer._onPointerExit = PointerExitHomeHealth;
        AwayPropagatePointer._onPointerEnter = PointerEnterAwayHealth;
        AwayPropagatePointer._onPointerExit = PointerExitAwayHealth;
        
        RunManager.Instance.Environment.FieldChangedNeuron.Add(RefreshField);
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
        
        RunManager.Instance.Environment.FieldChangedNeuron.Remove(RefreshField);
    }

    private void ReactionFromBeginDrag(LegacyInteractBehaviour ib, PointerEventData d)
    {
        object obj = ib.GetSimpleView().Get<object>();
        RunSkill skill;

        if (obj is SkillSlot slot)
        {
            skill = slot.Skill;
            if (skill == null)
                return;
            
            IEntity entity = EnemyView.Get<IEntity>();
            string reactionKey = entity.GetReactionKeyFromSkill(skill);
            Sprite reactionSprite = ReactionDict[reactionKey];
            ReactionView.BeginDrag(reactionSprite, IntensityFromMousePosition(d.position));
            return;
        }

        if (obj is RunSkill runSkill)
        {
            skill = runSkill;
            
            IEntity entity = EnemyView.Get<IEntity>();
            string reactionKey = entity.GetReactionKeyFromSkill(skill);
            Sprite reactionSprite = ReactionDict[reactionKey];
            ReactionView.BeginDrag(reactionSprite, IntensityFromMousePosition(d.position));
            return;
        }
        
        Debug.Log($"Error: BeginDrag, {obj}");
    }

    private void ReactionFromEndDrag(LegacyInteractBehaviour ib, PointerEventData d)
    {
        ReactionView.EndDrag();
    }

    private void ReactionFromDrop(LegacyInteractBehaviour from, LegacyInteractBehaviour to, PointerEventData d)
    {
        ReactionView.EndDrag();
    }

    private void ReactionFromDrag(LegacyInteractBehaviour ib, PointerEventData d)
    {
        float intensity = IntensityFromMousePosition(d.position);
        ReactionView.Drag(intensity);
    }

    private float IntensityFromMousePosition(Vector2 mouseUIPosition)
    {
        Vector3 reactionPosition = ReactionView.transform.position;
        Vector3 mouseWorldPosition = CanvasManager.Instance.UI2World(mouseUIPosition);
        return Vector3.Distance(reactionPosition, mouseWorldPosition).Remap(2, 10, 1, 0.1f);
    }

    private void Combat(PointerEventData eventData)
    {
        RunManager.Instance.Environment.LegacyReceiveSignalProcedure(new ClickCombatSignal());
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void Skip()
    {
        RunEnvironment env = RunManager.Instance.Environment;
        Signal signal = new SkipCombatSignal(env.GetSimulateResult().Flag == 1);
        CanvasManager.Instance.RunCanvas.LegacySetPanelSAsyncFromSignal(signal);
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
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(PlayBattleBGM)
            .Append(CanvasManager.Instance.Curtain.GetAnimator().TweenFromSetState(0))
            .Append(EnemyView.ShowTween());

    public override Tween EnterHide()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayJingJieBGM)
            .Append(EnemyView.HideTween())
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
