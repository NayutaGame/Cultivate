
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunConfigTabView : XView
{
    [SerializeField] protected TMP_Text RowLabel;
    public ToggleButton ToggleButton;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        ToggleButton.LeftClickNeuron.Join(ClickedTab);
    }

    // public override void Refresh()
    // {
    //     base.Refresh();
    //
    //     // RunConfigTabControl control = Get<RunConfigTabControl>();
    //     // Debug.Log($"Type = {control.GetType()}");
    //
    //     // SettingsTab settingsTab = Get<SettingsTab>();
    //     // RowLabel.text = settingsTab.Name;
    // }

    public virtual void OnEnable()
    {
        ApplyTabButtonDown();
    }

    public virtual void OnDisable()
    {
    }

    private void ApplyTabButtonDown()
    {
        bool isDown = Get<RunConfigTabControl>() == AppManager.Instance.ConfigManager.GetSelectedTab();
        ToggleButton.IsDown = isDown;
    }

    private void ClickedTab(InteractBehaviour ib, PointerEventData d)
    {
        RunConfigTabControl tab = Get<RunConfigTabControl>();
        AppManager.Instance.ConfigManager.SelectTabProcedure(tab);
    }
}