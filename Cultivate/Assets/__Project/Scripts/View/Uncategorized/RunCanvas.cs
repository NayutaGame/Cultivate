
using CLLibrary;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

    public SkillGhost SkillGhost;
    [SerializeField] private RunSkillPreview RunSkillPreview;

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

        MMDMLayer.Configure();
        NodeLayer.Configure();
    }

    public void Refresh()
    {
        TopBar.Refresh();
        ConsolePanel.Refresh();

        NodeLayer.Refresh();
        MMDMLayer.Refresh();
    }

    public void SetIndexPathForPreview(IndexPath indexPath)
    {
        RunSkillPreview.Configure(indexPath);
        RunSkillPreview.Refresh();
    }

    public void UpdateMousePosForPreview(Vector2 pos)
    {
        RunSkillPreview.UpdateMousePos(pos);
        RunSkillPreview.Refresh();
    }

    public void SetNodeState(PanelDescriptor panelDescriptor)
    {
        if (NodeLayer.CurrentIsDescriptor(panelDescriptor))
            return;

        Sequence seq = DOTween.Sequence().SetAutoKill();

        PanelDescriptor d = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel;

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

        seq.Restart();
    }
}
