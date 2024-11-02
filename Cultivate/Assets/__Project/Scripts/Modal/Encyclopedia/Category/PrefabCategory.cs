
using System.Collections.Generic;

public class PrefabCategory : Category<PrefabEntry>
{
    public PrefabCategory()
    {
        AddRange(new List<PrefabEntry>()
        {
            // UI Entity Models
            new("UIEntityModel缺失模型", "Prefabs/UIEntityModels/缺失模型"),
            new("UIEntityModel徐福", "Prefabs/UIEntityModels/徐福"),
            new("UIEntityModel子非鱼", "Prefabs/UIEntityModels/子非鱼"),
            new("UIEntityModel子非燕", "Prefabs/UIEntityModels/子非燕"),
            new("UIEntityModel风雨晴", "Prefabs/UIEntityModels/风雨晴"),
            new("UIEntityModel梦乃遥", "Prefabs/UIEntityModels/梦乃遥"),
            new("UIEntityModel噬金甲", "Prefabs/UIEntityModels/噬金甲"),
            new("UIEntityModel墨蛟", "Prefabs/UIEntityModels/墨蛟"),
            new("UIEntityModel推山兽", "Prefabs/UIEntityModels/推山兽"),
            new("UIEntityModel毕方", "Prefabs/UIEntityModels/毕方"),
            new("UIEntityModel渊虾", "Prefabs/UIEntityModels/渊虾"),
            new("UIEntityModel火蟾", "Prefabs/UIEntityModels/火蟾"),
            new("UIEntityModel白泽", "Prefabs/UIEntityModels/白泽"),
            new("UIEntityModel鲲", "Prefabs/UIEntityModels/鲲"),
            new("UIEntityModel麒麟", "Prefabs/UIEntityModels/麒麟"),
            new("UIEntityModel鹤仙人", "Prefabs/UIEntityModels/鹤仙人"),
            new("UIEntityModel鹿仙人", "Prefabs/UIEntityModels/鹿仙人"),
        });
    }

    public PrefabEntry MissingStageModel() => this["UIEntityModel缺失模型"];
    public PrefabEntry MissingUIEntityModel() => this["UIEntityModel缺失模型"];
}
