
using System;

[Flags]
public enum TagType
{
    None = 0,
    Attack = 1 << 0,
    Defend = 1 << 1,
    Mana = 1 << 2,
    Health = 1 << 3,
    Swift = 1 << 4,
    Growth = 1 << 5,
    Exhaust = 1 << 6,
    Deplete = 1 << 7,
}
