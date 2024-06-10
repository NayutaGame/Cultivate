
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RunPanelCollection : MonoBehaviour
{
    public BattlePanel BattlePanel;
    public PuzzlePanel PuzzlePanel;
    public DialogPanel DialogPanel;
    public DiscoverSkillPanel DiscoverSkillPanel;
    public CardPickerPanel CardPickerPanel;
    public ShopPanel ShopPanel;
    public BarterPanel BarterPanel;
    public ArbitraryCardPickerPanel ArbitraryCardPickerPanel;
    public ImagePanel ImagePanel;
    public RunResultPanel RunResultPanel;

    private Panel _currentPanel;

    private Dictionary<Type, Panel> _panelDict;

    public bool CurrentIsDescriptor(PanelDescriptor d)
    {
        // if (d == null && _currentPanel == null)
        //     return true;

        if (d != null && _panelDict[d.GetType()] == _currentPanel)
            return true;

        return false;
    }

    public async Task SetPanel(PanelDescriptor panelDescriptor)
        => await SetPanel(_panelDict[panelDescriptor.GetType()]);

    public async Task SetPanel(Panel panel)
    {
        if (_currentPanel == panel && panel != null)
        {
            panel.Refresh();
        }

        if (_currentPanel != null)
            await _currentPanel.AsyncSetState(0);
        _currentPanel = panel;
        if (_currentPanel != null)
        {
            _currentPanel.Configure();
            _currentPanel.Refresh();
            await _currentPanel.AsyncSetState(1);
        }
    }

    public void DisableCurrentPanel()
    {
        if (_currentPanel == null)
            return;

        _currentPanel.gameObject.SetActive(false);
        _currentPanel.SetState(0);
        _currentPanel = null;
    }

    public void Configure()
    {
        _panelDict = new()
        {
            { typeof(BattlePanelDescriptor), BattlePanel },
            { typeof(PuzzlePanelDescriptor), PuzzlePanel },
            { typeof(DialogPanelDescriptor), DialogPanel },
            { typeof(DiscoverSkillPanelDescriptor), DiscoverSkillPanel },
            { typeof(CardPickerPanelDescriptor), CardPickerPanel },
            { typeof(ShopPanelDescriptor), ShopPanel },
            { typeof(BarterPanelDescriptor), BarterPanel },
            { typeof(ArbitraryCardPickerPanelDescriptor), ArbitraryCardPickerPanel },
            { typeof(ImagePanelDescriptor), ImagePanel },
            { typeof(RunResultPanelDescriptor), RunResultPanel },
        };

        // _panelDict.Do(kvp => kvp.Value.Configure());
    }

    public void Refresh()
    {
        if (_currentPanel != null)
        {
            _currentPanel.Configure();
            _currentPanel.Refresh();
        }
    }
}
