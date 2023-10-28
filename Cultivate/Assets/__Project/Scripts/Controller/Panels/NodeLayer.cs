
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
    public ImagePanel ImagePanel;
    public RunResultPanel RunResultPanel;

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
            seq.Append(_currentPanel.HideAnimation());
        _currentPanel = panel;
        if (_currentPanel != null)
        {
            seq.Append(_currentPanel.ShowAnimation());
            _currentPanel.Configure();
            _currentPanel.Refresh();
        }

        return seq;
    }

    public void DisableCurrentPanel()
    {
        if (_currentPanel == null)
            return;

        _currentPanel.gameObject.SetActive(false);
        _currentPanel = null;
    }

    public void Configure()
    {
        _panelDict = new()
        {
            { typeof(BattlePanelDescriptor), BattlePanel },
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
