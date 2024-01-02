
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

        if (d is CardPickerPanelDescriptor)
        {
            DeckPanel.HandView.LeftClickNeuron.Add(ToggleSkill);
            DeckPanel.FieldView.LeftClickNeuron.Add(ToggleSkillSlot);
        }
        else
        {
            DeckPanel.HandView.LeftClickNeuron.Remove(ToggleSkill);
            DeckPanel.FieldView.LeftClickNeuron.Remove(ToggleSkillSlot);
        }
    }

    private void ToggleSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        RunPanelCollection.CardPickerPanel.ToggleSkill(ib);
        Refresh();
    }

    private void ToggleSkillSlot(InteractBehaviour ib, PointerEventData eventData)
    {
        RunPanelCollection.CardPickerPanel.ToggleSkillSlot(ib);
        Refresh();
    }
}
