
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
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
    public RunResultPanel RunResultPanel;

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
            RunResultPanel,
        });

        // _panelDict.Do(kvp => kvp.Value.Configure());

        DeckPanel.Configure();
        MapPanel.Configure();

        ReservedLayer.Configure();
        TopBar.Configure();

        ConsolePanel.Configure();

        // if (!Application.isEditor)
        //     ConsolePanel.gameObject.SetActive(false);
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
        TopBar.Refresh();
        ConsolePanel.Refresh();
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

    public void SetPanelS(PanelS panelS)
    {
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

    #region Staging
    
    public void DrawSkillsProcedure(SkillEntryCollectionDescriptor descriptor, Vector3? preferredPosition = null)
    {
        int startCount = DeckPanel.HandView.ActivePool.Count;

        preferredPosition ??= Vector3.zero;
        
        RunManager.Instance.Environment.DrawSkillsProcedure(descriptor);
        Refresh();

        DrawSkillsStaging(startCount, descriptor.Count, preferredPosition.Value);
    }

    private void DrawSkillsStaging(int startCount, int count, Vector3 position)
    {
        void SetSkillPosition(int handIndex, Vector3 position)
        {
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(DeckIndex.FromHand(handIndex));
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = Vector3.zero;
                extraBehaviourPivot.Animator.SetState(3);
                newIB.SetInteractable(false);
            }
        }

        void SetSkillShow(int handIndex, Vector3 position)
        {
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(DeckIndex.FromHand(handIndex));
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = extraBehaviourPivot.HoverTransform.localScale;
                extraBehaviourPivot.Animator.SetStateAsync(3);
            }
        }

        void GainSkillStaging(int handIndex, Vector3 position)
        {
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(DeckIndex.FromHand(handIndex));
            newIB.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        int offset = 1;
        
        for (int i = 0; i < count; i++)
        {
            int handIndex = i;
            SetSkillPosition(startCount + handIndex, position + handIndex * offset * Vector3.left);
        }
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);

        for (int i = 0; i < count; i++)
        {
            int handIndex = i;
            seq.AppendCallback(() => SetSkillShow(startCount + handIndex, position + handIndex * offset * Vector3.left))
                .AppendInterval(0.1f);
        }
        
        seq.AppendInterval(0.2f);
        
        for (int i = 0; i < count; i++)
        {
            int handIndex = i;
            seq.AppendCallback(() => GainSkillStaging(startCount + handIndex, position + handIndex * offset * Vector3.left))
                .AppendInterval(0.1f);
        }

        seq.SetAutoKill().Restart();
    }

    public void DrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null, Vector3? preferredPosition = null)
    {
        preferredPosition ??= Vector3.zero;
        
        RunManager.Instance.Environment.DrawSkillProcedure(descriptor, preferredDeckIndex);
        Refresh();
        
        DrawSkillStaging(SkillInteractBehaviourFromDeckIndex(preferredDeckIndex), preferredPosition.Value);
    }

    private void DrawSkillStaging(InteractBehaviour ib, Vector3 position)
    {
        void SetSkillPosition(InteractBehaviour ib, Vector3 position)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = Vector3.zero;
                extraBehaviourPivot.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillShow(InteractBehaviour ib, Vector3 position)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = extraBehaviourPivot.HoverTransform.localScale;
                extraBehaviourPivot.Animator.SetStateAsync(3);
            }
        }

        void GainSkillStaging(InteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        SetSkillPosition(ib, position);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.05f);

        seq.AppendCallback(() => SetSkillShow(ib, position))
            .AppendInterval(0.1f);
        
        seq.AppendInterval(0.2f);
        
        seq.AppendCallback(() => GainSkillStaging(ib, position))
            .AppendInterval(0.1f);

        seq.SetAutoKill().Restart();
    }

    public void AddSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null, Vector3? preferredPosition = null)
    {
        preferredPosition ??= Vector3.zero;
        
        RunManager.Instance.Environment.AddSkillProcedure(skillEntry, preferredJingJie, preferredDeckIndex);
        Refresh();
        
        DrawSkillStaging(SkillInteractBehaviourFromDeckIndex(preferredDeckIndex), preferredPosition.Value);
    }

    public void PickDiscoveredSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null, Vector3? preferredPosition = null)
    {
        preferredPosition ??= Vector3.zero;
        
        RunManager.Instance.Environment.AddSkillProcedure(skillEntry, preferredJingJie, preferredDeckIndex);
        Refresh();
        
        InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(preferredDeckIndex);
        ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
        {
            extraBehaviourPivot.PositionToIdle(preferredPosition.Value);
        }

        // AudioManager.Play("CardPlacement");
    }

    public InteractBehaviour SkillInteractBehaviourFromDeckIndex(DeckIndex? deckIndex)
    {
        if (!deckIndex.HasValue)
            return DeckPanel.HandView.ActivePool.Last().GetInteractBehaviour();
        
        if (deckIndex.Value.InField)
            return DeckPanel.PlayerEntity.SkillList.ActivePool[deckIndex.Value.Index].GetInteractBehaviour();

        return DeckPanel.HandView.ActivePool[deckIndex.Value.Index].GetInteractBehaviour();
    }
    
    // TODO: Audio
    public Neuron<int> GainMingYuanStagingNeuron = new();
    public Neuron<int> LoseMingYuanStagingNeuron = new();
    public Neuron<int> GainGoldStagingNeuron = new();
    public Neuron<int> LoseGoldStagingNeuron = new();
    public Neuron<int> GainDHealthStagingNeuron = new();
    public Neuron<int> LoseDHealthStagingNeuron = new();

    public void GainMingYuanProcedure(int value)
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(value);
        GainMingYuanStagingNeuron.Invoke(value);
    }

    public void LoseMingYuanProcedure(int value)
    {
        RunManager.Instance.Environment.SetDMingYuanProcedure(value);
        CanvasManager.Instance.RedFlashAnimation();
        CanvasManager.Instance.CanvasShakeAnimation();
        CanvasManager.Instance.UIFloatTextVFX(value.ToString(), Color.red);
        LoseMingYuanStagingNeuron.Invoke(value);
    }

    public void GainGoldProcedure(int value)
    {
        RunManager.Instance.Environment.SetDGoldProcedure(value);
        GainGoldStagingNeuron.Invoke(value);
    }

    public void LoseGoldProcedure(int value)
    {
        RunManager.Instance.Environment.SetDGoldProcedure(value);
        LoseGoldStagingNeuron.Invoke(value);
    }
    
    public void GainDHealthProcedure(int value)
    {
        RunManager.Instance.Environment.SetDDHealthProcedure(value);
        GainDHealthStagingNeuron.Invoke(value);
    }

    public void LoseDHealthProcedure(int value)
    {
        RunManager.Instance.Environment.SetDDHealthProcedure(value);
        LoseDHealthStagingNeuron.Invoke(value);
    }

    #endregion
}
