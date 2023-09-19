
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class NodeLayer : MonoBehaviour
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

    public bool CurrentIsDescriptor(PanelDescriptor d)
    {
        if (d == null && _currentPanel == null)
            return true;

        if (d != null && _panelDict[d.GetType()] == _currentPanel)
            return true;

        return false;
    }

    public Tween SetPanel(PanelDescriptor panelDescriptor)
        => SetPanel(_panelDict[panelDescriptor.GetType()]);

    public Tween SetPanel(Panel panel)
    {
        Sequence seq = DOTween.Sequence().SetAutoKill();

        if (_currentPanel == panel && panel != null)
        {
            panel.Refresh();
            return seq;
        }

        if (_currentPanel != null)
            seq.Append(_currentPanel.GetHideTween());
        _currentPanel = panel;
        if (_currentPanel != null)
        {
            seq.Append(_currentPanel.GetShowTween());
            _currentPanel.Refresh();
        }

        return seq;
    }

    public void Configure()
    {
        // _panelDict = new()
        // {
        //     { typeof(BattlePanelDescriptor), BattlePanel },
        //     { typeof(DialogPanelDescriptor), DialogPanel },
        //     { typeof(DiscoverSkillPanelDescriptor), DiscoverSkillPanel },
        //     { typeof(CardPickerPanelDescriptor), CardPickerPanel },
        //     { typeof(ShopPanelDescriptor), ShopPanel },
        //     { typeof(BarterPanelDescriptor), BarterPanel },
        //     { typeof(ArbitraryCardPickerPanelDescriptor), ArbitraryCardPickerPanel },
        // };
        //
        // _panelDict.Do(kvp => kvp.Value.Configure());
    }

    public void Refresh()
    {
        // if (_currentPanel != null)
        //     _currentPanel.Refresh();
    }
}
