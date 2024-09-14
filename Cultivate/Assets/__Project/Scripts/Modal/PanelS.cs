
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

    public static PanelS FromPanelDescriptor(PanelDescriptor panelDescriptor)
    {
        return new(Dict[panelDescriptor.GetType()]);
    }

    public static Dictionary<Type, int> Dict = new()
    {
        // 0 for hide
        { typeof(BattlePanelDescriptor),                1 },
        { typeof(PuzzlePanelDescriptor),                2 },
        { typeof(DialogPanelDescriptor),                3 },
        { typeof(DiscoverSkillPanelDescriptor),         4 },
        { typeof(CardPickerPanelDescriptor),            5 },
        { typeof(ShopPanelDescriptor),                  6 },
        { typeof(BarterPanelDescriptor),                7 },
        { typeof(GachaPanelDescriptor),                 8 },
        { typeof(ArbitraryCardPickerPanelDescriptor),   9 },
        { typeof(ImagePanelDescriptor),                10 },
        { typeof(RunResultPanelDescriptor),            11 },
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
