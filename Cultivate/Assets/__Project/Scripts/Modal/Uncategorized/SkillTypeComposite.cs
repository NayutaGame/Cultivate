
using System.Collections.Generic;
using System.Text;

public class SkillTypeComposite
{
    public IEnumerable<SkillType> ContainedSkillTypes
    {
        get
        {
            foreach (SkillType skillType in SkillType.Traversal)
            {
                if (Contains(skillType))
                    yield return skillType;
            }
        }
    }

    private int _value;
    public int Value => _value;

    public bool Contains(SkillTypeComposite other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public bool Contains(SkillType other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public static implicit operator int(SkillTypeComposite skillTypeComposite) => skillTypeComposite._value;
    public static implicit operator SkillTypeComposite(int value) => new() { _value = value };
    public static implicit operator SkillTypeComposite(SkillType skillType) => skillType._index;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (SkillType skillType in SkillType.Traversal)
        {
            if (Contains(skillType))
                sb.Append($"{skillType._name} ");
        }

        return sb.ToString();
    }
}
