
using System.Collections.Generic;

public class PrefabCategory : Category<PrefabEntry>
{
    public PrefabCategory()
    {
        AddRange(new List<PrefabEntry>()
        {
            // Config Models
            new("Prefab0001", "ConfigModel缺失模型", "Prefabs/ConfigModels/缺失模型"),
            new("Prefab0002", "ConfigModel徐福", "Prefabs/ConfigModels/徐福"),
            new("Prefab0003", "ConfigModel子非鱼", "Prefabs/ConfigModels/子非鱼"),
            new("Prefab0004", "ConfigModel子非燕", "Prefabs/ConfigModels/子非燕"),
            new("Prefab0005", "ConfigModel彼此卿", "Prefabs/ConfigModels/彼此卿"),
            
            // Run Models
            new("Prefab0006", "RunModel缺失模型", "Prefabs/RunModels/缺失模型"),
            new("Prefab0007", "RunModel徐福", "Prefabs/RunModels/徐福"),
            new("Prefab0008", "RunModel子非鱼", "Prefabs/RunModels/子非鱼"),
            new("Prefab0009", "RunModel子非燕", "Prefabs/RunModels/子非燕"),
            new("Prefab0010", "RunModel风雨晴", "Prefabs/RunModels/风雨晴"),
            new("Prefab0011", "RunModel梦乃遥", "Prefabs/RunModels/梦乃遥"),
            new("Prefab0012", "RunModel噬金甲", "Prefabs/RunModels/噬金甲"),
            new("Prefab0013", "RunModel墨蛟", "Prefabs/RunModels/墨蛟"),
            new("Prefab0014", "RunModel渊虾", "Prefabs/RunModels/渊虾"),
            new("Prefab0015", "RunModel九尾狐", "Prefabs/RunModels/九尾狐"),
            new("Prefab0016", "RunModel推山兽", "Prefabs/RunModels/推山兽"),
            new("Prefab0017", "RunModel白泽", "Prefabs/RunModels/白泽"),
            new("Prefab0018", "RunModel鲲", "Prefabs/RunModels/鲲"),
            new("Prefab0019", "RunModel毕方", "Prefabs/RunModels/毕方"),
            new("Prefab0020", "RunModel火蟾", "Prefabs/RunModels/火蟾"),
            new("Prefab0021", "RunModel麒麟", "Prefabs/RunModels/麒麟"),
            new("Prefab0022", "RunModel鹤仙人", "Prefabs/RunModels/鹤仙人"),
            new("Prefab0023", "RunModel鹿仙人", "Prefabs/RunModels/鹿仙人"),
            
            // Stage Models
            new("Prefab0024", "StageModel缺失模型", "Prefabs/StageModels/缺失模型"),
            new("Prefab0025", "StageModel徐福", "Prefabs/StageModels/徐福"),
            new("Prefab0026", "StageModel子非鱼", "Prefabs/StageModels/子非鱼"),
            new("Prefab0027", "StageModel子非燕", "Prefabs/StageModels/子非燕"),
            new("Prefab0028", "StageModel风雨晴", "Prefabs/StageModels/风雨晴"),
            new("Prefab0029", "StageModel彼此卿", "Prefabs/StageModels/彼此卿"),
            new("Prefab0030", "StageModel梦乃遥", "Prefabs/StageModels/梦乃遥"),
            
            new("Prefab0031", "StageModel噬金甲", "Prefabs/StageModels/噬金甲"),
            new("Prefab0032", "StageModel墨蛟", "Prefabs/StageModels/墨蛟"),
            new("Prefab0033", "StageModel渊虾", "Prefabs/StageModels/渊虾"),
            new("Prefab0034", "StageModel九尾狐", "Prefabs/StageModels/九尾狐"),
            new("Prefab0035", "StageModel推山兽", "Prefabs/StageModels/推山兽"),
            new("Prefab0036", "StageModel白泽", "Prefabs/StageModels/白泽"),
            new("Prefab0037", "StageModel鲲", "Prefabs/StageModels/鲲"),
            new("Prefab0038", "StageModel毕方", "Prefabs/StageModels/毕方"),
            new("Prefab0039", "StageModel火蟾", "Prefabs/StageModels/火蟾"),
            new("Prefab0040", "StageModel麒麟", "Prefabs/StageModels/麒麟"),
            new("Prefab0041", "StageModel鹤仙人", "Prefabs/StageModels/鹤仙人"),
            new("Prefab0042", "StageModel鹿仙人", "Prefabs/StageModels/鹿仙人"),
            
            // Comics
            new("Prefab0043", "第一张", "Prefabs/Comics/Comic1"),
            new("Prefab0044", "第二张", "Prefabs/Comics/Comic2"),
        });
    }

    public PrefabEntry MissingConfigModel() => FromName("ConfigModel缺失模型");
    public PrefabEntry MissingStageModel() => FromName("StageModel缺失模型");
    public PrefabEntry MissingRunModel() => FromName("RunModel缺失模型");
}
