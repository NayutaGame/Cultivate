
using System.Linq;
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
        // TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    private void OnEnable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Add(GainSkillStaging);
        RunManager.Instance.Environment.GainSkillsNeuron.Add(GainSkillsStaging);
        RunManager.Instance.Environment.LoseMingYuanNeuron.Add(MingYuanDamageStaging);
    }

    private void OnDisable()
    {
        RunManager.Instance.Environment.GainSkillNeuron.Remove(GainSkillStaging);
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

    #region Staging

    private void GainSkillStaging(GainSkillDetails d)
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
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(InteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        Vector3 position = Vector3.zero;

        InteractBehaviour ib = SkillInteractBehaviourFromDeckIndex(d.DeckIndex);
        
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
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = Vector3.zero;
                extraBehaviourPivot.Animator.SetState(3);
                newIB.SetInteractable(false);
            }
        }

        void SetSkillShow(DeckIndex deckIndex, Vector3 position)
        {
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.FollowTransform.position = position;
                extraBehaviourPivot.FollowTransform.localScale = extraBehaviourPivot.HoverTransform.localScale;
                extraBehaviourPivot.Animator.SetStateAsync(3);
                newIB.SetInteractable(false);
            }
        }

        void SetSkillMove(DeckIndex deckIndex, Vector3 position)
        {
            InteractBehaviour newIB = SkillInteractBehaviourFromDeckIndex(deckIndex);
            newIB.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
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

    public void PickDiscoveredSkillStaging(InteractBehaviour cardIB, InteractBehaviour discoverIB)
    {
        void SetSkillPosition(InteractBehaviour ib, InteractBehaviour discoverIB)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                Transform t = discoverIB.GetSimpleView().transform;
                extraBehaviourPivot.FollowTransform.position = t.position;
                extraBehaviourPivot.FollowTransform.localScale = t.localScale;
                extraBehaviourPivot.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(InteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, discoverIB);
        SetSkillMove(cardIB, discoverIB.transform.position);
    }

    public void BuySkillStaging(InteractBehaviour cardIB, InteractBehaviour commodityIB)
    {
        void SetSkillPosition(InteractBehaviour ib, InteractBehaviour commodityIB)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                Transform t = commodityIB.GetSimpleView().transform;
                extraBehaviourPivot.FollowTransform.position = t.position;
                extraBehaviourPivot.FollowTransform.localScale = t.localScale;
                extraBehaviourPivot.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(InteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, commodityIB);
        SetSkillMove(cardIB, commodityIB.transform.position);
        // AudioManager.Instance.Play("钱币");
    }

    public void GachaStaging(InteractBehaviour cardIB, InteractBehaviour gachaIB)
    {
        void SetSkillPosition(InteractBehaviour ib, InteractBehaviour gachaIB)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                Transform t = gachaIB.GetSimpleView().transform;
                extraBehaviourPivot.FollowTransform.position = t.position;
                extraBehaviourPivot.FollowTransform.localScale = t.localScale;
                extraBehaviourPivot.Animator.SetState(3);
                ib.SetInteractable(false);
            }
        }

        void SetSkillMove(InteractBehaviour ib, Vector3 position)
        {
            ib.SetInteractable(true);
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
            {
                extraBehaviourPivot.PositionToIdle(position);
            }
            // AudioManager.Play("CardPlacement");
        }
        
        // Refresh();
        
        SetSkillPosition(cardIB, gachaIB);
        SetSkillMove(cardIB, gachaIB.transform.position);
        // AudioManager.Instance.Play("钱币");
    }

    public InteractBehaviour SkillInteractBehaviourFromDeckIndex(DeckIndex? deckIndex)
    {
        if (!deckIndex.HasValue)
            return DeckPanel.HandView.ActivePool.Last().GetInteractBehaviour();
        
        if (deckIndex.Value.InField)
            return DeckPanel.PlayerEntity.SkillList.ActivePool[deckIndex.Value.Index].GetInteractBehaviour();

        return DeckPanel.HandView.ActivePool[deckIndex.Value.Index].GetInteractBehaviour();
    }

    public void MingYuanDamageStaging(int value)
    {
        CanvasManager.Instance.RedFlashAnimation();
        CanvasManager.Instance.CanvasShakeAnimation();
        CanvasManager.Instance.UIFloatTextVFX(value.ToString(), Color.red);
    }

    #endregion
}
