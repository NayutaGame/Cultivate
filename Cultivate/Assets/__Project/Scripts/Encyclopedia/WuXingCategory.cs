
using System.Collections.Generic;
using CLLibrary;

public class WuXingCategory : Category<WuXing>
{
    public WuXingCategory()
    {
        AddRange(new List<WuXing>()
        {
            new("WuXing0001", "金", 0, true, "锋锐"),
            new("WuXing0002", "水", 1, true, "格挡"),
            new("WuXing0003", "木", 2, true, "力量"),
            new("WuXing0004", "火", 3, true, "灼烧"),
            new("WuXing0005", "土", 4, true, "坚毅"),
            new("WuXing0006", "无色", 5, false, null),
        });
    }
}