using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePanel : Panel
{
    public BattlePanel BattlePanel;
    public DialogPanel DialogPanel;
    public DiscoverSkillPanel DiscoverSkillPanel;
    public CardPickerPanel CardPickerPanel;
    public ShopPanel ShopPanel;
    public BarterPanel BarterPanel;
    public ArbitraryCardPickerPanel ArbitraryCardPickerPanel;

    private Panel _currentPanel;

    private Dictionary<Type, Panel> _panelDict;

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

        _panelDict = new()
        {
            { typeof(BattlePanelDescriptor), BattlePanel },
            { typeof(DialogPanelDescriptor), DialogPanel },
            { typeof(DiscoverSkillPanelDescriptor), DiscoverSkillPanel },
            { typeof(CardPickerPanelDescriptor), CardPickerPanel },
            { typeof(ShopPanelDescriptor), ShopPanel },
            { typeof(BarterPanelDescriptor), BarterPanel },
            { typeof(ArbitraryCardPickerPanelDescriptor), ArbitraryCardPickerPanel },
        };
    }

    public override void Refresh()
    {
        base.Refresh();

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        PanelDescriptor d = runNode?.CurrentPanel;
        if (d != null)
            ChangePanel(_panelDict[d.GetType()]);
    }
}
