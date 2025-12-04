
using System;
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class RunCanvas : Panel
{
    public DeckPanel DeckPanel;
    public MapPanel MapPanel;
    public Button MapButton;
    public TopBar TopBar;

    private PanelSM PanelSM;

    public BattlePanel BattlePanel;
    public PuzzlePanel PuzzlePanel;
    public DialogPanel DialogPanel;
    public DiscoverPanel DiscoverPanel;
    public RequirePanel RequirePanel;
    public ShopPanel ShopPanel;
    public BarterPanel BarterPanel;
    public GachaPanel GachaPanel;
    public PickPanel PickPanel;
    public ImagePanel ImagePanel;
    public ComicPanel ComicPanel;
    public CommitPanel CommitPanel;
    public NarrativePanel NarrativePanel;
    public ChoicePanel ChoicePanel;
    public DicePanel DicePanel;
    public ScribblePanel ScribblePanel;
    
    private AnimationQueue _animationQueue;
    public AnimationQueue GetAnimationQueue() => _animationQueue;
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _animationQueue = new();
        // _animationQueue.EnqueueNeuron.Join(() => Debug.Log(_animationQueue.Count()));
        // _animationQueue.DequeueNeuron.Join(() => Debug.Log(_animationQueue.Count()));

        PanelSM = new(new Panel[]
        {
            null,
            BattlePanel,
            PuzzlePanel,
            DialogPanel,
            DiscoverPanel,
            RequirePanel,
            ShopPanel,
            BarterPanel,
            GachaPanel,
            PickPanel,
            ImagePanel,
            ComicPanel,
            CommitPanel,
            NarrativePanel,
            ChoicePanel,
            DicePanel,
            ScribblePanel,
        });

        // _panelDict.Do(kvp => kvp.Value.Configure());

        DeckPanel.CheckAwake();
        MapPanel.CheckAwake();
        TopBar.CheckAwake();
    }

    private void RefreshPanel()
    {
        ChangePanel(RunManager.Instance.Environment.Cell);
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Add(GainSkillStaging);
        RunManager.Instance.Environment.RemoveSkillNeuron.Add(RemoveSkillStaging);
        RunManager.Instance.Environment.SkillSetJingJieNeuron.Add(SkillSetJingJieStaging);
        RunManager.Instance.Environment.ReplaceSkillNeuron.Add(ReplaceSkillStaging);
        
        RunManager.Instance.Environment.EquipNeuron.Add(EquipStaging);
        RunManager.Instance.Environment.SwapNeuron.Add(SwapStaging);
        RunManager.Instance.Environment.UnequipNeuron.Add(UnequipStaging);
        RunManager.Instance.Environment.MergeNeuron.Add(MergeStaging);
        
        RunManager.Instance.Environment.PanelChangedNeuron.Add(ChangePanel);
        RunManager.Instance.Environment.PanelChangedNeuron.Add(EnterPanelSound);
        
        RunManager.Instance.Environment.GainMingYuanNeuron.Add(AudioManager.PlayGainMingYuan);
        
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(MingYuanDamageStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(AudioManager.PlayLoseMingYuan);
        
        RunManager.Instance.Environment.GainHealthNeuron.Add(AudioManager.PlayGainMaxHealth);
        
        RunManager.Instance.Environment.GainGoldNeuron.Add(AudioManager.PlayGainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Add(AudioManager.PlayLoseGold);
        
        RefreshPanel();
        
        AppManager.Instance.PushEscFunc(TopBar.OpenMenu);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Remove(GainSkillStaging);
        RunManager.Instance.Environment.RemoveSkillNeuron.Remove(RemoveSkillStaging);
        RunManager.Instance.Environment.SkillSetJingJieNeuron.Remove(SkillSetJingJieStaging);
        RunManager.Instance.Environment.ReplaceSkillNeuron.Remove(ReplaceSkillStaging);
        
        RunManager.Instance.Environment.EquipNeuron.Remove(EquipStaging);
        RunManager.Instance.Environment.SwapNeuron.Remove(SwapStaging);
        RunManager.Instance.Environment.UnequipNeuron.Remove(UnequipStaging);
        RunManager.Instance.Environment.MergeNeuron.Remove(MergeStaging);
        
        RunManager.Instance.Environment.PanelChangedNeuron.Remove(ChangePanel);
        RunManager.Instance.Environment.PanelChangedNeuron.Remove(EnterPanelSound);
        
        RunManager.Instance.Environment.GainMingYuanNeuron.Remove(AudioManager.PlayGainMingYuan);
        
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(MingYuanDamageStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(AudioManager.PlayLoseMingYuan);
        
        RunManager.Instance.Environment.GainHealthNeuron.Remove(AudioManager.PlayGainMaxHealth);
        
        RunManager.Instance.Environment.GainGoldNeuron.Remove(AudioManager.PlayGainGold);
        RunManager.Instance.Environment.LoseGoldNeuron.Remove(AudioManager.PlayLoseGold);
        
        AppManager.Instance.PopEscFunc();
    }

    private readonly Dictionary<Type, string> _panelSoundMap = new Dictionary<Type, string>
    {
        { typeof(DialogCell), "EnterAdventure" },
        { typeof(ShopCell), "EnterShop" }
    };

    private void EnterPanelSound(PanelChangedDetails d)
    {
        if (d.FromPanel == d.ToPanel)
            return;
        
        if (_panelSoundMap.TryGetValue(d.ToPanel.GetType(), out string soundName))
        {
            AudioManager.Play(soundName);
        }
    }

    private void ChangePanel(PanelChangedDetails d)
        => ChangePanelAsync(d);
    
    private void ChangePanel(ICellAdapter toCell)
        => ChangePanelAsync(toCell);

    private async UniTask ChangePanelAsync(PanelChangedDetails panelChangedDetails)
        => await ChangePanelAsync(panelChangedDetails.ToPanel);
    
    private async UniTask ChangePanelAsync(ICellAdapter toCell)
    {
        await _animationQueue.WaitForQueueToComplete();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = PanelS.FromPanelDescriptor(toCell?.AsCell());

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                await PanelSM.GetCurrPanel().GetAnimator().SetStateAsync(1);
            return;
        }

        if (PanelSM[oldState] != null)
            await PanelSM[oldState].GetAnimator().SetStateAsync(0);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].CheckAwake();
            PanelSM[newState].Refresh();
            await PanelSM[newState].GetAnimator().SetStateAsync(1);
        }

        {
            // TODO： 这里报过一个很奇怪的错误
            // 环境是Run胜利，回到Title，再进入Run，BattlePanel中，打开卡牌Annotation，查看卡包
            // 然后过了一小段时间，不到一秒，报了错，下面这句为空，大概率是上一场对局的环境没有清理干净，还有一个线程再跑，然后下面访问Environment自然是空
            Cell d = RunManager.Instance.Environment.Cell.AsCell();
            bool showDeck = d is BattleCell || d is RequireCell || d is PuzzleCell ||
                            d is DiscoverCell;

            await DeckPanel.GetAnimator().SetStateAsync(showDeck ? 2 : 0);
        }

        {
            bool showMap = newState.Index == 0;
            await MapPanel.GetAnimator().SetStateAsync(showMap ? 2 : 0);
        }
    }

    public void SetPanelToNull()
    {
        _animationQueue.CompleteAllAnimations();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = PanelS.FromHide();
        
        PanelSM[oldState].GetAnimator().SetState(0);
        PanelSM.SetState(newState);
        GetAnimator().SetState(0);
    }

    public Neuron<Predicate<RunSkill>> HighlightQualifiersNeuron = new();
    public Neuron UnhighlightQualifiersNeuron = new();

    #region Staging

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true));
    
    private void GainSkillStaging(GainSkillBuilder b)
    {
        void SetPosition(SlotView view, Vector3 position)
        {
            view.GetAnimator().SetState(SlotView.FREE);
            view.GetContentView().GetRect().position = position;
            view.GetContentView().GetRect().localScale = Vector3.zero;
        }
        
        void SetShow(SlotView view)
        {
            AudioManager.PlayGainSkill();
            view.GetAnimator().SetTweenAsync(view.GetContentView().GetRect().DOScale(1, 0.15f));
        }
        
        void SetIdle(SlotView view)
        {
            AudioManager.PlayCardPlacement();
            view.GetAnimator().SetStateAsync(1);
        }

        b.GainingSkills.Do(gainingSkill =>
        {
            IDeckIndex deckIndex = gainingSkill.GetDeckIndex();

            if (deckIndex.Region == SkillRegion.Field)
            {
                DeckPanel.PlayerEntity.FieldView.Modified(deckIndex.Index);
            }
            else if (deckIndex.Region == SkillRegion.Hand)
            {
                DeckPanel.HandView.InsertItem(deckIndex.Index);
            }
        });
        
        Vector3 position = Vector3.zero;
        int offset = 1;
        
        for (int i = 0; i < b.GainingSkills.Count; i++)
        {
            SlotView view = DeckPanel.SkillItemFromDeckIndex(b.GainingSkills[i].GetDeckIndex().Reify());
            Vector3 showPosition = position + i * offset * Vector3.left;
            SetPosition(view, showPosition);
        }
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);
        
        foreach (GainingSkill gainingSkill in b.GainingSkills)
        {
            DeckIndex deckIndex = gainingSkill.GetDeckIndex().Reify();
            SlotView view = DeckPanel.SkillItemFromDeckIndex(deckIndex);
            seq.AppendCallback(() => SetShow(view))
                .AppendInterval(0.15f);
        }
        
        seq.AppendInterval(0.2f);
        
        foreach (GainingSkill gainingSkill in b.GainingSkills)
        {
            DeckIndex deckIndex = gainingSkill.GetDeckIndex().Reify();
            SlotView view = DeckPanel.SkillItemFromDeckIndex(deckIndex);
            seq.AppendCallback(() => SetIdle(view))
                .AppendInterval(0.1f);
        }
        
        _animationQueue.QueueAnimation(seq);
    }

    private void RemoveSkillStaging(RemoveSkillDetails d)
    {
        if (d.DeckIndex.Region == SkillRegion.Field)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else if (d.DeckIndex.Region == SkillRegion.Hand)
        {
            DeckPanel.HandView.RemoveItemAt(d.DeckIndex.Index);
        }
    }

    private void SkillSetJingJieStaging(SkillSetJingJieDetails d)
    {
        if (d.DeckIndex.Region == SkillRegion.Field)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else if (d.DeckIndex.Region == SkillRegion.Hand)
        {
            DeckPanel.HandView.Modified(d.DeckIndex.Index);
        }
        else if (d.DeckIndex.Region == SkillRegion.Requirement)
        {
            RequirePanel.Requirements.Modified(d.DeckIndex.Index);
        }
    }

    private void ReplaceSkillStaging(ReplaceSkillDetails d)
    {
        if (d.DeckIndex.Region == SkillRegion.Field)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else if (d.DeckIndex.Region == SkillRegion.Hand)
        {
            DeckPanel.HandView.Modified(d.DeckIndex.Index);
        }
        else if (d.DeckIndex.Region == SkillRegion.Requirement)
        {
            RequirePanel.Requirements.Modified(d.DeckIndex.Index);
        }
    }

    private void MergeStaging(MergeDetails d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(2, null);
        
        SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
        SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        from.GetAnimator().SetStateAsync(1);
        
        DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        if (d.FromDeckIndex.Index > d.ToDeckIndex.Index)
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index);
        else
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index - 1);
        
        AudioManager.Play("CardUpgrade");
    }

    private void EquipStaging(EquipDetails d)
    {
        if (d.IsReplace)
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.HandView.Modified(d.FromDeckIndex.Index);
        }
        else
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.GetAnimator().SetStateAsync(1);
            
            DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        }
    }

    private void SwapStaging(SwapDetails d)
    {
        if (d.IsReplace)
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex);
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex);
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
    }

    private void UnequipStaging(UnequipDetails d)
    {
        DeckPanel.HandView.AddItem();
        
        SlotView from = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex);
        SlotView to = DeckPanel.LatestSkillItem();
        
        DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        from.GetAnimator().SetStateAsync(1);
        
        AudioManager.Play("CardPlacement");
    }

    public void BuySkillStaging(BuySkillDetails d)
    {
        void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(SlotView.FREE);
            view.GetContentView().GetRect().position = position;
            view.GetContentView().GetRect().localScale = localScale;
        }
        
        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Play("CardPlacement");
        // AudioManager.Instance.Play("钱币");
        
        DeckPanel.HandView.AddItem();
        
        SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex);
        SlotView commodityView = ShopPanel.CommodityItemFromIndex(d.CommodityIndex) as SlotView;
        
        ShopPanel.ListView.RemoveItemAt(d.CommodityIndex);
        
        SetPosition(view, commodityView.GetRect().position, commodityView.GetRect().localScale);
        SetIdle(view);
    }

    public void ExchangeSkillStaging(ExchangeSkillDetails d)
    {
        // void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        // {
        //     view.GetAnimator().SetState(SlotView.FREE);
        //     view.GetContentView().GetRect().position = position;
        //     view.GetContentView().GetRect().localScale = localScale;
        // }
        //
        // void SetIdle(SlotView view)
        // {
        //     view.GetAnimator().SetStateAsync(1);
        // }
        //
        // // AudioManager.Play("CardPlacement");
        // // AudioManager.Instance.Play("钱币");
        //
        // SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex);
        // SlotView barterItemView = BarterPanel.BarterItemFromIndex(d.BarterItemIndex) as SlotView;
        //
        // if (d.DeckIndex.Region == SkillRegion.Hand)
        //     DeckPanel.HandView.Modified(d.DeckIndex.Index);
        // else if (d.DeckIndex.Region == SkillRegion.Field)
        //     DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        // BarterPanel.ListView.RemoveItemAt(d.BarterItemIndex);
        // BarterPanel.ListView.Sync();
        //
        // SetPosition(view, barterItemView.GetRect().position, barterItemView.GetRect().localScale);
        // SetIdle(view);
    }

    public void GachaStaging(GachaDetails d)
    {
        void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(SlotView.FREE);
            view.GetContentView().GetRect().position = position;
            view.GetContentView().GetRect().localScale = localScale;
        }
        
        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Instance.Play("钱币");
        
        DeckPanel.HandView.AddItem();
        
        SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex);
        SlotView gachaItemView = GachaPanel.GachaItemFromIndex(d.GachaIndex) as SlotView;
        
        GachaPanel.ListView.RemoveItemAt(d.GachaIndex);
        
        SetPosition(view, gachaItemView.GetRect().position, gachaItemView.GetRect().localScale);
        SetIdle(view);
    }

    public void MingYuanDamageStaging(int value)
    {
        CanvasManager.Instance.RedFlashAnimation();
        CanvasManager.Instance.CanvasShakeAnimation();
        CanvasManager.Instance.UIFloatTextVFX(value.ToString(), Color.red);
    }

    #endregion
}
