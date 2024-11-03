
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
            new("UIEntityModel渊虾", "Prefabs/UIEntityModels/渊虾"),
            new("UIEntityModel九尾狐", "Prefabs/UIEntityModels/九尾狐"),
            new("UIEntityModel推山兽", "Prefabs/UIEntityModels/推山兽"),
            new("UIEntityModel白泽", "Prefabs/UIEntityModels/白泽"),
            new("UIEntityModel鲲", "Prefabs/UIEntityModels/鲲"),
            new("UIEntityModel毕方", "Prefabs/UIEntityModels/毕方"),
            new("UIEntityModel火蟾", "Prefabs/UIEntityModels/火蟾"),
            new("UIEntityModel麒麟", "Prefabs/UIEntityModels/麒麟"),
            new("UIEntityModel鹤仙人", "Prefabs/UIEntityModels/鹤仙人"),
            new("UIEntityModel鹿仙人", "Prefabs/UIEntityModels/鹿仙人"),
            
            // Stage Models
            new("StageModel缺失模型", "Prefabs/StageModels/缺失模型"),
            new("StageModel徐福", "Prefabs/StageModels/徐福"),
            new("StageModel子非鱼", "Prefabs/StageModels/子非鱼"),
            new("StageModel子非燕", "Prefabs/StageModels/子非燕"),
            new("StageModel风雨晴", "Prefabs/StageModels/风雨晴"),
            new("StageModel梦乃遥", "Prefabs/StageModels/梦乃遥"),
            
            new("StageModel噬金甲", "Prefabs/StageModels/噬金甲"),
            new("StageModel墨蛟", "Prefabs/StageModels/墨蛟"),
            new("StageModel渊虾", "Prefabs/StageModels/渊虾"),
            new("StageModel九尾狐", "Prefabs/StageModels/九尾狐"),
            new("StageModel推山兽", "Prefabs/StageModels/推山兽"),
            new("StageModel白泽", "Prefabs/StageModels/白泽"),
            new("StageModel鲲", "Prefabs/StageModels/鲲"),
            new("StageModel毕方", "Prefabs/StageModels/毕方"),
            new("StageModel火蟾", "Prefabs/StageModels/火蟾"),
            new("StageModel麒麟", "Prefabs/StageModels/麒麟"),
            new("StageModel鹤仙人", "Prefabs/StageModels/鹤仙人"),
            new("StageModel鹿仙人", "Prefabs/StageModels/鹿仙人"),
            
            // Comics
            new("第一张", "Prefabs/Comics/Comic1"),
            new("第二张", "Prefabs/Comics/Comic2"),
        });
    }

    public PrefabEntry MissingStageModel() => this["StageModel缺失模型"];
    public PrefabEntry MissingUIEntityModel() => this["UIEntityModel缺失模型"];
}
