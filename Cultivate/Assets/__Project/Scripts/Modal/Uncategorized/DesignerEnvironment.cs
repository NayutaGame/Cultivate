
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;

public class DesignerEnvironment
{
    public static DesignerConfig GetDesignerConfig()
        => new(runEventDescriptors: new[] { WeplayConfigMap, ConfigSkillPool, WeplayRun });

    private static readonly RunEventDescriptor StandardConfigMap =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            Map map = env.Map;
            bool firstTime = true;

            map.EntityPool = new();
            map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
            map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is BattleNodeEntry).ToList());
            map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is RewardNodeEntry).ToList());
            map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is AdventureNodeEntry).ToList());

            map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
            {
                { JingJie.LianQi   , new NodeEntry[] { firstTime ? "初入蓬莱" : null, null, null, null, null, null, null, null, null, null } },
                { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            };

            map._normalPools = new Dictionary<JingJie, CyclicPool<NodeEntry>[]>()
            {
                { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            };
        });

    private static readonly RunEventDescriptor CustomConfigMap =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            Map map = env.Map;
            bool firstTime = true;
            bool exhibitionVersion = true;

            map.EntityPool = new();
            map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
            map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is BattleNodeEntry).ToList());
            map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is RewardNodeEntry).ToList());
            map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.WithInPool && n is AdventureNodeEntry).ToList());

            map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
            {
                { JingJie.LianQi   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                // { JingJie.LianQi   , new NodeEntry[] { Encyclopedia.NodeCategory["天机阁"], null, null, null, null, null, null, null, null, null } }, // Arbitrary
                // { JingJie.LianQi   , new NodeEntry[] { Encyclopedia.NodeCategory["以物易物"], null, null, null, null, null, null, null, null, null } }, // Barter
                // { JingJie.LianQi   , new NodeEntry[] { Encyclopedia.NodeCategory["提升境界"], null, null, null, null, null, null, null, null, null } }, // CardPickerPanel
                // { JingJie.LianQi   , new NodeEntry[] { Encyclopedia.NodeCategory["照相机"], null, null, null, null, null, null, null, null, null } }, // Dialog
                // { JingJie.LianQi   , new NodeEntry[] { Encyclopedia.NodeCategory["商店"], null, null, null, null, null, null, null, null, null } }, // Shop
                { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, exhibitionVersion ? "愿望单" : null } },
                { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            };

            // map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
            // {
            //     { JingJie.LianQi   , new NodeEntry[] { firstTime ? "初入蓬莱" : null, null, null, null, null, null, null, null, null, null } },
            //     { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            //     { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, exhibitionVersion ? "愿望单" : null } },
            //     { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            //     { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            //     { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            // };

            map._normalPools = new Dictionary<JingJie, CyclicPool<NodeEntry>[]>()
            {
                { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._r, map._b } },
                { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            };
        });

    private static readonly RunEventDescriptor WeplayConfigMap =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            Map map = env.Map;
            map.EntityPool = new();
            map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
            map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is BattleNodeEntry).ToList());
            map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is RewardNodeEntry).ToList());
            map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is AdventureNodeEntry).ToList());

            map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
            {
                // 悟道 回复命元 获得金钱 提升境界 加生命上限 商店 以物易物
                { JingJie.LianQi   , new NodeEntry[] { "初入蓬莱", null, null, null, null, null, null, null, null, null } },
                { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, "同境界合成教学", null, null, null, null, null, null } },
                { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, "愿望单" } },
                { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
                { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            };

            map._normalPools = new Dictionary<JingJie, CyclicPool<NodeEntry>[]>()
            {
                { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
                { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            };
        });

    private static readonly RunEventDescriptor ConfigSkillPool =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -3, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            4.Do(i => env.SkillPool.Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e.WithinPool)));
        });

    private static readonly RunEventDescriptor StandardRun =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -1, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            env.SetJingJieProcedure(JingJie.LianQi);
            env.SetDGold(50);
            env.ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
        });

    private static readonly RunEventDescriptor CustomRun =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -1, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            env.SetJingJieProcedure(JingJie.HuaShen);
            env.ForceDrawSkills(jingJie: JingJie.HuaShen, count: 12);
        });

    private static readonly RunEventDescriptor WeplayRun =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -1, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            env.SetJingJieProcedure(JingJie.LianQi);
            env.SetDGold(50);
        });

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        // if (d.Owner == owner)
        //     await owner.ArmorLoseSelfProcedure(owner.Armor);
    }
}
