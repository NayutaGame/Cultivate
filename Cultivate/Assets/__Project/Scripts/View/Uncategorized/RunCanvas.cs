
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

        // if (!Application.isEditor)
        //     ConsolePanel.gameObject.SetActive(false);
    }

    public override void Refresh()
    {
        RefreshPanel();
    }

    private void RefreshPanel()
    {
        ChangePanel(RunManager.Instance.Environment.GetActivePanel());
    }

    public void LayoutRebuild()
    {
        DiscoverSkillPanel.LayoutRebuild();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Add(GainSkillStaging);
        RunManager.Instance.Environment.RemoveSkillNeuron.Add(RemoveSkillStaging);
        RunManager.Instance.Environment.SkillSetJingJieNeuron.Add(SkillSetJingJieStaging);
        RunManager.Instance.Environment.ReplaceSkillNeuron.Add(ReplaceSkillStaging);
        
        EquipEvent.Add(RunManager.Instance.Environment.EquipProcedure);
        RunManager.Instance.Environment.EquipNeuron.Add(EquipStaging);
        
        SwapEvent.Add(RunManager.Instance.Environment.SwapProcedure);
        RunManager.Instance.Environment.SwapNeuron.Add(SwapStaging);
        
        UnequipEvent.Add(RunManager.Instance.Environment.UnequipProcedure);
        RunManager.Instance.Environment.UnequipNeuron.Add(UnequipStaging);
        
        MergeEvent.Add(RunManager.Instance.Environment.MergeProcedure);
        RunManager.Instance.Environment.MergeNeuron.Add(MergeStaging);
        
        RunManager.Instance.Environment.PanelChangedNeuron.Add(ChangePanel);
        
        RunManager.Instance.Environment.GainSkillsNeuron.Add(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(MingYuanDamageStaging);
        
        RefreshPanel();
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Remove(GainSkillStaging);
        RunManager.Instance.Environment.RemoveSkillNeuron.Remove(RemoveSkillStaging);
        RunManager.Instance.Environment.SkillSetJingJieNeuron.Remove(SkillSetJingJieStaging);
        RunManager.Instance.Environment.ReplaceSkillNeuron.Remove(ReplaceSkillStaging);
        
        EquipEvent.Remove(RunManager.Instance.Environment.EquipProcedure);
        RunManager.Instance.Environment.EquipNeuron.Remove(EquipStaging);
        
        SwapEvent.Remove(RunManager.Instance.Environment.SwapProcedure);
        RunManager.Instance.Environment.SwapNeuron.Remove(SwapStaging);
        
        UnequipEvent.Remove(RunManager.Instance.Environment.UnequipProcedure);
        RunManager.Instance.Environment.UnequipNeuron.Remove(UnequipStaging);
        
        MergeEvent.Remove(RunManager.Instance.Environment.MergeProcedure);
        RunManager.Instance.Environment.MergeNeuron.Remove(MergeStaging);
        
        RunManager.Instance.Environment.PanelChangedNeuron.Remove(ChangePanel);
        
        RunManager.Instance.Environment.GainSkillsNeuron.Remove(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(MingYuanDamageStaging);
    }

    private void ChangePanel(PanelChangedDetails d)
        => ChangePanelAsync(d);
    
    private void ChangePanel(PanelDescriptor toPanel)
        => ChangePanelAsync(toPanel);

    private async UniTask ChangePanelAsync(PanelChangedDetails panelChangedDetails)
        => await ChangePanelAsync(panelChangedDetails.ToPanel);
    
    private async UniTask ChangePanelAsync(PanelDescriptor toPanel)
    {
        if (toPanel is not null && toPanel is not RunResultPanelDescriptor)
            AppManager.Instance.ProfileManager.Save();
        
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
        // else
        //     await GetAnimator().SetStateAsync(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].CheckAwake();
            PanelSM[newState].Refresh();
            await PanelSM[newState].GetAnimator().SetStateAsync(1);
        }
        // else
        //     await GetAnimator().SetStateAsync(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        bool showDeck = d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor ||
                        d is DiscoverSkillPanelDescriptor;
        
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

    public Neuron<EquipDetails> EquipEvent = new();
    public Neuron<SwapDetails> SwapEvent = new();
    public Neuron<UnequipDetails> UnequipEvent = new();
    public Neuron<MergeDetails> MergeEvent = new();

    #region Staging

    public override Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true));

    private void GainSkillStaging(GainSkillDetails d)
    {
        void SetPosition(DelegatingView view, Vector3 position)
        {
            view.GetAnimator().SetState(4);
            view.GetDelegatedView().GetRect().position = position;
            view.GetDelegatedView().GetRect().localScale = Vector3.zero;
        }

        void SetShow(DelegatingView view)
        {
            view.GetAnimator().SetTweenAsync(view.GetDelegatedView().GetRect().DOScale(1, 0.15f));
        }

        void SetIdle(DelegatingView view)
        {
            view.GetAnimator().SetStateAsync(1);
            // AudioManager.Play("CardPlacement");
        }

        if (d.DeckIndex.InField)
        {
            DeckPanel.PlayerEntity.FieldView.Modified(d.DeckIndex.Index);
        }
        else
        {
            DeckPanel.HandView.InsertItem(d.DeckIndex.Index);
        }

        DelegatingView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as DelegatingView;
        Vector3 position = Vector3.zero;
        
        SetPosition(view, position);

        Sequence seq = DOTween.Sequence()
            .AppendInterval(0.05f)
            .AppendCallback(() => SetShow(view))
            .AppendInterval(0.3f)
            .AppendCallback(() => SetIdle(view));
        
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
    
    private void GainSkillsStaging(GainSkillsDetails d)
    {
        void SetPosition(DelegatingView view, Vector3 position)
        {
            view.GetAnimator().SetState(4);
            view.GetDelegatedView().GetRect().position = position;
            view.GetDelegatedView().GetRect().localScale = Vector3.zero;
        }
        
        void SetShow(DelegatingView view)
        {
            view.GetAnimator().SetTweenAsync(view.GetDelegatedView().GetRect().DOScale(1, 0.15f));
        }
        
        void SetIdle(DelegatingView view)
        {
            view.GetAnimator().SetStateAsync(1);
            // AudioManager.Play("CardPlacement");
        }
        
        d.DeckIndices.Length.Do(i => DeckPanel.HandView.AddItem());
        
        Vector3 position = Vector3.zero;
        int offset = 1;

        for (int i = 0; i < d.DeckIndices.Length; i++)
        {
            DelegatingView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndices[i]) as DelegatingView;
            Vector3 showPosition = position + i * offset * Vector3.left;
            SetPosition(view, showPosition);
        }
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);

        foreach (DeckIndex deckIndex in d.DeckIndices)
        {
            DelegatingView view = DeckPanel.SkillItemFromDeckIndex(deckIndex) as DelegatingView;
            seq.AppendCallback(() => SetShow(view))
                .AppendInterval(0.1f);
        }
        
        seq.AppendInterval(0.2f);

        foreach (DeckIndex deckIndex in d.DeckIndices)
        {
            DelegatingView view = DeckPanel.SkillItemFromDeckIndex(deckIndex) as DelegatingView;
            seq.AppendCallback(() => SetIdle(view))
                .AppendInterval(0.1f);
        }

        _animationQueue.QueueAnimation(seq);
    }

    private void EquipStaging(EquipDetails d)
    {
        if (d.IsReplace)
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.HandView.Modified(d.FromDeckIndex.Index);
        }
        else
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
            
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
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
            
            from.SetMoveFromRectToIdle(to.GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    private void UnequipStaging(UnequipDetails d)
    {
        DeckPanel.HandView.AddItem();
        
        DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
        DelegatingView to = DeckPanel.LatestSkillItem() as DelegatingView;
        
        DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
        
        to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
        
        from.GetAnimator().SetStateAsync(1);
        
        AudioManager.Play("CardPlacement");

        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
    }

    private void MergeStaging(MergeDetails d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergePreresultAsync(2, null);
        
        DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
        DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
        
        to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
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
        void SetPosition(DelegatingView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
            view.GetDelegatedView().GetRect().position = position;
            view.GetDelegatedView().GetRect().localScale = localScale;
        }
        
        void SetIdle(DelegatingView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Play("CardPlacement");
        // AudioManager.Instance.Play("钱币");
        
        DeckPanel.HandView.AddItem();
        
        DelegatingView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as DelegatingView;
        DelegatingView commodityView = ShopPanel.CommodityItemFromIndex(d.CommodityIndex) as DelegatingView;
        
        ShopPanel.CommodityListView.RemoveItemAt(d.CommodityIndex);
        
        SetPosition(view, commodityView.GetRect().position, commodityView.GetRect().localScale);
        SetIdle(view);
    }

    public void ExchangeSkillStaging(ExchangeSkillDetails d)
    {
        void SetPosition(DelegatingView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
            view.GetDelegatedView().GetRect().position = position;
            view.GetDelegatedView().GetRect().localScale = localScale;
        }
        
        void SetIdle(DelegatingView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Play("CardPlacement");
        
        DelegatingView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as DelegatingView;
        DelegatingView barterItemView = BarterPanel.BarterItemFromIndex(d.BarterItemIndex) as DelegatingView;
        
        DeckPanel.HandView.Modified(d.DeckIndex.Index);
        BarterPanel.BarterItemListView.RemoveItemAt(d.BarterItemIndex);
        
        SetPosition(view, barterItemView.GetRect().position, barterItemView.GetRect().localScale);
        SetIdle(view);
    }

    public void GachaStaging(GachaDetails d)
    {
        void SetPosition(DelegatingView view, Vector3 position, Vector3 localScale)
        {
            view.GetAnimator().SetState(4);
            view.GetDelegatedView().GetRect().position = position;
            view.GetDelegatedView().GetRect().localScale = localScale;
        }
        
        void SetIdle(DelegatingView view)
        {
            view.GetAnimator().SetStateAsync(1);
        }
        
        // AudioManager.Instance.Play("钱币");
        
        DeckPanel.HandView.AddItem();
        
        DelegatingView view = DeckPanel.SkillItemFromDeckIndex(d.DeckIndex) as DelegatingView;
        DelegatingView gachaItemView = GachaPanel.GachaItemFromIndex(d.GachaIndex) as DelegatingView;
        
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
