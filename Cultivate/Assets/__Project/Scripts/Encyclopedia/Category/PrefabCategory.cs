
using System.Collections.Generic;

public class PrefabCategory : Category<PrefabEntry>
{
    public PrefabCategory()
    {
        AddRange(new List<PrefabEntry>()
        {
            // Config Models
            new("Prefab01_001", "ConfigModel缺失", "Prefabs/ConfigModels/缺失"),
            new("Prefab01_002", "ConfigModel徐福", "Prefabs/ConfigModels/徐福"),
            new("Prefab01_003", "ConfigModel子非鱼", "Prefabs/ConfigModels/子非鱼"),
            new("Prefab01_004", "ConfigModel子非燕", "Prefabs/ConfigModels/子非燕"),
            new("Prefab01_005", "ConfigModel彼此卿", "Prefabs/ConfigModels/彼此卿"),
            new("Prefab01_006", "ConfigModel风雨晴", "Prefabs/ConfigModels/风雨晴"),
            // new("Prefab01_007", "ConfigModel梦乃遥", "Prefabs/ConfigModels/梦乃遥"),
            
            // Run Models
            new("Prefab02_001", "RunModel缺失模型", "Prefabs/RunModels/缺失"),
            new("Prefab02_002", "RunModel徐福", "Prefabs/RunModels/徐福"),
            new("Prefab02_003", "RunModel子非鱼", "Prefabs/RunModels/子非鱼"),
            new("Prefab02_004", "RunModel子非燕", "Prefabs/RunModels/子非燕"),
            // new("Prefab02_005", "RunModel彼此卿", "Prefabs/RunModels/彼此卿"),
            new("Prefab02_006", "RunModel风雨晴", "Prefabs/RunModels/风雨晴"),
            // new("Prefab02_007", "RunModel梦乃遥", "Prefabs/RunModels/梦乃遥"),
            
            new("Prefab02_008", "RunModel噬金甲", "Prefabs/RunModels/噬金甲"),
            new("Prefab02_009", "RunModel墨蛟", "Prefabs/RunModels/墨蛟"),
            new("Prefab02_010", "RunModel渊虾", "Prefabs/RunModels/渊虾"),
            new("Prefab02_011", "RunModel九尾狐", "Prefabs/RunModels/九尾狐"),
            new("Prefab02_012", "RunModel推山兽", "Prefabs/RunModels/推山兽"),
            new("Prefab02_013", "RunModel白泽", "Prefabs/RunModels/白泽"),
            new("Prefab02_014", "RunModel鲲", "Prefabs/RunModels/鲲"),
            new("Prefab02_015", "RunModel毕方", "Prefabs/RunModels/毕方"),
            new("Prefab02_016", "RunModel火蟾", "Prefabs/RunModels/火蟾"),
            new("Prefab02_017", "RunModel麒麟", "Prefabs/RunModels/麒麟"),
            // new("Prefab02_018", "RunModel醉良", "Prefabs/RunModels/醉良"),
            // new("Prefab02_019", "RunModel童游", "Prefabs/RunModels/童游"),
            // new("Prefab02_020", "RunModel眠谷", "Prefabs/RunModels/眠谷"),
            // new("Prefab02_021", "RunModel常夏", "Prefabs/RunModels/常夏"),
            // new("Prefab02_022", "RunModel司方", "Prefabs/RunModels/司方"),
            
            new("Prefab02_023", "RunModel鹤仙人", "Prefabs/RunModels/鹤仙人"),
            new("Prefab02_024", "RunModel鹿仙人", "Prefabs/RunModels/鹿仙人"),
            
            // Stage Models
            new("Prefab03_001", "StageModel缺失", "Prefabs/StageModels/缺失"),
            new("Prefab03_002", "StageModel徐福", "Prefabs/StageModels/徐福"),
            new("Prefab03_003", "StageModel子非鱼", "Prefabs/StageModels/子非鱼"),
            new("Prefab03_004", "StageModel子非燕", "Prefabs/StageModels/子非燕"),
            new("Prefab03_005", "StageModel彼此卿", "Prefabs/StageModels/彼此卿"),
            new("Prefab03_006", "StageModel风雨晴", "Prefabs/StageModels/风雨晴"),
            // new("Prefab03_007", "StageModel梦乃遥", "Prefabs/StageModels/梦乃遥"),
            
            new("Prefab03_008", "StageModel噬金甲", "Prefabs/StageModels/噬金甲"),
            new("Prefab03_009", "StageModel墨蛟", "Prefabs/StageModels/墨蛟"),
            new("Prefab03_010", "StageModel渊虾", "Prefabs/StageModels/渊虾"),
            new("Prefab03_011", "StageModel九尾狐", "Prefabs/StageModels/九尾狐"),
            new("Prefab03_012", "StageModel推山兽", "Prefabs/StageModels/推山兽"),
            new("Prefab03_013", "StageModel白泽", "Prefabs/StageModels/白泽"),
            new("Prefab03_014", "StageModel鲲", "Prefabs/StageModels/鲲"),
            new("Prefab03_015", "StageModel毕方", "Prefabs/StageModels/毕方"),
            new("Prefab03_016", "StageModel火蟾", "Prefabs/StageModels/火蟾"),
            new("Prefab03_017", "StageModel麒麟", "Prefabs/StageModels/麒麟"),
            new("Prefab03_018", "StageModel醉良", "Prefabs/StageModels/醉良"),
            new("Prefab03_019", "StageModel童游", "Prefabs/StageModels/童游"),
            new("Prefab03_020", "StageModel眠谷", "Prefabs/StageModels/眠谷"),
            new("Prefab03_021", "StageModel常夏", "Prefabs/StageModels/常夏"),
            new("Prefab03_022", "StageModel司方", "Prefabs/StageModels/司方"),
            
            new("Prefab03_023", "StageModel鹤仙人", "Prefabs/StageModels/鹤仙人"),
            new("Prefab03_024", "StageModel鹿仙人", "Prefabs/StageModels/鹿仙人"),
            
            // Narrative Models
            new("Prefab04_001", "NarrativeModel缺失", "Prefabs/NarrativeModels/缺失"),
            new("Prefab04_002", "NarrativeModel徐福", "Prefabs/NarrativeModels/徐福"),
            new("Prefab04_003", "NarrativeModel子非鱼", "Prefabs/NarrativeModels/子非鱼"),
            new("Prefab04_004", "NarrativeModel子非燕", "Prefabs/NarrativeModels/子非燕"),
            new("Prefab04_005", "NarrativeModel彼此卿", "Prefabs/NarrativeModels/彼此卿"),
            new("Prefab04_006", "NarrativeModel风雨晴", "Prefabs/NarrativeModels/风雨晴"),
            // new("Prefab04_007", "NarrativeModel梦乃遥", "Prefabs/NarrativeModels/梦乃遥"),
            
            new("Prefab04_008", "NarrativeModel噬金甲", "Prefabs/NarrativeModels/噬金甲"),
            new("Prefab04_009", "NarrativeModel墨蛟", "Prefabs/NarrativeModels/墨蛟"),
            new("Prefab04_010", "NarrativeModel渊虾", "Prefabs/NarrativeModels/渊虾"),
            new("Prefab04_011", "NarrativeModel九尾狐", "Prefabs/NarrativeModels/九尾狐"),
            new("Prefab04_012", "NarrativeModel推山兽", "Prefabs/NarrativeModels/推山兽"),
            new("Prefab04_013", "NarrativeModel白泽", "Prefabs/NarrativeModels/白泽"),
            new("Prefab04_014", "NarrativeModel鲲", "Prefabs/NarrativeModels/鲲"),
            new("Prefab04_015", "NarrativeModel毕方", "Prefabs/NarrativeModels/毕方"),
            new("Prefab04_016", "NarrativeModel火蟾", "Prefabs/NarrativeModels/火蟾"),
            new("Prefab04_017", "NarrativeModel麒麟", "Prefabs/NarrativeModels/麒麟"),
            new("Prefab04_018", "NarrativeModel醉良", "Prefabs/NarrativeModels/醉良"),
            new("Prefab04_019", "NarrativeModel童游", "Prefabs/NarrativeModels/童游"),
            new("Prefab04_020", "NarrativeModel眠谷", "Prefabs/NarrativeModels/眠谷"),
            new("Prefab04_021", "NarrativeModel常夏", "Prefabs/NarrativeModels/常夏"),
            new("Prefab04_022", "NarrativeModel司方", "Prefabs/NarrativeModels/司方"),
            
            new("Prefab04_023", "NarrativeModel鹤仙人", "Prefabs/NarrativeModels/鹤仙人"),
            new("Prefab04_024", "NarrativeModel鹿仙人", "Prefabs/NarrativeModels/鹿仙人"),
            
            // Scribble Models
            new("Prefab05_001", "ScribbleModel徐福", "Prefabs/ScribbleModels/缺失"),
            new("Prefab05_002", "ScribbleModel徐福", "Prefabs/ScribbleModels/徐福"),
            new("Prefab05_003", "ScribbleModel子非鱼", "Prefabs/ScribbleModels/子非鱼"),
            new("Prefab05_004", "ScribbleModel子非燕", "Prefabs/ScribbleModels/子非燕"),
            new("Prefab05_005", "ScribbleModel彼此卿", "Prefabs/ScribbleModels/彼此卿"),
            new("Prefab05_006", "ScribbleModel风雨晴", "Prefabs/ScribbleModels/风雨晴"),
            
            // Comics
            new("Prefab0043", "第一张", "Prefabs/Comics/Comic1"),
            new("Prefab0044", "第二张", "Prefabs/Comics/Comic2"),
        });
    }

    public PrefabEntry MissingConfigModel() => FromName("ConfigModel缺失");
    public PrefabEntry MissingRunModel() => FromName("RunModel缺失");
    public PrefabEntry MissingStageModel() => FromName("StageModel缺失");
    public PrefabEntry MissingScribbleModel() => FromName("ScribbleModel缺失");
}
