
public class SkillType : SuperEnum<SkillType>
{
    private SkillType(int index, string name) : base(index, name) { }

    public static void Init()
    {
        _list = new SkillType[]
        {
            new(1 << 0, "攻击"),
            new(1 << 1, "剑阵"),
            new(1 << 2, "灵气"),
            new(1 << 3, "二动"),
            new(1 << 4, "消耗"),
        };
    }

    public static SkillType Attack   => _list[0];
    public static SkillType JianZhen => _list[1];
    public static SkillType LingQi   => _list[2];
    public static SkillType ErDong   => _list[3];
    public static SkillType XiaoHao  => _list[4];

    public static implicit operator int(SkillType skillType) => skillType._index;
    // public static implicit operator SkillType(int index) => _list[index];
}
