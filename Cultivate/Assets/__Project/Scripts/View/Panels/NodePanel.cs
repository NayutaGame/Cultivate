using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePanel : Panel
{
    public BattlePanel BattlePanel;
    public DialogPanel DialogPanel;
    public DiscoverSkillPanel DiscoverSkillPanel;

    private Panel _currentPanel;

    private void ChangePanel(Panel panel)
    {
        if (_currentPanel == panel)
        {
            panel.Refresh();
            return;
        }

        if (_currentPanel != null)
            _currentPanel.gameObject.SetActive(false);
        _currentPanel = panel;
        _currentPanel.gameObject.SetActive(true);
        _currentPanel.Refresh();
    }

    public override void Configure()
    {
        base.Configure();
    }

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        PanelDescriptor d = runNode?.CurrentPanel;
        if (d is BattlePanelDescriptor)
        {
            ChangePanel(BattlePanel);
        }
        else if (d is DialogPanelDescriptor)
        {
            ChangePanel(DialogPanel);
        }
        else if (d is DiscoverSkillPanelDescriptor)
        {
            ChangePanel(DiscoverSkillPanel);
        }
    }
}
