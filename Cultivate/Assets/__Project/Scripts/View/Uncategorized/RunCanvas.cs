
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunCanvas : MonoBehaviour
{
    public NodeLayer NodeLayer;

    public DeckPanel DeckPanel;
    public MapPanel MapPanel;
    public Button MapButton;
    public ReservedLayer ReservedLayer;
    public TopBar TopBar;
    public Button ConsoleButton;
    public ConsolePanel ConsolePanel;

    public void Configure()
    {
        NodeLayer.Configure();
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
        NodeLayer.Refresh();
        DeckPanel.Refresh();
        MapPanel.Refresh();
        ReservedLayer.Refresh();
        TopBar.Refresh();
        ConsolePanel.Refresh();
    }

    public async Task SetNodeState(PanelDescriptor panelDescriptor)
    {
        if (NodeLayer.CurrentIsDescriptor(panelDescriptor))
        {
            NodeLayer.Refresh();
            return;
        }

        PanelDescriptor d = RunManager.Instance.Environment.GetActivePanel();

        if (panelDescriptor == null)
        {
            await NodeLayer.SetPanel(panel: null);
            await MapPanel.SetShowing(true);
        }
        else
        {
            await MapPanel.SetShowing(false);
            await NodeLayer.SetPanel(d);
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

        InteractDelegate interactDelegate = DeckPanel.GetDelegate();
        if (d is CardPickerPanelDescriptor)
        {
            interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, ToggleSkill);
            interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 2, ToggleSkillSlot);
        }
        else
        {
            interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, null);
            interactDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 2, null);
        }
    }

    private void ToggleSkill(IInteractable view, PointerEventData eventData)
    {
        NodeLayer.CardPickerPanel.ToggleSkill(view);
        Refresh();
    }

    private void ToggleSkillSlot(IInteractable view, PointerEventData eventData)
    {
        NodeLayer.CardPickerPanel.ToggleSkillSlot(view);
        Refresh();
    }
}
