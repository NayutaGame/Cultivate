
public class SkillType : SuperEnum<SkillType>
{
    private SkillType(int index, int value, string name) : base(index, value, name)
    {
    }
    
    public static void Init()
    {
        _list = new SkillType[]
        {
            new(0, 1 << 0, "攻击"),
            new(1, 1 << 1, "防御"),
            new(2, 1 << 2, "灵气"),
            new(3, 1 << 3, "治疗"),
            new(4, 1 << 4, "二动"),
            new(5, 1 << 5, "消耗"),
            new(6, 1 << 6, "损耗"),
            new(7, 1 << 7, "自指"),
        };
    }

    public static SkillType Attack   => _list[0];
    public static SkillType Defend   => _list[1];
    public static SkillType LingQi   => _list[2];
    public static SkillType ZhiLiao  => _list[3];
    public static SkillType ErDong   => _list[4];
    public static SkillType XiaoHao  => _list[5];
    public static SkillType SunHao   => _list[6];
    public static SkillType ZiZhi    => _list[7];

    public static implicit operator int(SkillType skillType) => skillType._value;
}
