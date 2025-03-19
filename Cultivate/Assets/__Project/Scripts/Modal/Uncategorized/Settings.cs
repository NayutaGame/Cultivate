
using System;
using System.Collections.Generic;
using CLLibrary;

public class Settings : Addressable
{
    private SettingsTabListModel _tabs;
    private SettingsTab _selectedTab;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Settings()
    {
        _accessors = new()
        {
            { "Tabs",                           () => _tabs },
            { "CurrentWidgets",                 () => _selectedTab.Widgets },
        };

        _tabs = new();
        _tabs.AddRange(new SettingsTab[]
        {
            new("综合", new WidgetListModel(new WidgetModel[]
            {
                new SwitchModel("语言", this,
                    new List<string>() { "中文", "还是中文" },
                    null,
                    0),
                new ToggleModel("开关测试", this,
                    null,
                    0),
            })),
            new("画面", new WidgetListModel(new WidgetModel[]
            {
                new SwitchModel("显示模式", this,
                    new List<string>() { "全屏", "窗口", "无边框全屏" },
                    null,
                    0),
                new SwitchModel("分辨率", this,
                    new List<string>() { "1920x1080" },
                    null,
                    0),
            })),
            new("声音", new WidgetListModel(new WidgetModel[]
            {
                SliderModel.CreateWithDefaultRange("主音量", this,
                    AudioManager.SetMasterVolume,
                    50),
                SliderModel.CreateWithDefaultRange("音乐", this,
                    AudioManager.SetMusicVolume,
                    80),
                SliderModel.CreateWithDefaultRange("音效", this,
                    AudioManager.SetSFXVolume,
                    100),
                new ButtonModel("推荐音量", AudioManager.SetPreferredVolume),
            })),
        });
        
        LoadOrDefault();
        ApplySettingsData();
        
        ResetSelectedTab();
    }

    private SettingsData _settingsData;

    public SettingsData GetData()
        => _settingsData;
    
    private void LoadOrDefault()
    {
        if (!FileUtility.IsFileExists(SettingsData.Filename))
        {
            DefaultSettingsData();
            SaveProcedure();
        }
        else
        {
            LoadProcedure();
        }
    }

    private void ApplySettingsData()
    {
        foreach (var tab in _tabs.Traversal())
        foreach (var widget in tab.Widgets.Traversal())
            if (widget is SwitchModel switchModel)
                switchModel.Value = switchModel.Value;
            else if (widget is ToggleModel toggleModel)
                toggleModel.Value = toggleModel.Value;
            else if (widget is SliderModel sliderModel)
                sliderModel.Value = sliderModel.Value;
    }

    private void DefaultSettingsData()
    {
        _settingsData = new();
        foreach (var tab in _tabs.Traversal())
        foreach (var widget in tab.Widgets.Traversal())
            if (widget is SwitchModel switchModel)
                _settingsData._dict[widget.Name] = switchModel.DefaultValue;
            else if (widget is ToggleModel toggleModel)
                _settingsData._dict[widget.Name] = toggleModel.DefaultValue;
            else if (widget is SliderModel sliderModel)
                _settingsData._dict[widget.Name] = sliderModel.DefaultValue;
    }
    
    public void SaveProcedure()
    {
        FileUtility.WriteToFile(_settingsData, SettingsData.Filename);
    }

    public void LoadProcedure()
    {
        _settingsData = FileUtility.ReadFromFile<SettingsData>(SettingsData.Filename);
        // case存档损坏
    }

    public SettingsTab GetSelectedTab() => _selectedTab;
    public void SetSelectedTab(SettingsTab settingsTab) => _selectedTab = settingsTab;
    public void ResetSelectedTab() => _selectedTab = _tabs.Count() > 0 ? _tabs[0] : null;
    public bool IsSelectedTab(SettingsTab settingsTab) => settingsTab == _selectedTab;
    public int FindIndexOfTab(SettingsTab settingsTab) => _tabs.IndexOf(settingsTab);
}
