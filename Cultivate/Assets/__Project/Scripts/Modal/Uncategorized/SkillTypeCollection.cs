using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SkillTypeCollection
{
    public static readonly SkillTypeCollection None =     0b00000000;
    public static readonly SkillTypeCollection Attack =   0b00000001;
    public static readonly SkillTypeCollection JianZhen = 0b00000010;
    public static readonly SkillTypeCollection LingQi =   0b00000100;
    public static readonly SkillTypeCollection ErDong =   0b00001000;
    public static readonly SkillTypeCollection XiaoHao =  0b00010000;

    public IEnumerable<SkillType> ContainedTags
    {
        get
        {
            foreach (SkillType tag in SkillType.Traversal)
            {
                if (Contains(tag))
                    yield return tag;
            }
        }
    }

    private int _value;
    public int Value => _value;

    public bool Contains(SkillTypeCollection other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public bool Contains(SkillType other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public static implicit operator int(SkillTypeCollection skillTypeCollection) => skillTypeCollection._value;
    public static implicit operator SkillTypeCollection(int value) => new() { _value = value };

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (Contains(Attack))
            sb.Append("攻击 ");

        if (Contains(JianZhen))
            sb.Append("剑阵 ");

        if (Contains(LingQi))
            sb.Append("灵气 ");

        if (Contains(ErDong))
            sb.Append("二动 ");

        if (Contains(XiaoHao))
            sb.Append("消耗 ");

        return sb.ToString();
    }
}
