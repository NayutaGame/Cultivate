
using System;
using System.Collections.Generic;

public struct PanelS : IEquatable<PanelS>
{
    private int _index;
    public int Index => _index;

    private PanelS(int index)
    {
        _index = index;
    }

    public static PanelS FromHide()
    {
        return new(0);
    }

    public static PanelS FromPanelDescriptor(Cell cell)
    {
        if (cell == null)
            return FromHide();
        return new(Dict[cell.GetType()]);
    }

    public static Dictionary<Type, int> Dict = new()
    {
        // 0 for hide
        { typeof(BattleCell),                1 },
        { typeof(PuzzleCell),                2 },
        { typeof(DialogCell),                3 },
        { typeof(DiscoverSkillCell),         4 },
        { typeof(CardPickerCell),            5 },
        { typeof(ShopCell),                  6 },
        { typeof(BarterCell),                7 },
        { typeof(GachaCell),                 8 },
        { typeof(ArbitraryCardPickerCell),   9 },
        { typeof(ImageCell),                10 },
        { typeof(ComicCell),                11 },
        { typeof(RunResultCell),            12 },
    };

    public bool Equals(PanelS other)
    {
        return _index == other._index;
    }

    public override bool Equals(object obj)
    {
        return obj is PanelS other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _index;
    }
}
