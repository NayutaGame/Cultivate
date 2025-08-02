
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JingJie : Entry, IComparable<JingJie>
{
    [NonSerialized] private int _index;
    [NonSerialized] private string _colorName;
    [NonSerialized] private string _rawDescription;
    [NonSerialized] private Description _description;
    [NonSerialized] private SpriteEntry _sprite;
    [NonSerialized] private string _audioName;
    [NonSerialized] private AudioEntry _audio;
    
    public JingJie(string id, string name, int index, string colorName, string audioName, string rawDescription) : base(id, name)
    {
        _index = index;
        _colorName = colorName;
        _audioName = audioName;
        _rawDescription = rawDescription;
    }

    public override void Init()
    {
        _sprite = Encyclopedia.SpriteCategory.FromName($"JingJie{GetName()}");
        _description = new(_rawDescription);
        _audio = Encyclopedia.AudioCategory.FromName(_audioName);
    }
    
    public int GetIndex() => _index;
    public string GetColorName() => _colorName;
    public Description GetDescription() => _description;
    public Sprite GetSprite() => _sprite.Sprite;
    public AudioEntry GetAudio() => _audio;
    
    // IComparable<JingJie> 实现
    public int CompareTo(JingJie other)
    {
        if (other == null) return 1;
        return _index.CompareTo(other._index);
    }
    
    // 重载比较运算符，方便使用
    public static bool operator <(JingJie left, JingJie right) => left.CompareTo(right) < 0;
    public static bool operator >(JingJie left, JingJie right) => left.CompareTo(right) > 0;
    public static bool operator <=(JingJie left, JingJie right) => left.CompareTo(right) <= 0;
    public static bool operator >=(JingJie left, JingJie right) => left.CompareTo(right) >= 0;
    
    public static JingJie LianQi => Encyclopedia.JingJieCategory[0];
    public static JingJie ZhuJi  =>Encyclopedia.JingJieCategory[1];
    public static JingJie JinDan  =>Encyclopedia.JingJieCategory[2];
    public static JingJie YuanYing => Encyclopedia.JingJieCategory[3];
    public static JingJie HuaShen  =>Encyclopedia.JingJieCategory[4];
    public static JingJie FanXu => Encyclopedia.JingJieCategory[5];

    public static IEnumerable<JingJie> Traversal
    {
        get
        {
            foreach (JingJie item in Encyclopedia.JingJieCategory)
                yield return item;
        }
    }

    public static CLLibrary.Bound LianQi2HuaShen => new(0, 5);
    public static CLLibrary.Bound LianQiOnly => new(0, 1);
    public static CLLibrary.Bound ZhuJi2HuaShen => new(1, 5);
    public static CLLibrary.Bound ZhuJiOnly => new(1, 2);
    public static CLLibrary.Bound JinDan2HuaShen => new(2, 5);
    public static CLLibrary.Bound JinDanOnly => new(2, 3);
    public static CLLibrary.Bound YuanYing2HuaShen => new(3, 5);
    public static CLLibrary.Bound YuanYingOnly => new(3, 4);
    public static CLLibrary.Bound HuaShenOnly => new(4, 5);
    public static CLLibrary.Bound FanXuOnly => new(5, 6);
    
    public static implicit operator int(JingJie jingJie) => jingJie._index;
    public static implicit operator JingJie(int index) => Encyclopedia.JingJieCategory[index];
}