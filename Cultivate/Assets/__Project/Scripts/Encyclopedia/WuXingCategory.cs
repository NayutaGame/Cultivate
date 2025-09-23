
using System.Collections.Generic;
using CLLibrary;

public class WuXingCategory : Category<WuXing>
{
    public WuXingCategory()
    {
        AddRange(new List<WuXing>()
        {
            new("WuXing0001", "金", "金行对应着锋锐。\n当角色获得锋锐时，会触发金流转，将现有的坚毅一起转化成为锋锐。", 0, true, "锋锐"),
            new("WuXing0002", "水", "水行对应着格挡。\n当角色获得格挡时，会触发水流转，将现有的锋锐一起转化成为格挡。", 1, true, "格挡"),
            new("WuXing0003", "木", "木行对应着力量。\n当角色获得力量时，会触发木流转，将现有的格挡一起转化成为力量。", 2, true, "力量"),
            new("WuXing0004", "火", "火行对应着灼烧。\n当角色获得灼烧时，会触发火流转，将现有的力量一起转化成为灼烧。", 3, true, "灼烧"),
            new("WuXing0005", "土", "土行对应着坚毅。\n当角色获得坚毅时，会触发土流转，将现有的灼烧一起转化成为坚毅。", 4, true, "坚毅"),
            new("WuXing0006", "无色", "无色，没有对应的基础Buff", 5, false, null),
        });
    }
}