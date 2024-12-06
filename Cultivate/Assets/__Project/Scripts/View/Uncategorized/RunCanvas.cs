
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
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
    
    public override void AwakeFunction()
    {
        base.AwakeFunction();

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

        ReservedLayer.Configure();
        TopBar.Configure();

        ConsolePanel.CheckAwake();

        if (!Application.isEditor)
            ConsolePanel.gameObject.SetActive(false);
    }

    public override void Refresh()
    {
        base.Refresh();

        Panel currentPanel = PanelSM.GetCurrPanel();
        if (currentPanel != null)
        {
            currentPanel.AwakeFunction();
            currentPanel.Refresh();
        }

        DeckPanel.Refresh();
        MapPanel.Refresh();
        ReservedLayer.Refresh();
        // TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    public void LayoutRebuild()
    {
        Panel currentPanel = PanelSM.GetCurrPanel();
        if (currentPanel == null)
            return;

        if (currentPanel is DiscoverSkillPanel p)
        {
            p.LayoutRebuild();
        }
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
        
        RunManager.Instance.Environment.PanelChangedNeuron.Add(ChangePanel);
        
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
        
        RunManager.Instance.Environment.PanelChangedNeuron.Remove(ChangePanel);
        
        RunManager.Instance.Environment.LegacyGainSkillNeuron.Remove(GainSkillStaging);
        RunManager.Instance.Environment.GainSkillsNeuron.Remove(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Remove(MingYuanDamageStaging);
    }


    #region AnimationQueue

    private Queue<Tween> _animationQueue;

    public void CompleteAnimationQueue()
    {
        while (_animationQueue.Count > 0)
        {
            Tween t = _animationQueue.Peek();
            t.Complete();
            _animationQueue.Dequeue();
        }
    }

    public async UniTask WaitForQueueToComplete()
    {
        while (_animationQueue.Count > 0)
            await _animationQueue.Peek().AsyncWaitForCompletion();
    }

    public async UniTask TryProcessAnimationQueue()
    {
        if (_animationQueue.Count != 0)
            return;
        
        while (_animationQueue.Count > 0)
        {
            Tween t = _animationQueue.Peek();
            t.SetAutoKill().Restart();
            await t.AsyncWaitForCompletion();
            _animationQueue.Dequeue();
        }
    }

    public void QueueAnimation(Tween tween)
    {
        _animationQueue.Enqueue(tween);
        TryProcessAnimationQueue();
    }

    #endregion
    
    public void ChangePanel(PanelChangedDetails d)
        => ChangePanelAsync(d);
    
    public async UniTask ChangePanelAsync(PanelChangedDetails panelChangedDetails)
    {
        await WaitForQueueToComplete();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = PanelS.FromPanelDescriptor(panelChangedDetails.ToPanel);
        
        MapPanel.Refresh();

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                await PanelSM.GetCurrPanel().GetAnimator().SetStateAsync(1);
            return;
        }

        if (PanelSM[oldState] != null)
            await PanelSM[oldState].GetAnimator().SetStateAsync(0);
        else
            await GetAnimator().SetStateAsync(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].AwakeFunction();
            PanelSM[newState].Refresh();
            await PanelSM[newState].GetAnimator().SetStateAsync(1);
        }
        else
            await GetAnimator().SetStateAsync(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            await DeckPanel.GetAnimator().SetStateAsync(2);
        }
        else
        {
            await DeckPanel.GetAnimator().SetStateAsync(0);
        }
    }

    public async UniTask LegacySetPanelSAsyncFromSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.LegacyReceiveSignalProcedure(signal);
        // if (RunManager.Instance.Environment == null)
        //     return;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        await LegacySetPanelSAsync(panelS);
    }

    public async UniTask LegacySetPanelSAsync(PanelS panelS)
    {
        await WaitForQueueToComplete();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;
        
        MapPanel.Refresh();

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                await PanelSM.GetCurrPanel().GetAnimator().SetStateAsync(1);
            return;
        }

        if (PanelSM[oldState] != null)
            await PanelSM[oldState].GetAnimator().SetStateAsync(0);
        else
            await GetAnimator().SetStateAsync(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].AwakeFunction();
            PanelSM[newState].Refresh();
            await PanelSM[newState].GetAnimator().SetStateAsync(1);
        }
        else
            await GetAnimator().SetStateAsync(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor || d is DiscoverSkillPanelDescriptor)
        {
            await DeckPanel.GetAnimator().SetStateAsync(2);
        }
        else
        {
            await DeckPanel.GetAnimator().SetStateAsync(0);
        }
    }

    public void LegacySetPanelSFromSignal(Signal signal)
    {
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.LegacyReceiveSignalProcedure(signal);
        // if (RunManager.Instance.Environment == null)
        //     return;
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        LegacySetPanelS(panelS);
    }

    public void LegacySetPanelS(PanelS panelS)
    {
        CompleteAnimationQueue();
        
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;
        
        MapPanel.Refresh();

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                PanelSM.GetCurrPanel().GetAnimator().SetState(1);
            return;
        }
        
        if (PanelSM[oldState] != null)
            PanelSM[oldState].GetAnimator().SetState(0);
        else
            GetAnimator().SetState(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].CheckAwake();
            PanelSM[newState].Refresh();
            PanelSM[newState].GetAnimator().SetState(1);
        }
        else
            GetAnimator().SetState(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor || d is DiscoverSkillPanelDescriptor)
        {
            DeckPanel.GetAnimator().SetState(2);
        }
        else
        {
            DeckPanel.GetAnimator().SetState(0);
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
        
        QueueAnimation(seq);
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
        // CanvasManager.Instance.RunCanvas.Refresh();
    }

    private void UnequipStaging(UnequipDetails d)
    {
        // env.ReceiveSignalProcedure(new FieldChangedSignal(slot.ToDeckIndex(), DeckIndex.FromHand()));
        
        DeckPanel.HandView.AddItem();
        
        DelegatingView from = DeckPanel.SkillItemFromDeckIndex(d.FromDeckIndex) as DelegatingView;
        DelegatingView to = DeckPanel.LatestSkillItem() as DelegatingView;
        
        DeckPanel.PlayerEntity.FieldView.Modified(d.FromDeckIndex.Index);
        
        to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
        
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
        
        to.SetMoveFromRectToIdle(from.GetDelegatedView().GetRect());
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

        QueueAnimation(seq);
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

        QueueAnimation(seq);
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
