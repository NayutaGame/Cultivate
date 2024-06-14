
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

    public static PanelS FromMap()
    {
        return new(1);
    }

    public static PanelS FromPanelDescriptor(PanelDescriptor panelDescriptor)
    {
        return new(Dict[panelDescriptor.GetType()]);
    }

    public static PanelS FromPanelDescriptorNullMeansMap(PanelDescriptor panelDescriptor)
    {
        return new(panelDescriptor != null ? Dict[panelDescriptor.GetType()] : 1);
    }

    public static Dictionary<Type, int> Dict = new()
    {
        // 0 for hide
        // 1 for map
        { typeof(BattlePanelDescriptor), 2 },
        { typeof(PuzzlePanelDescriptor), 3 },
        { typeof(DialogPanelDescriptor), 4 },
        { typeof(DiscoverSkillPanelDescriptor), 5 },
        { typeof(CardPickerPanelDescriptor), 6 },
        { typeof(ShopPanelDescriptor), 7 },
        { typeof(BarterPanelDescriptor), 8 },
        { typeof(ArbitraryCardPickerPanelDescriptor), 9 },
        { typeof(ImagePanelDescriptor), 10 },
        { typeof(RunResultPanelDescriptor), 11 },
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
