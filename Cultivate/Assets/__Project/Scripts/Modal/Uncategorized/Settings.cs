
using System;
using System.Collections.Generic;

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
                new SwitchModel("语言", new List<string>() { "中文", "还是中文" }),
                new ToggleModel("开关测试"),
            })),
            new("画面", new WidgetListModel(new WidgetModel[]
            {
                new SwitchModel("显示模式", new List<string>() { "窗口", "无边框全屏", "全屏" }),
                new SwitchModel("分辨率", new List<string>() { "1920x1080" }),
            })),
            new("声音", new WidgetListModel(new WidgetModel[]
            {
                new SliderModel("主音量", 0, 100, true, AudioManager.SetMasterVolume),
                new SliderModel("音乐", 0, 100, true, AudioManager.SetMusicVolume),
                new SliderModel("音效", 0, 100, true, AudioManager.SetSFXVolume),
                new ButtonModel("推荐音量"),
            })),
        });
        
        ResetSelectedTab();
    }

    public SettingsTab GetSelectedTab() => _selectedTab;
    public void SetSelectedTab(SettingsTab settingsTab) => _selectedTab = settingsTab;
    public void ResetSelectedTab() => _selectedTab = _tabs.Count() > 0 ? _tabs[0] : null;
    public bool IsSelectedTab(SettingsTab settingsTab) => settingsTab == _selectedTab;
    public int FindIndexOfTab(SettingsTab settingsTab) => _tabs.IndexOf(settingsTab);
}
