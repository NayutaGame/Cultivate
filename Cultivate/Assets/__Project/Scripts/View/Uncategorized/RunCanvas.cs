
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RunCanvas : Singleton<RunCanvas>
{
    public Color TechColorGreen;
    public Color TechColorYellow;
    public Color TechColorRed;

    public Button BackgroundButton;

    public ThirdPanelLayer ThirdPanelLayer;
    public NodePanelLayer NodePanelLayer;
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

    public void SetSecondLayerToShow()
    {
        Sequence seq = DOTween.Sequence().SetAutoKill();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        PanelDescriptor d = runNode?.CurrentPanel;

        if (d is BattlePanelDescriptor || d is CardPickerPanelDescriptor)
            seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.D));
        else
            seq.Append(MMDMLayer.SetState(MMDMLayer.MMDMState.N));

        seq.AppendInterval(0.4f)
            .Join(NodePanelLayer.SetPanel(d));

        seq.Restart();
    }

    public void SetSecondLayerToHide()
    {

    }

    public void Configure()
    {
        BackgroundButton.onClick.AddListener(MMDMLayer.ToggleMap);

        TopBar.Configure();
        ToggleShowingConsolePanelButton.onClick.AddListener(ToggleConsolePanel);

        MMDMLayer.Configure();
        NodePanelLayer.Configure();
    }

    public void Refresh()
    {
        TopBar.Refresh();
        ConsolePanel.Refresh();

        NodePanelLayer.Refresh();
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

    public void SetState(MMDMLayer.MMDMState mmdmState, Panel panel)
    {

    }
}
