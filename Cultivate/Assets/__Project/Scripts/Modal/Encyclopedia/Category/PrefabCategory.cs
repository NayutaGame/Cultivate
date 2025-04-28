
using System.Collections.Generic;

public class PrefabCategory : Category<PrefabEntry>
{
    public PrefabCategory()
    {
        AddRange(new List<PrefabEntry>()
        {
            // Config Models
            new("ConfigModel缺失模型", "Prefabs/ConfigModels/缺失模型"),
            new("ConfigModel徐福", "Prefabs/ConfigModels/徐福"),
            new("ConfigModel子非鱼", "Prefabs/ConfigModels/子非鱼"),
            new("ConfigModel子非燕", "Prefabs/ConfigModels/子非燕"),
            
            // Run Models
            new("RunModel缺失模型", "Prefabs/RunModels/缺失模型"),
            new("RunModel徐福", "Prefabs/RunModels/徐福"),
            new("RunModel子非鱼", "Prefabs/RunModels/子非鱼"),
            new("RunModel子非燕", "Prefabs/RunModels/子非燕"),
            new("RunModel风雨晴", "Prefabs/RunModels/风雨晴"),
            new("RunModel梦乃遥", "Prefabs/RunModels/梦乃遥"),
            new("RunModel噬金甲", "Prefabs/RunModels/噬金甲"),
            new("RunModel墨蛟", "Prefabs/RunModels/墨蛟"),
            new("RunModel渊虾", "Prefabs/RunModels/渊虾"),
            new("RunModel九尾狐", "Prefabs/RunModels/九尾狐"),
            new("RunModel推山兽", "Prefabs/RunModels/推山兽"),
            new("RunModel白泽", "Prefabs/RunModels/白泽"),
            new("RunModel鲲", "Prefabs/RunModels/鲲"),
            new("RunModel毕方", "Prefabs/RunModels/毕方"),
            new("RunModel火蟾", "Prefabs/RunModels/火蟾"),
            new("RunModel麒麟", "Prefabs/RunModels/麒麟"),
            new("RunModel鹤仙人", "Prefabs/RunModels/鹤仙人"),
            new("RunModel鹿仙人", "Prefabs/RunModels/鹿仙人"),
            
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

    public PrefabEntry MissingConfigModel() => this["ConfigModel缺失模型"];
    public PrefabEntry MissingStageModel() => this["StageModel缺失模型"];
    public PrefabEntry MissingRunModel() => this["RunModel缺失模型"];
}
