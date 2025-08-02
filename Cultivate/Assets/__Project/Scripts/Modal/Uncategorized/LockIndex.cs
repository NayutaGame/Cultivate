
using System;

public readonly struct LockIndex : IEquatable<LockIndex>
{
    // 锁类型
    public enum LockType
    {
        Character,   // 角色锁
        Slot,       // 卡槽锁
        Pack        // 卡包锁
    }

    private readonly LockType _type;
    private readonly string _id;    // 主ID（角色ID或卡包ID）
    private readonly int _secondaryId;     // 次ID（槽位索引）

    public LockType Type => _type;
    public string ID => _id;
    public int SecondaryId => _secondaryId;

    // 构造函数们
    private LockIndex(LockType type, string id, int secondaryId = -1)
    {
        _type = type;
        _id = id;
        _secondaryId = secondaryId;
    }

    // 工厂方法
    public static LockIndex FromCharacter(string characterName)
        => new(LockType.Character, characterName);
    public static LockIndex FromCharacter(CharacterEntry character)
        => new(LockType.Character, character.GetName());

    public static LockIndex FromSlot(string characterName, int slotIndex)
        => new(LockType.Slot, characterName, slotIndex);
    public static LockIndex FromSlot(CharacterEntry character, int slotIndex)
        => new(LockType.Slot, character.GetName(), slotIndex);

    public static LockIndex FromPack(string packName)
        => new(LockType.Pack, packName);
    public static LockIndex FromPack(PackEntry pack)
        => new(LockType.Pack, pack.GetName());

    // 相等性比较
    public bool Equals(LockIndex other)
        => _type == other._type && 
           _id == other._id && 
           _secondaryId == other._secondaryId;

    public override bool Equals(object obj)
        => obj is LockIndex other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(_type, _id, _secondaryId);

    // 字符串表示
    public override string ToString()
    {
        return _type switch
        {
            LockType.Character => $"Character:{_id}",
            LockType.Slot => $"Slot:{_id}:{_secondaryId}",
            LockType.Pack => $"Pack:{_id}",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
