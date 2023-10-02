
using System.Collections.Generic;

public class EntityCategory : Category<EntityEntry>
{
    public EntityCategory()
    {
         // AddRange(new List<EntityEntry>()
         // {
         //     new("鶸", "除了聚气什么都不会的废物"),
         // });

        AddRange(new List<EntityEntry>()
        {
            new("鶸", "除了聚气什么都不会的废物"),
            new("噬金甲", "普通金系减甲"),
            new("墨蛟", "普通水系吸血"),
            new("渊虾", "普通木系穿透"),
            new("九尾狐", "普通火系多段"),
            new("推山兽", "普通土系高伤"),
            new("白泽", "精英金系锋锐"),
            new("鲲", "精英水系格挡"),
            new("毕方", "精英木系闪避"),
            new("火蟾", "精英火系灼烧"),
            new("麒麟", "精英土系加甲"),
            new("凌霄大圣", "Boss金系"),
            new("龟仙人", "Boss水系"),
            new("鹤仙人", "Boss木系"),
            new("鹿仙人", "Boss火系"),
            new("土行大圣", "Boss土系"),
        });
    }

    public override EntityEntry DefaultEntry() => this["鶸"];
}
