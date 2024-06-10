
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RunCanvas : MonoBehaviour
{
    public RunPanelCollection RunPanelCollection;

    public DeckPanel DeckPanel;
    public MapPanel MapPanel;
    public Button MapButton;
    public ReservedLayer ReservedLayer;
    public TopBar TopBar;
    public ConsolePanel ConsolePanel;

    public void Configure()
    {
        RunPanelCollection.Configure();
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

    public void Refresh()
    {
        RunPanelCollection.Refresh();
        DeckPanel.Refresh();
        MapPanel.Refresh();
        ReservedLayer.Refresh();
        TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    public async Task SetNodeState(PanelDescriptor panelDescriptor)
    {
        if (RunPanelCollection.CurrentIsDescriptor(panelDescriptor))
        {
            RunPanelCollection.Refresh();
            return;
        }

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();

        if (panelDescriptor == null)
        {
            await RunPanelCollection.SetPanel(panel: null);
            await MapPanel.AsyncSetState(1);
        }
        else
        {
            await MapPanel.AsyncSetState(0);
            await RunPanelCollection.SetPanel(d);
        }

        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor || d is PuzzlePanelDescriptor)
        {
            DeckPanel.SetLocked(true);
            await DeckPanel.AsyncSetState(1);
        }
        else
        {
            DeckPanel.SetLocked(false);
            await DeckPanel.AsyncSetState(0);
        }
    }
}
