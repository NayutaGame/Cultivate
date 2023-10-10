
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RunCanvas : Singleton<RunCanvas>
{
    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    public Button BackgroundButton;

    public ReservedLayer ReservedLayer;
    public NodeLayer NodeLayer;
    public MMDMLayer MMDMLayer;

    public TopBar TopBar;

    public Button ToggleShowingConsolePanelButton;
    public ConsolePanel ConsolePanel;

    public MechGhost MechGhost;
    public SkillPreview SkillPreview;
    public FormationPreview FormationPreview;

    public override void DidAwake()
    {
        base.DidAwake();
        Configure();
        Refresh();
    }

    private void ToggleConsolePanel() => ConsolePanel.Toggle();

    public void Configure()
    {
        BackgroundButton.onClick.RemoveAllListeners();
        BackgroundButton.onClick.AddListener(MMDMLayer.ToggleMap);

        TopBar.Configure();
        ConsolePanel.Configure();

        ToggleShowingConsolePanelButton.onClick.RemoveAllListeners();
        ToggleShowingConsolePanelButton.onClick.AddListener(ToggleConsolePanel);

        ReservedLayer.Configure();
        MMDMLayer.Configure();
        NodeLayer.Configure();

        if (!Application.isEditor)
            ConsolePanel.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        TopBar.Refresh();
        ConsolePanel.Refresh();

        ReservedLayer.Refresh();
        NodeLayer.Refresh();
        MMDMLayer.Refresh();
    }

    public void SetNodeState(PanelDescriptor panelDescriptor)
    {
        if (NodeLayer.CurrentIsDescriptor(panelDescriptor))
        {
            NodeLayer.Refresh();
            return;
        }

        Sequence seq = DOTween.Sequence().SetAutoKill();

        PanelDescriptor d = RunManager.Instance.Environment.Map.CurrentNode?.CurrentPanel;

        if (panelDescriptor == null)
        {
            seq.Join(NodeLayer.SetPanel(panel: null));

            seq.AppendInterval(0.4f)
                .Append(MMDMLayer.SetState(MMDMLayer.MMDMState.MM));
        }
        else
        {
            if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor)
                seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.D));
            else
                seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.N));

            seq.AppendInterval(0.4f)
                .Join(NodeLayer.SetPanel(d));
        }

        InteractDelegate interactDelegate = MMDMLayer.DeckPanel.GetDelegate();
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

        seq.Restart();
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
