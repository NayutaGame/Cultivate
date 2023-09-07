using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private SettingsContentModel[] _contents;
    private int _index;

    public Settings()
    {
        _contents = new SettingsContentModel[]
        {
            new("选项卡一"),
            new("选项卡二"),
            new("选项卡三"),
        };

        _index = 0;
    }

    public SettingsContentModel GetCurrentContentModel()
        => _contents[_index];

    public int GetOtherContentCount()
        => _contents.Length - 1;

    public SettingsContentModel GetOtherContent(int i)
    {
        return _contents[i + (_index <= i ? 1 : 0)];
    }
}
