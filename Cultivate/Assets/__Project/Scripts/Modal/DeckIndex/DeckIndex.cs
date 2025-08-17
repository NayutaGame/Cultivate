
using System;

public struct DeckIndex : IDeckIndex, IEquatable<DeckIndex>
{
    private SkillRegion _region;
    public SkillRegion Region => _region;
    private int _index;
    public int Index => _index;

    public DeckIndex(SkillRegion region, int index)
    {
        _region = region;
        _index = index;
    }

    public static bool operator ==(DeckIndex left, DeckIndex right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DeckIndex left, DeckIndex right)
    {
        return !(left == right);
    }

    public bool Equals(DeckIndex other)
    {
        return _region == other._region && _index == other._index;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;
        return Equals((DeckIndex)obj);
    }

    public override int GetHashCode() => _region.GetHashCode() + _index.GetHashCode();

    public static DeckIndex FromField(int index)
        => new(SkillRegion.Field, index);
    
    public static DeckIndex FromHand(int index = 0)
        => new(SkillRegion.Hand, index);

    public static DeckIndex FromRequirement(int index)
        => new(SkillRegion.Requirement, index);

    public DeckIndex Reify()
        => this;
}
