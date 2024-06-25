
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
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
    public ArbitraryCardPickerPanel ArbitraryCardPickerPanel;
    public ImagePanel ImagePanel;
    public RunResultPanel RunResultPanel;

    public override void Configure()
    {
        base.Configure();

        PanelSM = new(new Panel[]
        {
            null,
            MapPanel,
            BattlePanel,
            PuzzlePanel,
            DialogPanel,
            DiscoverSkillPanel,
            CardPickerPanel,
            ShopPanel,
            BarterPanel,
            ArbitraryCardPickerPanel,
            ImagePanel,
            RunResultPanel,
        });

        // _panelDict.Do(kvp => kvp.Value.Configure());

        DeckPanel.Configure();
        MapPanel.Configure();

        MapButton.onClick.RemoveAllListeners();
        MapButton.onClick.AddListener(() => MapPanel.ToggleShowing());

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
        // MapPanel.Refresh();
        ReservedLayer.Refresh();
        TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    public async Task SetPanelSAsync(PanelS panelS)
    {
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                await PanelSM.GetCurrPanel().SetStateAsync(1);
            return;
        }

        if (PanelSM[oldState] != null)
            await PanelSM[oldState].SetStateAsync(0);
        else
            await SetStateAsync(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].Configure();
            PanelSM[newState].Refresh();
            await PanelSM[newState].SetStateAsync(1);
        }
        else
            await SetStateAsync(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            await DeckPanel.SetStateAsync(2);
        }
        else
        {
            await DeckPanel.SetStateAsync(0);
        }
    }

    public void SetPanelS(PanelS panelS)
    {
        PanelS oldState = PanelSM.State;
        PanelS newState = panelS;

        if (oldState.Equals(newState))
        {
            if (PanelSM.GetCurrPanel() != null)
                PanelSM.GetCurrPanel().SetState(1);
            return;
        }

        if (PanelSM[oldState] != null)
            PanelSM[oldState].SetState(0);
        else
            SetState(1);

        PanelSM.SetState(newState);

        if (PanelSM[newState] != null)
        {
            PanelSM[newState].Configure();
            PanelSM[newState].Refresh();
            PanelSM[newState].SetState(1);
        }
        else
            SetState(0);

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();
        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            DeckPanel.SetState(2);
        }
        else
        {
            DeckPanel.SetState(0);
        }
    }

    #region Staging

    public void GainSkillProcedure(Vector3 position, SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
    {
        RunManager.Instance.Environment.DrawSkillProcedure(descriptor, preferredDeckIndex);
        Refresh();
        
        InteractBehaviour newIB = DeckPanel.HandView.ActivePool.Last().GetInteractBehaviour();
        ExtraBehaviourPivot extraBehaviourPivot = newIB.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
        {
            extraBehaviourPivot.FollowTransform.position = position;
            extraBehaviourPivot.SetPathAnimated(extraBehaviourPivot.FollowTransform, extraBehaviourPivot.IdleTransform);
        }

        // AudioManager.Play("CardPlacement");
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
