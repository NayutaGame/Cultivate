
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattlePanel : Panel
{
    [SerializeField] private BattleEntityView EnemyView;
    [SerializeField] private ReactionView ReactionView;
    
    [SerializeField] private TMP_Text HomeHealth;
    [SerializeField] private TMP_Text AwayHealth;
    [SerializeField] public CombatButton CombatButton;
    [SerializeField] private GameObject VictoryStamp;

    private static readonly float WinBaseScale = 1f;
    private static readonly float LoseBaseScale = 0.6f;

    [SerializeField] public Color WinColor;
    [SerializeField] public Color LoseColor;

    [SerializeField] public Sprite SmirkSprite;
    [SerializeField] public Sprite AfraidSprite;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        EnemyView.SetAddress(_address.Append(".Enemy"));

        CombatButton.ClickNeuron.Join(Combat);
    }

    public override void Refresh()
    {
        EnemyView.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        if (d.GetResult() is { } result)
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
        
        CanvasManager.Instance.RefreshGuide();
    }

    private Dictionary<string, Sprite> ReactionDict;

    private void OnEnable()
    {
        ReactionDict ??= new Dictionary<string, Sprite>()
            { { "Normal", null }, { "Smirk", SmirkSprite }, { "Afraid", AfraidSprite }, };
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.BeginDragNeuron.Join(ReactionFromBeginDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.EndDragNeuron.Join(ReactionFromEndDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DragNeuron.Join(ReactionFromDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DropNeuron.Join(ReactionFromDrop);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.BeginDragNeuron.Join(ReactionFromBeginDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.EndDragNeuron.Join(ReactionFromEndDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.DragNeuron.Join(ReactionFromDrag);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.BeginDragNeuron.Remove(ReactionFromBeginDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.EndDragNeuron.Remove(ReactionFromEndDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DragNeuron.Remove(ReactionFromDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.DropNeuron.Remove(ReactionFromDrop);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.BeginDragNeuron.Remove(ReactionFromBeginDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.EndDragNeuron.Remove(ReactionFromEndDrag);
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.DragNeuron.Remove(ReactionFromDrag);
    }

    private void ReactionFromBeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        object obj = ib.GetSimpleView().Get<object>();
        RunSkill skill;

        if (obj is SkillSlot slot)
        {
            skill = slot.Skill;
            
            EntityModel entity = EnemyView.Get<EntityModel>();
            string reactionKey = entity.GetReactionKeyFromSkill(skill);
            Sprite reactionSprite = ReactionDict[reactionKey];
            ReactionView.SetSprite(reactionSprite);
            return;
        }

        if (obj is RunSkill runSkill)
        {
            skill = runSkill;
            
            EntityModel entity = EnemyView.Get<EntityModel>();
            string reactionKey = entity.GetReactionKeyFromSkill(skill);
            Sprite reactionSprite = ReactionDict[reactionKey];
            ReactionView.SetSprite(reactionSprite);
            return;
        }
        
        Debug.Log($"BeginDrag, {obj}");
    }

    private void ReactionFromEndDrag(InteractBehaviour ib, PointerEventData d)
    {
        object obj = ib.GetSimpleView().Get<object>();

        // int intensity = GetIntensityAccordingToDistance();
        // reactionView.SetIntensity(intensity);
    }

    private void ReactionFromDrag(InteractBehaviour ib, PointerEventData d)
    {
        // reactionView.ResetState();
    }

    private void ReactionFromDrop(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        object toObj = to.GetSimpleView().Get<object>();
        // reactionView.ResetState();
    }

    private void Combat(PointerEventData eventData)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(new ClickBattleSignal());
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void SetVictory(bool victory)
    {
        CombatButton.SetBaseScale(victory ? WinBaseScale : LoseBaseScale);
        VictoryStamp.SetActive(victory);
        HomeHealth.color = victory ? WinColor : LoseColor;
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

    public override Tween ShowTween()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .AppendCallback(PlayBattleBGM)
            .Append(EnemyView.ShowTween());
    }

    public override Tween HideTween()
    {
        return DOTween.Sequence()
            .AppendCallback(PlayJingJieBGM)
            .Append(EnemyView.HideTween())
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
