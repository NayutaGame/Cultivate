
public class MechType : SuperEnum<MechType>
{
    private MechType(int index, string name) : base(index, name) { }

    public static void Init()
    {
        _list = new MechType[]
        {
            new(1 << 0, "香"),
            new(1 << 2, "刃"),
            new(1 << 4, "匣"),
            new(1 << 6, "轮"),
        };
    }

    public static MechType Xiang => _list[0];
    public static MechType Ren => _list[1];
    public static MechType Xia => _list[2];
    public static MechType Lun => _list[3];

    public static implicit operator int(MechType mechType) => mechType._index;
    // public static implicit operator MechType(int index) => _list[index];
}
