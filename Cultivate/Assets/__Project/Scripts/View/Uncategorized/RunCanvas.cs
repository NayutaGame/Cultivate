
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

    public void RegisterEnvironment(RunEnvironment env)
    {
        env.MingYuanChangedNeuron.Add(MingYuanChanged);
        LoseMingYuanStagingNeuron.Add(MingYuanLose);
        
        env.GoldChangedNeuron.Add(GoldChanged);
        
        env.DHealthChangedNeuron.Add(DHealthChanged);
    }

    public void UnregisterEnvironment(RunEnvironment env)
    {
        env.MingYuanChangedNeuron.Remove(MingYuanChanged);
        LoseMingYuanStagingNeuron.Remove(MingYuanLose);
        
        env.GoldChangedNeuron.Remove(GoldChanged);
        
        env.DHealthChangedNeuron.Remove(DHealthChanged);
    }

    private void MingYuanChanged(SetDMingYuanDetails d)
    {
        if (d.Value > 0)
            GainMingYuanStagingNeuron.Invoke(d);
        else if (d.Value < 0)
            LoseMingYuanStagingNeuron.Invoke(d);
    }

    public Neuron<SetDMingYuanDetails> GainMingYuanStagingNeuron = new(); // TODO: Audio
    public Neuron<SetDMingYuanDetails> LoseMingYuanStagingNeuron = new(); // TODO: Audio

    private void MingYuanLose(SetDMingYuanDetails d)
    {
        CanvasManager.Instance.RedFlashAnimation();
        CanvasManager.Instance.CanvasShakeAnimation();
        CanvasManager.Instance.UIFloatTextVFX(d.Value.ToString(), Color.red);
    }

    public Neuron<SetDGoldDetails> GainGoldStagingNeuron = new();
    public Neuron<SetDGoldDetails> LoseGoldStagingNeuron = new();
    public Neuron<SetDGoldDetails> ConsumeGoldStagingNeuron = new();

    private void GoldChanged(SetDGoldDetails d)
    {
        if (d.Value > 0)
            GainGoldStagingNeuron.Invoke(d);
        else if (d.Value < 0)
        {
            if (d.Consume)
                ConsumeGoldStagingNeuron.Invoke(d);
            else
                LoseGoldStagingNeuron.Invoke(d);
        }
    }
    
    public Neuron<SetDDHealthDetails> GainDHealthStagingNeuron = new();
    public Neuron<SetDDHealthDetails> LoseDHealthStagingNeuron = new();

    private void DHealthChanged(SetDDHealthDetails d)
    {
        if (d.Value > 0)
            GainDHealthStagingNeuron.Invoke(d);
        else if (d.Value < 0)
            LoseDHealthStagingNeuron.Invoke(d);
    }
}
