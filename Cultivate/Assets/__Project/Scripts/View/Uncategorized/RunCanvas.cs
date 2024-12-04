
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
    public ReservedLayer ReservedLayer;
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

    private Tween _blockingAnimation;

    public override void Configure()
    {
        base.Configure();

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

        DeckPanel.Configure();
        MapPanel.Configure();

        ReservedLayer.Configure();
        TopBar.Configure();

        ConsolePanel.Configure();

        if (!Application.isEditor)
            ConsolePanel.gameObject.SetActive(false);
    }

    public override void Refresh()
    {
        base.Refresh();

        Panel currentPanel = PanelSM.GetCurrPanel();
        if (currentPanel != null)
        {
            currentPanel.Configure();
            currentPanel.Refresh();
        }

        DeckPanel.Refresh();
        MapPanel.Refresh();
        ReservedLayer.Refresh();
        // TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.AddSkillNeuron.Add(AddSkillStaging);
        
        EquipEvent.Add(RunManager.Instance.Environment.EquipProcedure);
        RunManager.Instance.Environment.EquipNeuron.Add(EquipStaging);
        
        SwapEvent.Add(RunManager.Instance.Environment.SwapProcedure);
        RunManager.Instance.Environment.SwapNeuron.Add(SwapStaging);
        
        UnequipEvent.Add(RunManager.Instance.Environment.UnequipProcedure);
        RunManager.Instance.Environment.UnequipNeuron.Add(UnequipStaging);
        
        MergeEvent.Add(RunManager.Instance.Environment.MergeProcedure);
        RunManager.Instance.Environment.MergeNeuron.Add(MergeStaging);
        
        RunManager.Instance.Environment.LegacyGainSkillNeuron.Add(GainSkillStaging);
        RunManager.Instance.Environment.GainSkillsNeuron.Add(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(MingYuanDamageStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.AddSkillNeuron.Remove(AddSkillStaging);
        
        EquipEvent.Remove(RunManager.Instance.Environment.EquipProcedure);
        RunManager.Instance.Environment.EquipNeuron.Remove(EquipStaging);
        
        SwapEvent.Remove(RunManager.Instance.Environment.SwapProcedure);
        RunManager.Instance.Environment.SwapNeuron.Remove(SwapStaging);
        
        UnequipEvent.Remove(RunManager.Instance.Environment.UnequipProcedure);
        RunManager.Instance.Environment.UnequipNeuron.Remove(UnequipStaging);
        
        MergeEvent.Remove(RunManager.Instance.Environment.MergeProcedure);
        RunManager.Instance.Environment.MergeNeuron.Remove(MergeStaging);
        
        RunManager.Instance.Environment.LegacyGainSkillNeuron.Remove(GainSkillStaging);
        RunManager.Instance.Environment.GainSkillsNeuron.Remove(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(MingYuanDamageStaging);
    }

    public async UniTask SetPanelSAsyncFromSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(signal);
        // if (RunManager.Instance.Environment == null)
        //     return;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        await SetPanelSAsync(panelS);
    }

    private async UniTask SetPanelSAsync(PanelS panelS)
    {
        if (_blockingAnimation != null && _blockingAnimation.IsPlaying())
            await _blockingAnimation.AsyncWaitForCompletion();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;
        
        MapPanel.Refresh();

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                await PanelSM.GetCurrPanel().Animator.SetStateAsync(1);
            return;
        }

        if (PanelSM[oldState] != null)
            await PanelSM[oldState].Animator.SetStateAsync(0);
        else
            await Animator.SetStateAsync(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].Configure();
            PanelSM[newState].Refresh();
            await PanelSM[newState].Animator.SetStateAsync(1);
        }
        else
            await Animator.SetStateAsync(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            await DeckPanel.Animator.SetStateAsync(2);
        }
        else
        {
            await DeckPanel.Animator.SetStateAsync(0);
        }
    }

    public void SetPanelSFromSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(signal);
        // if (RunManager.Instance.Environment == null)
        //     return;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        SetPanelS(panelS);
    }

    public void SetPanelS(PanelS panelS)
    {
        _blockingAnimation.Complete();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;
        
        MapPanel.Refresh();

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                PanelSM.GetCurrPanel().Animator.SetState(1);
            return;
        }
        
        if (PanelSM[oldState] != null)
            PanelSM[oldState].Animator.SetState(0);
        else
            Animator.SetState(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].Configure();
            PanelSM[newState].Refresh();
            PanelSM[newState].Animator.SetState(1);
        }
        else
            Animator.SetState(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            DeckPanel.Animator.SetState(2);
        }
        else
        {
            DeckPanel.Animator.SetState(0);
        }
    }

    public Neuron<EquipDetails> EquipEvent = new();
    public Neuron<SwapDetails> SwapEvent = new();
    public Neuron<UnequipDetails> UnequipEvent = new();
    public Neuron<MergeDetails> MergeEvent = new();

    #region Staging

    private void AddSkillStaging(AddSkillDetails d)
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

        seq.SetAutoKill().Restart();

        _blockingAnimation = seq;
    }

    private void EquipStaging(EquipDetails d)
    {
        // env.ReceiveSignalProcedure(new FieldChangedSignal(DeckIndex.FromHand(), slot.ToDeckIndex()));
        // FieldChangeNeuron O-- Refresh All Field
        
        if (d.IsReplace)
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
            to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
            to.GetAnimator().SetStateAsync(1);
        
            from.GetDelegatedView().GetRect().position = to.GetRect().position;
            from.GetDelegatedView().GetRect().localScale = to.GetRect().localScale;
            from.GetAnimator().SetStateAsync(1);
            
            DeckPanel.HandView.Modified(d.FromDeckIndex.Index);
        }
        else
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
            to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
            to.GetAnimator().SetStateAsync(1);
            
            from.GetAnimator().SetStateAsync(1);
            
            DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void SwapStaging(SwapDetails d)
    {
        // env.ReceiveSignalProcedure(new FieldChangedSignal(fromSlot.ToDeckIndex(), toSlot.ToDeckIndex()));
        
        if (d.IsReplace)
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
            to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
            to.GetAnimator().SetStateAsync(1);
        
            from.GetDelegatedView().GetRect().position = to.GetRect().position;
            from.GetDelegatedView().GetRect().localScale = to.GetRect().localScale;
            from.GetAnimator().SetStateAsync(1);
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        else
        {
            DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
            DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
            to.Refresh();
            to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
            to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
            to.GetAnimator().SetStateAsync(1);
            
            DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
            DeckPanel.PlayerEntity.FieldView.Modified(d.ToDeckIndex.Index);
        }
        
        AudioManager.Play("CardPlacement");
        
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void UnequipStaging(UnequipDetails d)
    {
        // env.ReceiveSignalProcedure(new FieldChangedSignal(slot.ToDeckIndex(), DeckIndex.FromHand()));
        
        DeckPanel.HandView.AddItem();
        
        DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
        DelegatingView to = DeckPanel.LatestSkillItem() as DelegatingView;
        
        DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
        
        to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
        to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
        to.GetAnimator().SetStateAsync(1);
        from.GetAnimator().SetStateAsync(1);
        
        AudioManager.Play("CardPlacement");

        // PlayerEntity.Refresh();
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void MergeStaging(MergeDetails d)
    {
        CanvasManager.Instance.MergePreresultView.SetMergePreresultAsync(2, null);
        
        // RunManager.Instance.Environment.ReceiveSignalProcedure(new FieldChangedSignal(DeckIndex.FromHand(), DeckIndex.FromHand()));
        
        DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
        DelegatingView to = DeckPanel.SkillItemFromDeckIndex(d.ToDeckIndex) as DelegatingView;
        
        to.GetDelegatedView().GetRect().position = from.GetDelegatedView().GetRect().position;
        to.GetDelegatedView().GetRect().localScale = from.GetDelegatedView().GetRect().localScale;
        to.GetAnimator().SetStateAsync(1);
        from.GetAnimator().SetStateAsync(1);
        
        DeckPanel.HandView.RemoveItemAt(d.FromDeckIndex.Index);
        if (d.FromDeckIndex.Index > d.ToDeckIndex.Index)
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index);
        else
            DeckPanel.HandView.Modified(d.ToDeckIndex.Index - 1);
        
        AudioManager.Play("CardUpgrade");
    
        // CanvasManager.Instance.RunCanvas.CardPickerPanel.ClearAllSelections();
        // CanvasManager.Instance.RunCanvas.Refresh();
    }
    
    
    
    
    
    
    
    private void GainSkillStaging(GainSkillDetails d)
    {
        void SetSkillPosition(LegacyInteractBehaviour ib, Vector3 position)
        {
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.FollowTransform.position = position;
                pivotBehaviour.FollowTransform.localScale = Vector3.zero;
                pivotBehaviour.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillShow(LegacyInteractBehaviour ib, Vector3 position)
        {
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.FollowTransform.position = position;
                pivotBehaviour.FollowTransform.localScale = pivotBehaviour.HoverTransform.localScale;
                pivotBehaviour.Animator.SetStateAsync(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(LegacyInteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        Vector3 position = Vector3.zero;

        LegacyInteractBehaviour ib = SkillInteractBehaviourFromDeckIndex(d.DeckIndex);
        
        SetSkillPosition(ib, position);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);

        seq.AppendCallback(() => SetSkillShow(ib, position))
            .AppendInterval(0.1f);
        
        seq.AppendInterval(0.2f);
        
        seq.AppendCallback(() => SetSkillMove(ib, position))
            .AppendInterval(0.1f);

        seq.SetAutoKill().Restart();

        _blockingAnimation = seq;
    }

    private void GainSkillsStaging(GainSkillsDetails d)
    {
        void SetSkillPosition(DeckIndex deckIndex, Vector3 position)
        {
            LegacyInteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            LegacyPivotBehaviour pivotBehaviour = newIB.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.FollowTransform.position = position;
                pivotBehaviour.FollowTransform.localScale = Vector3.zero;
                pivotBehaviour.Animator.SetState(3);
                newIB.SetInteractable(false);
            }
        }

        void SetSkillShow(DeckIndex deckIndex, Vector3 position)
        {
            LegacyInteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            LegacyPivotBehaviour pivotBehaviour = newIB.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.FollowTransform.position = position;
                pivotBehaviour.FollowTransform.localScale = pivotBehaviour.HoverTransform.localScale;
                pivotBehaviour.Animator.SetStateAsync(3);
                newIB.SetInteractable(false);
            }
        }

        void SetSkillMove(DeckIndex deckIndex, Vector3 position)
        {
            LegacyInteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            newIB.SetInteractable(true);
            LegacyPivotBehaviour pivotBehaviour = newIB.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        Vector3 position = Vector3.zero;
        
        int offset = 1;

        for (int i = 0; i < d.DeckIndices.Length; i++)
        {
            int index = i;
            SetSkillPosition(d.DeckIndices[index], position + index * offset * Vector3.left);
        }
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);

        for (int i = 0; i < d.DeckIndices.Length; i++)
        {
            int index = i;
            seq.AppendCallback(() => SetSkillShow(d.DeckIndices[index], position + index * offset * Vector3.left))
                .AppendInterval(0.1f);
        }
        
        seq.AppendInterval(0.2f);

        for (int i = 0; i < d.DeckIndices.Length; i++)
        {
            int index = i;
            seq.AppendCallback(() => SetSkillMove(d.DeckIndices[index], position + index * offset * Vector3.left))
                .AppendInterval(0.1f);
        }

        seq.SetAutoKill().Restart();

        _blockingAnimation = seq;
    }

    public void PickDiscoveredSkillStaging(LegacyInteractBehaviour cardIB, LegacyInteractBehaviour discoverIB)
    {
        void SetSkillPosition(LegacyInteractBehaviour ib, LegacyInteractBehaviour discoverIB)
        {
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                Transform t = discoverIB.GetSimpleView().transform;
                pivotBehaviour.FollowTransform.position = t.position;
                pivotBehaviour.FollowTransform.localScale = t.localScale;
                pivotBehaviour.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(LegacyInteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, discoverIB);
        SetSkillMove(cardIB, discoverIB.transform.position);
    }

    public void BuySkillStaging(LegacyInteractBehaviour cardIB, LegacyInteractBehaviour commodityIB)
    {
        void SetSkillPosition(LegacyInteractBehaviour ib, LegacyInteractBehaviour commodityIB)
        {
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                Transform t = commodityIB.GetSimpleView().transform;
                pivotBehaviour.FollowTransform.position = t.position;
                pivotBehaviour.FollowTransform.localScale = t.localScale;
                pivotBehaviour.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(LegacyInteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, commodityIB);
        SetSkillMove(cardIB, commodityIB.transform.position);
        // AudioManager.Instance.Play("钱币");
    }

    public void GachaStaging(LegacyInteractBehaviour cardIB, LegacyInteractBehaviour gachaIB)
    {
        void SetSkillPosition(LegacyInteractBehaviour ib, LegacyInteractBehaviour gachaIB)
        {
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                Transform t = gachaIB.GetSimpleView().transform;
                pivotBehaviour.FollowTransform.position = t.position;
                pivotBehaviour.FollowTransform.localScale = t.localScale;
                pivotBehaviour.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(LegacyInteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            LegacyPivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
            {
                pivotBehaviour.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, gachaIB);
        SetSkillMove(cardIB, gachaIB.transform.position);
        // AudioManager.Instance.Play("钱币");
    }

    public LegacyInteractBehaviour SkillInteractBehaviourFromDeckIndex(DeckIndex? deckIndex)
    {
        if (!deckIndex.HasValue)
            return LatestSkillInteractBehaviour();
        
        return DeckPanel.LegacySkillItemFromDeckIndex(deckIndex.Value).GetInteractBehaviour();
    }

    public LegacyInteractBehaviour LatestSkillInteractBehaviour()
        => DeckPanel.LegacyLatestSkillItem().GetInteractBehaviour();

    public void MingYuanDamageStaging(int value)
    {
        CanvasManager.Instance.RedFlashAnimation();
        CanvasManager.Instance.CanvasShakeAnimation();
        CanvasManager.Instance.UIFloatTextVFX(value.ToString(), Color.red);
    }

    #endregion
}
