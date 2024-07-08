
using System.Collections.Generic;
using System.Text;
using CLLibrary;

public class SkillTypeComposite
{
    private List<SkillType> _skillTypes;
    public List<SkillType> SkillTypes => _skillTypes;

    private int _value;
    public int Value => _value;

    public SkillTypeComposite(int value)
    {
        _value = value;

        _skillTypes = new List<SkillType>();
        _skillTypes.AddRange(SkillType.Traversal.FilterObj(Contains));
    }

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
    public static implicit operator SkillTypeComposite(int value) => new(value);
    public static implicit operator SkillTypeComposite(SkillType skillType) => skillType._value;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        SkillTypes.Do(skillType => sb.Append($"{skillType._name} "));
        return sb.ToString();
    }

    public SkillTypeComposite Clone() => _value;
}
