
using System;
using System.Collections.Generic;

public class Settings : Addressable
{
    private SettingsContentModel[] _options;
    private int _index;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Settings()
    {
        _accessors = new()
        {
            { "CurrentWidgets",                 () => GetCurrentContentModel().Widgets },
        };

        _options = new SettingsContentModel[]
        {
            new("综合", new WidgetModel[]
            {
                new SwitchModel("语言", new List<string>() { "中文" }),
                new CheckboxModel("这里有个开关"),
            }),
            new("画面", new WidgetModel[]
            {
                new SwitchModel("显示模式", new List<string>() { "窗口", "无边框全屏", "全屏" }),
                new SwitchModel("分辨率", new List<string>() { "1920x1080" }),
            }),
            new("声音", new WidgetModel[]
            {
                new SliderModel("主音量", 0, 100, true),
                new SliderModel("音乐", 0, 100, true),
                new SliderModel("音效", 0, 100, true),
                new ButtonModel("推荐音量"),
            }),
        };

        _index = 0;
    }

    public SettingsContentModel GetCurrentContentModel()
        => _options[_index];

    public int GetOtherContentCount()
        => _options.Length - 1;

    public SettingsContentModel GetOtherContent(int i)
    {
        return _options[i + (_index <= i ? 1 : 0)];
    }

    public void ChangeIndex(int i)
    {
        _index = i + (_index <= i ? 1 : 0);
    }
}
