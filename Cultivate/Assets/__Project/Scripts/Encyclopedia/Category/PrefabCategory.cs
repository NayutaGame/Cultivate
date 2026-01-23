
using System.Collections.Generic;

public class PrefabCategory : Category<PrefabEntry>
{
    public PrefabCategory()
    {
        AddRange(new List<PrefabEntry>()
        {
            // Config Models
            new("Prefab01_001", "ConfigModel缺失模型", "Prefabs/ConfigModels/缺失模型"),
            new("Prefab01_002", "ConfigModel徐福", "Prefabs/ConfigModels/徐福"),
            new("Prefab01_003", "ConfigModel子非鱼", "Prefabs/ConfigModels/子非鱼"),
            new("Prefab01_004", "ConfigModel子非燕", "Prefabs/ConfigModels/子非燕"),
            new("Prefab01_005", "ConfigModel彼此卿", "Prefabs/ConfigModels/彼此卿"),
            new("Prefab01_006", "ConfigModel风雨晴", "Prefabs/ConfigModels/风雨晴"),
            
            // Stage Models
            new("Prefab02_001", "StageModel缺失模型", "Prefabs/StageModels/缺失模型"),
            new("Prefab02_002", "StageModel徐福", "Prefabs/StageModels/徐福"),
            new("Prefab02_003", "StageModel子非鱼", "Prefabs/StageModels/子非鱼"),
            new("Prefab02_004", "StageModel子非燕", "Prefabs/StageModels/子非燕"),
            new("Prefab02_005", "StageModel风雨晴", "Prefabs/StageModels/风雨晴"),
            new("Prefab02_006", "StageModel彼此卿", "Prefabs/StageModels/彼此卿"),
            
            new("Prefab02_007", "StageModel噬金甲", "Prefabs/StageModels/噬金甲"),
            new("Prefab02_008", "StageModel墨蛟", "Prefabs/StageModels/墨蛟"),
            new("Prefab02_009", "StageModel渊虾", "Prefabs/StageModels/渊虾"),
            new("Prefab02_010", "StageModel九尾", "Prefabs/StageModels/九尾"),
            new("Prefab02_011", "StageModel推山兽", "Prefabs/StageModels/推山兽"),
            new("Prefab02_012", "StageModel白泽", "Prefabs/StageModels/白泽"),
            new("Prefab02_013", "StageModel鲲", "Prefabs/StageModels/鲲"),
            new("Prefab02_014", "StageModel毕方", "Prefabs/StageModels/毕方"),
            new("Prefab02_015", "StageModel火蟾", "Prefabs/StageModels/火蟾"),
            new("Prefab02_016", "StageModel麒麟", "Prefabs/StageModels/麒麟"),
            
            new("Prefab02_017", "StageModel醉良", "Prefabs/StageModels/醉良"),
            new("Prefab02_018", "StageModel童游", "Prefabs/StageModels/童游"),
            new("Prefab02_019", "StageModel眠谷", "Prefabs/StageModels/眠谷"),
            new("Prefab02_020", "StageModel常夏", "Prefabs/StageModels/常夏"),
            new("Prefab02_021", "StageModel司方", "Prefabs/StageModels/司方"),
            
            // Comics
            new("Prefab0043", "第一张", "Prefabs/Comics/Comic1"),
            new("Prefab0044", "第二张", "Prefabs/Comics/Comic2"),
        });
    }

    public PrefabEntry MissingConfigModel() => FromName("ConfigModel缺失模型");
    public PrefabEntry MissingStageModel() => FromName("StageModel缺失模型");
}
