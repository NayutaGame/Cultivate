
using System;

public struct DeckIndex : IEquatable<DeckIndex>
{
    private bool _inField;
    public bool InField => _inField;
    private int _index;
    public int Index => _index;

    public DeckIndex(bool inField, int index)
    {
        _inField = inField;
        _index = index;
    }

    public Address ToAddress()
    {
        if (_inField)
            return new Address($"Run.Environment.Hero.Slots#{_index}");
        else
            return new Address($"Run.Environment.Hand#{_index}");
    }

    public bool Equals(DeckIndex other)
    {
        return _inField == other._inField && _index == other._index;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;
        return Equals((DeckIndex)obj);
    }

    public override int GetHashCode() => _inField.GetHashCode() + _index.GetHashCode();

    public override string ToString()
    {
        string r = _inField ? "战斗区" : "手牌区";
        return $"{r}#{_index}";
    }
}
