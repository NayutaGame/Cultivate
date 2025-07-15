
using System;
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RunCanvas : Panel
{
    public DeckPanel DeckPanel;
    public MapPanel MapPanel;
    public Button MapButton;
    public TopBar TopBar;
    public ConsolePanel ConsolePanel;

    private PanelSM PanelSM;

    public BattlePanel BattlePanel;
    public PuzzlePanel PuzzlePanel;
    public DialogPanel DialogPanel;
    public DiscoverSkillPanel DiscoverSkillPanel;
    public CardPickerPanel CardPickerPanel;
    public ShopPanel ShopPanel;
    public BarterPanel BarterPanel;
    public GachaPanel GachaPanel;
    public ArbitraryCardPickerPanel ArbitraryCardPickerPanel;
    public ImagePanel ImagePanel;
    public ComicPanel ComicPanel;
    public RunResultPanel RunResultPanel;
    
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
            DiscoverSkillPanel,
            CardPickerPanel,
            ShopPanel,
            BarterPanel,
            GachaPanel,
            ArbitraryCardPickerPanel,
            ImagePanel,
            ComicPanel,
            RunResultPanel,
        });

        // _panelDict.Do(kvp => kvp.Value.Configure());

        DeckPanel.CheckAwake();
        MapPanel.CheckAwake();
        TopBar.CheckAwake();
        ConsolePanel.CheckAwake();
        
        ConsolePanel.gameObject.SetActive(!AppManager.Instance.AudienceIsPlayer());
    }

    private void RefreshPanel()
    {
        ChangePanel(RunManager.Instance.Environment.GetPanel());
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
    
    private void ChangePanel(Cell toPanel)
        => ChangePanelAsync(toPanel);

    private async UniTask ChangePanelAsync(PanelChangedDetails panelChangedDetails)
        => await ChangePanelAsync(panelChangedDetails.ToPanel);
    
    private async UniTask ChangePanelAsync(Cell toPanel)
    {
        await _animationQueue.WaitForQueueToComplete();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = PanelS.FromPanelDescriptor(toPanel);

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

        Cell d = RunManager.Instance.Environment.GetPanel();
        bool showDeck = d is BattleCell || d is CardPickerCell || d is PuzzleCell ||
                        d is DiscoverSkillCell;

        await DeckPanel.GetAnimator().SetStateAsync(showDeck ? 2 : 0);
    }

    public void SetPanelToNull()
    {
        _animationQueue.CompleteAnimationQueue();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = PanelS.FromHide();
        
        PanelSM[oldState].GetAnimator().SetState(0);
        PanelSM.SetState(newState);
        GetAnimator().SetState(0);
    }

    #region Staging

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true));
    
    private void GainSkillStaging(GainSkillBuilder b)
    {
        void SetPosition(SlotView view, Vector3 position)
        {
            view.GetAnimator().SetState(4);
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

        b.PreferredDeckIndices.Do(deckIndex =>
        {
            if (deckIndex.InField)
            {
                DeckPanel.PlayerEntity.FieldView.Modified(deckIndex.Index);
            }
            else
            {
                DeckPanel.HandView.InsertItem(deckIndex.Index);
            }
        });
        
        Vector3 position = Vector3.zero;
        int offset = 1;
        
        for (int i = 0; i < b.PreferredDeckIndices.Count; i++)
        {
            SlotView view = DeckPanel.SkillItemFromDeckIndex(b.PreferredDeckIndices[i].Reify()) as SlotView;
            Vector3 showPosition = position + i * offset * Vector3.left;
            SetPosition(view, showPosition);
        }
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);
        
        foreach (DeckIndex deckIndex in b.PreferredDeckIndices)
        {
            SlotView view = DeckPanel.SkillItemFromDeckIndex(deckIndex) as SlotView;
            seq.AppendCallback(() => SetShow(view))
                .AppendInterval(0.15f);
        }
        
        seq.AppendInterval(0.2f);
        
        foreach (DeckIndex deckIndex in b.PreferredDeckIndices)
        {
            SlotView view = DeckPanel.SkillItemFromDeckIndex(deckIndex) as SlotView;
            seq.AppendCallback(() => SetIdle(view))
                .AppendInterval(0.1f);
        }
        
        _animationQueue.QueueAnimation(seq);
    }

    private void RemoveSkillStaging(RemoveSkillDetails d)
    {
        if (d.DeckIndex.InField)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else
        {
            DeckPanel.HandView.RemoveItemAt(d.DeckIndex.Index);
        }
    }

    private void SkillSetJingJieStaging(SkillSetJingJieDetails d)
    {
        if (d.DeckIndex.InField)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else
        {
            DeckPanel.HandView.Modified(d.DeckIndex.Index);
        }
    }

    private void ReplaceSkillStaging(ReplaceSkillDetails d)
    {
        if (d.DeckIndex.InField)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else
        {
            DeckPanel.HandView.Modified(d.DeckIndex.Index);
        }
    }

    private void EquipStaging(EquipDetails d)
    {
        if (d.IsReplace)
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as SlotView;
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as SlotView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.HandView.Modified(d.FromDeckIndex.Index);
        }
        else
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as SlotView;
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as SlotView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.GetAnimator().SetStateAsync(1);
            
            DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    private void SwapStaging(SwapDetails d)
    {
        if (d.IsReplace)
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as SlotView;
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as SlotView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as SlotView;
            SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as SlotView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    private void UnequipStaging(UnequipDetails d)
    {
        DeckPanel.HandView.AddItem();
        
        SlotView from = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as SlotView;
        SlotView to = DeckPanel.LatestSkillItem();
        
        DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        
        from.GetAnimator().SetStateAsync(1);
        
        AudioManager.Play("CardPlacement");

        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    private void MergeStaging(MergeDetails d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergeTargetAsync(2, null);
        
        SlotView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as SlotView;
        SlotView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as SlotView;
        
        to.SetMoveFromRectToIdle(from.GetContentView().GetRect());
        from.GetAnimator().SetStateAsync(1);
        
        DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        if (d.FromDeckIndex.Index > d.ToDeckIndex.Index)
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index);
        else
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index - 1);
        
        AudioManager.Play("CardUpgrade");
    
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    public void BuySkillStaging(BuySkillDetails d)
    {
        void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
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
        
        SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as SlotView;
        SlotView commodityView = ShopPanel.CommodityItemFromIndex(d.CommodityIndex) as SlotView;
        
        ShopPanel.ListView.RemoveItemAt(d.CommodityIndex);
        
        SetPosition(view, commodityView.GetRect().position, commodityView.GetRect().localScale);
        SetIdle(view);
    }

    public void ExchangeSkillStaging(ExchangeSkillDetails d)
    {
        void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
            view.GetContentView().GetRect().position = position;
            view.GetContentView().GetRect().localScale = localScale;
        }
        
        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Play("CardPlacement");
        // AudioManager.Instance.Play("钱币");
        
        SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as SlotView;
        SlotView barterItemView = BarterPanel.BarterItemFromIndex(d.BarterItemIndex) as SlotView;
        
        DeckPanel.HandView.Modified(d.DeckIndex.Index);
        BarterPanel.ListView.RemoveItemAt(d.BarterItemIndex);
        BarterPanel.ListView.Sync();
        
        SetPosition(view, barterItemView.GetRect().position, barterItemView.GetRect().localScale);
        SetIdle(view);
    }

    public void GachaStaging(GachaDetails d)
    {
        void SetPosition(SlotView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
            view.GetContentView().GetRect().position = position;
            view.GetContentView().GetRect().localScale = localScale;
        }
        
        void SetIdle(SlotView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Instance.Play("钱币");
        
        DeckPanel.HandView.AddItem();
        
        SlotView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as SlotView;
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
