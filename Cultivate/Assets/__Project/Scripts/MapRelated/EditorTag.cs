
using System;

[Flags]
public enum EditorTag
{
    无 = 0,
    攻击 = 1 << 0,
    防御 = 1 << 1,
    灵气 = 1 << 2,
    气血 = 1 << 3,
    二动 = 1 << 4,
    成长 = 1 << 5,
    升华 = 1 << 6,
    一次性 = 1 << 7,
}
