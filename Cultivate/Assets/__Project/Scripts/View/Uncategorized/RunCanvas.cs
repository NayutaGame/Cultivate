
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunCanvas : MonoBehaviour
{
    public RunPanelCollection RunPanelCollection;

    public DeckPanel DeckPanel;
    public MapPanel MapPanel;
    public Button MapButton;
    public ReservedLayer ReservedLayer;
    public TopBar TopBar;
    public Button ConsoleButton;
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

        ConsoleButton.onClick.RemoveAllListeners();
        ConsoleButton.onClick.AddListener(() => ConsolePanel.ToggleShowing());

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
            await MapPanel.SetShowing(true);
        }
        else
        {
            await MapPanel.SetShowing(false);
            await RunPanelCollection.SetPanel(d);
        }

        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor)
        {
            DeckPanel.SetLocked(true);
            await DeckPanel.SetShowing(true);
        }
        else
        {
            DeckPanel.SetLocked(false);
            await DeckPanel.SetShowing(false);
        }

        InteractHandler interactHandler = DeckPanel.GetDelegate();
        if (d is CardPickerPanelDescriptor)
        {
            interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, ToggleSkill);
            interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 2, ToggleSkillSlot);
        }
        else
        {
            interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 0, null);
            interactHandler.SetHandle(InteractHandler.POINTER_LEFT_CLICK, 2, null);
        }
    }

    private void ToggleSkill(InteractBehaviour view, PointerEventData eventData)
    {
        RunPanelCollection.CardPickerPanel.ToggleSkill(view);
        Refresh();
    }

    private void ToggleSkillSlot(InteractBehaviour view, PointerEventData eventData)
    {
        RunPanelCollection.CardPickerPanel.ToggleSkillSlot(view);
        Refresh();
    }
}
