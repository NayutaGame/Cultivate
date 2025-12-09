
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsPanel : PopupPanel
{
    [SerializeField] public ListView TabListView;
    
    [SerializeField] private ListView WidgetListView;

    [SerializeField] private CLButton ToTitleButton;
    [SerializeField] private CLButton ToDesktopButton;
    [SerializeField] private CLButton ResumeButton;

    public void ShowExitButtons()
    {
        ToTitleButton.gameObject.SetActive(true);
        ToDesktopButton.gameObject.SetActive(true);
    }

    public void HideExitButtons()
    {
        ToTitleButton.gameObject.SetActive(false);
        ToDesktopButton.gameObject.SetActive(false);
    }

    private Address _address;
    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Settings");
        
        TabListView.SetAddress(_address.Append(".Tabs"));

        WidgetListView.SetPrefabProvider(model =>
        {
            WidgetModel widgetModel = (WidgetModel)model;
            if (widgetModel is SliderModel)
                return 0;
            if (widgetModel is SwitchModel)
                return 1;
            if (widgetModel is ToggleModel)
                return 2;
            if (widgetModel is ButtonModel)
                return 3;
            return -1;
        });
        WidgetListView.SetAddress(_address.Append(".CurrentWidgets"));
        WidgetListView.Refresh();
    }

    public override void Refresh()
    {
        TabListView.Refresh();
        WidgetListView.Refresh();
    }

    private void OnEnable()
    {
        AppManager.Instance.Settings.SettingsTabChangedNeuron.Add(TabChanged);
        
        ToTitleButton.LeftClickNeuron.Add(ToTitle);
        ToDesktopButton.LeftClickNeuron.Add(ToDesktop);
        ResumeButton.LeftClickNeuron.Add(Return);
        
        AudioManager.PlayEnterSettings();
        
        AppManager.Instance.PushEscFunc(Return);
    }

    private void OnDisable()
    {
        AppManager.Instance.Settings.SettingsTabChangedNeuron.Remove(TabChanged);
        
        ToTitleButton.LeftClickNeuron.Remove(ToTitle);
        ToDesktopButton.LeftClickNeuron.Remove(ToDesktop);
        ResumeButton.LeftClickNeuron.Remove(Return);
        
        AudioManager.PlayExitSettings();
        
        AppManager.Instance.PopEscFunc();
    }
    
    public override void Return()
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.Instance.Pop();
    }

    private void Return(InteractBehaviour ib, PointerEventData d)
        => Return();

    private void ToTitle(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.Instance.Pop(2);
    }

    private void ToDesktop(InteractBehaviour ib, PointerEventData d)
    {
        AppManager.Instance.Settings.SaveProcedure();
        AppManager.ExitGame();
    }

    private void TabChanged(SettingsTabChangedDetails d)
    {
        SlotView fromSlot = TabListView.ViewFromIndex(d.FromIndex);
        SettingsTabView fromView = fromSlot.GetContentView() as SettingsTabView;
        fromView.ToggleButton.IsDown = false;
        
        SlotView toSlot = TabListView.ViewFromIndex(d.ToIndex);
        SettingsTabView toView = toSlot.GetContentView() as SettingsTabView;
        toView.ToggleButton.IsDown = true;
        
        WidgetListView.Sync();
    }
}
