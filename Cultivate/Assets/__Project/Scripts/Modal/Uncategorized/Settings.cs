
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

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
            // new("综合", new WidgetListModel(new WidgetModel[]
            // {
            //     new SwitchModel("语言", this,
            //         new List<string>() { "中文", "还是中文" },
            //         null,
            //         0),
            //     new ToggleModel("开关测试", this,
            //         null,
            //         0),
            // })),
            new("画面", new WidgetListModel(new WidgetModel[]
            {
                new SwitchModel("显示模式", this,
                    new List<string>() { "全屏", "窗口", "无边框全屏" },
                    SetFullscreen,
                    0),
                new SwitchModel("分辨率", this,
                    new List<string>() { "3840x2160", "2560x1440", "1920x1080", "1600x900", "1280x720", "2560x1600", "1920x1200", "1440x900", "1280x800", "800x600" },
                    SetResolution,
                    2),
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
                new ButtonModel("推荐音量", SetPreferredVolume),
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
        if (!FileUtility.IsPersistentFileExists(SettingsData.Filename))
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
        foreach (var tab in _tabs)
        foreach (var widget in tab.Widgets)
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
        foreach (var tab in _tabs)
        foreach (var widget in tab.Widgets)
            if (widget is SwitchModel switchModel)
                _settingsData._dict[widget.Name] = switchModel.DefaultValue;
            else if (widget is ToggleModel toggleModel)
                _settingsData._dict[widget.Name] = toggleModel.DefaultValue;
            else if (widget is SliderModel sliderModel)
                _settingsData._dict[widget.Name] = sliderModel.DefaultValue;
    }
    
    public void SaveProcedure()
    {
        FileUtility.WritePersistentFile(_settingsData, SettingsData.Filename);
    }

    public void LoadProcedure()
    {
        _settingsData = FileUtility.ReadPersistentFile<SettingsData>(SettingsData.Filename);
        // case存档损坏
    }

    public SettingsTab GetSelectedTab() => _selectedTab;
    public void SetSelectedTab(SettingsTab settingsTab) => _selectedTab = settingsTab;
    public void ResetSelectedTab() => _selectedTab = _tabs.Count() > 0 ? _tabs[0] : null;
    public bool IsSelectedTab(SettingsTab settingsTab) => settingsTab == _selectedTab;
    public int FindIndexOfTab(SettingsTab settingsTab) => _tabs.IndexOf(settingsTab);

    #region Implementations

    private void SetFullscreen(int index)
    {
        // "全屏", "窗口", "无边框全屏"
        switch (index)
        {
            case 0: // 全屏
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1: // 窗口
                Screen.fullScreen = false;
                break;
            case 2: // 无边框全屏
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    private void SetResolution(int index)
    {
        // 3840x2160, 2560x1440, 1920x1080, 1600x900, 1280x720
        // 2560x1600, 1920x1200, 1440x900, 1280x800
        // 800x600
        int width = 0;
        int height = 0;
        
        switch (index)
        {
            case 0: // 3840x2160 (4K)
                width = 3840;
                height = 2160;
                break;
            case 1: // 2560x1440 (2K)
                width = 2560;
                height = 1440;
                break;
            case 2: // 1920x1080 (1080p)
                width = 1920;
                height = 1080;
                break;
            case 3: // 1600x900
                width = 1600;
                height = 900;
                break;
            case 4: // 1280x720 (720p)
                width = 1280;
                height = 720;
                break;
            case 5: // 2560x1600
                width = 2560;
                height = 1600;
                break;
            case 6: // 1920x1200
                width = 1920;
                height = 1200;
                break;
            case 7: // 1440x900
                width = 1440;
                height = 900;
                break;
            case 8: // 1280x800
                width = 1280;
                height = 800;
                break;
            case 9: // 800x600
                width = 800;
                height = 600;
                break;
        }

        if (width > 0 && height > 0)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }

    private void SetPreferredVolume()
    {
        SettingsTab soundTab = _tabs.GetSoundTab();
        Assert.IsTrue(soundTab != null);
        
        foreach (var widget in soundTab.Widgets)
            if (widget is SwitchModel switchModel)
                switchModel.Value = switchModel.DefaultValue;
            else if (widget is ToggleModel toggleModel)
                toggleModel.Value = toggleModel.DefaultValue;
            else if (widget is SliderModel sliderModel)
                sliderModel.Value = sliderModel.DefaultValue;
        
        CanvasManager.Instance.AppCanvas.SettingsPanel.Refresh();
    }

    #endregion
}
