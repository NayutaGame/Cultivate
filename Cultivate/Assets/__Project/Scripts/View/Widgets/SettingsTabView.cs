
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsTabView : XView
{
    [SerializeField] private TMP_Text RowLabel;
    public ToggleButton ToggleButton;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ToggleButton.LeftClickNeuron.Join(ClickedTab);
    }

    public override void Refresh()
    {
        base.Refresh();

        SettingsTab settingsTab = Get<SettingsTab>();
        RowLabel.text = settingsTab.Name;
    }

    private void OnEnable()
    {
        ApplyTabButtonDown();
    }
    
    private void ApplyTabButtonDown()
    {
        bool isDown = Get<SettingsTab>() == AppManager.Instance.Settings.GetSelectedTab();
        ToggleButton.IsDown = isDown;
    }

    private void ClickedTab(InteractBehaviour ib, PointerEventData d)
    {
        SettingsTab tab = Get<SettingsTab>();
        AppManager.Instance.Settings.SelectTabProcedure(tab);
    }
}