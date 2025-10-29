
using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

[Serializable]
public class JingJie : Entry, AnnotatableJingJie, IComparable<JingJie>
{
    [NonSerialized] private int _index;
    [NonSerialized] private string _colorName;
    [NonSerialized] private string _rawDescription;
    [NonSerialized] private Description _description;
    [NonSerialized] private SpriteEntry _sprite;
    [NonSerialized] private string _audioName;
    [NonSerialized] private AudioEntry _audio;
    [NonSerialized] private SpriteEntry _backgroundSprite;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
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
        _backgroundSprite = Encyclopedia.SpriteCategory.FromName($"{GetName()}背景");
    }
    
    public int GetIndex() => _index;
    public string GetColorName() => _colorName;
    public Description GetDescription() => _description;
    public Sprite GetSprite() => _sprite.Sprite;
    public AudioEntry GetAudio() => _audio;
    public Sprite GetBackgroundSprite() => _backgroundSprite.Sprite;
    public bool CanShowAnnotation() => true;
    public Sprite GetIconSprite() => _sprite.Sprite;

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

    public static Bound LianQiOnly => new(0, 0);
    public static Bound LianQi2ZhuJi => new(0, 1);
    public static Bound LianQi2HuaShen => new(0, 4);
    public static Bound LianQi2FanXu => new(0, 5);
    public static Bound ZhuJiOnly => new(1, 1);
    public static Bound ZhuJi2HuaShen => new(1, 4);
    public static Bound ZhuJi2FanXu => new(1, 5);
    public static Bound JinDanOnly => new(2, 2);
    public static Bound JinDan2YuanYing => new(2, 3);
    public static Bound JinDan2HuaShen => new(2, 4);
    public static Bound JinDan2FanXu => new(2, 5);
    public static Bound YuanYingOnly => new(3, 3);
    public static Bound YuanYing2HuaShen => new(3, 4);
    public static Bound YuanYing2FanXu => new(3, 5);
    public static Bound HuaShenOnly => new(4, 4);
    public static Bound HuaShen2FanXu => new(4, 5);
    public static Bound FanXuOnly => new(5, 5);
    
    public static implicit operator int(JingJie jingJie) => jingJie._index;
    public static implicit operator JingJie(int index) => Encyclopedia.JingJieCategory[index];

    public static JingJie FromJingJieType(JingJieType jingJieType) => (int)jingJieType;
}