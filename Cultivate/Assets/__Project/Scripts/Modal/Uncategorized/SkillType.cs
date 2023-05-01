using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillType
{
    public static readonly SkillType Attack =   0x00000001;
    public static readonly SkillType JianZhen = 0x00000010;
    public static readonly SkillType LingQi =   0x00000100;

    private static readonly SkillType[] _list;
    private static readonly Dictionary<SkillType, string> _nameDict;

    static SkillType()
    {
        _list = new SkillType[]
        {
            Attack,
            JianZhen,
            LingQi,
        };

        _nameDict = new Dictionary<SkillType, string>()
        {
            { Attack, "攻击" },
            { JianZhen, "剑阵" },
            { LingQi, "灵气" },
        };
    }

    public static IEnumerable<SkillType> Traversal
    {
        get
        {
            foreach (var tag in _list) yield return tag;
        }
    }

    private int _value;
    public int Value => _value;

    public static implicit operator int(SkillType skillType) => skillType._value;
    public static implicit operator SkillType(int value) => new() { _value = value };

    public override string ToString()
    {
        return _nameDict[this];
    }
}
