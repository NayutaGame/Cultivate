using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePanel : Panel
{
    public BattlePanel BattlePanel;
    public DialogPanel DialogPanel;

    private Panel _currentPanel;

    private void OpenPanel(Panel panel)
    {
        if(_currentPanel != null)
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
            if (_currentPanel == BattlePanel)
            {
                BattlePanel.Refresh();
            }
            else
            {
                OpenPanel(BattlePanel);
            }
        }
        else if (d is DialogPanelDescriptor)
        {
            if (_currentPanel == DialogPanel)
            {
                DialogPanel.Refresh();
            }
            else
            {
                OpenPanel(DialogPanel);
            }
        }
    }
}
