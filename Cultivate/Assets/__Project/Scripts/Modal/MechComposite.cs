
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public class MechComposite
{
    private static readonly int MAX_CHAIN = 3;

    private MechType[] _mechTypes;

    public MechComposite()
    {
        _mechTypes = new MechType[MAX_CHAIN];
    }

    private static Dictionary<int, SkillEntry> _composeDict = new()
    {
        { 0, null },

        { MechType.Xiang, "醒神香" },
        { MechType.Ren, "飞镖" },
        { MechType.Xia, "铁匣" },
        { MechType.Lun, "滑索" },

        { MechType.Xiang + MechType.Xiang, "还魂香" },
        { MechType.Xiang + MechType.Ren, "净魂刀" },
        { MechType.Xiang + MechType.Xia, "防护罩" },
        { MechType.Xiang + MechType.Lun, "能量饮料" },
        { MechType.Ren + MechType.Ren, "炎铳" },
        { MechType.Ren + MechType.Xia, "机关人偶" },
        { MechType.Ren + MechType.Lun, "铁陀螺" },
        { MechType.Xia + MechType.Xia, "防壁" },
        { MechType.Xia + MechType.Lun, "不倒翁" },
        { MechType.Lun + MechType.Lun, "助推器" },

        { MechType.Xiang + MechType.Xiang + MechType.Xiang, "反应堆" },
        { MechType.Xiang + MechType.Xiang + MechType.Ren, "烟花" },
        { MechType.Xiang + MechType.Xiang + MechType.Xia, "长明灯" },
        { MechType.Xiang + MechType.Xiang + MechType.Lun, "大往生香" },
        { MechType.Lun + MechType.Xiang + MechType.Ren, "地府通讯器" },

        { MechType.Ren + MechType.Ren + MechType.Ren, "无人机阵列" },
        { MechType.Ren + MechType.Ren + MechType.Xiang, "弩炮" },
        { MechType.Ren + MechType.Ren + MechType.Xia, "尖刺陷阱" },
        { MechType.Ren + MechType.Ren + MechType.Lun, "暴雨梨花针" },
        { MechType.Xiang + MechType.Ren + MechType.Xia, "炼丹炉" },

        { MechType.Xia + MechType.Xia + MechType.Xia, "浮空艇" },
        { MechType.Xia + MechType.Xia + MechType.Xiang, "动量中和器" },
        { MechType.Xia + MechType.Xia + MechType.Ren, "机关伞" },
        { MechType.Xia + MechType.Xia + MechType.Lun, "一轮马" },
        { MechType.Ren + MechType.Xia + MechType.Lun, "外骨骼" },

        { MechType.Lun + MechType.Lun + MechType.Lun, "永动机" },
        { MechType.Lun + MechType.Lun + MechType.Xiang, "火箭靴" },
        { MechType.Lun + MechType.Lun + MechType.Ren, "定龙桩" },
        { MechType.Lun + MechType.Lun + MechType.Xia, "飞行器" },
        { MechType.Xia + MechType.Lun + MechType.Xiang, "时光机" },
    };

    private SkillEntry GetComposed()
        => _composeDict[_mechTypes.Map(t => t ?? 0).Sum()];
}
