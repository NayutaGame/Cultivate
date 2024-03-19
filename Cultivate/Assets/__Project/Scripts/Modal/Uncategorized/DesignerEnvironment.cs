
using System.Threading.Tasks;
using CLLibrary;

public class DesignerEnvironment
{
    public static DesignerConfig GetDesignerConfig()
        => new(runEventDescriptors: new[] { CustomMap, StandardSkillPool, StandardRun });

    private static readonly RunEventDescriptor StandardMap =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            Map map = env.Map;
            bool firstTime = true;
            NodeEntry firstNode = firstTime ? "初入蓬莱" : null;

            map.EntityPool = new();
            map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());

            map.AdventurePool = new();
            map.AdventurePool.Populate(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is AdventureNodeEntry));
            
            map.DrawDescriptors = new DrawDescriptor[][]
            {
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Adventure, firstNode),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Boss),
                },
            };

            map.SetLevel(0);
        });

    private static readonly RunEventDescriptor CustomMap =
        new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
        {
            RunEnvironment env = (RunEnvironment)listener;
            RunDetails d = (RunDetails)eventDetails;

            Map map = env.Map;

            map.EntityPool = new();
            map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());

            map.AdventurePool = new();
            map.AdventurePool.Populate(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is AdventureNodeEntry));
            
            map.DrawDescriptors = new DrawDescriptor[][]
            {
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Boss),
                    new(DrawDescriptor.NodeType.Ascension),
                },
                new DrawDescriptor[] {
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Adventure),
                    new(DrawDescriptor.NodeType.Battle),
                    new(DrawDescriptor.NodeType.Rest),
                    new(DrawDescriptor.NodeType.Shop),
                    new(DrawDescriptor.NodeType.Boss),
                },
            };

            map.SetLevel(0);
        });

    // private static readonly RunEventDescriptor WeplayMap =
    //     new(RunEventDict.RUN_ENVIRONMENT, RunEventDict.START_RUN, -4, (listener, eventDetails) =>
    //     {
    //         RunEnvironment env = (RunEnvironment)listener;
    //         RunDetails d = (RunDetails)eventDetails;
    //
    //         Map map = env.Map;
    //         map.EntityPool = new();
    //         map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
    //         map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is BattleNodeEntry).ToList());
    //         map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is RewardNodeEntry).ToList());
    //         map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool && e is AdventureNodeEntry).ToList());
    //
    //         map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
    //         {
    //             // 悟道 回复命元 获得金钱 提升境界 加生命上限 商店 以物易物
    //             { JingJie.LianQi   , new NodeEntry[] { "初入蓬莱", null, null, null, null, null, null, null, null, null } },
    //             { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, "同境界合成教学", null, null, null, null, null, null } },
    //             { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, "愿望单" } },
    //             { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
    //             { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
    //             { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
    //         };
    //
    //         map._normalPools = new Dictionary<JingJie, CyclicPool<NodeEntry>[]>()
    //         {
    //             { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //             { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //             { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //             { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //             { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //             { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
    //         };
    //     });

    private static readonly RunEventDescriptor StandardSkillPool =
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

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        if (d.Owner != owner)
            return;
        // await owner.LoseArmorProcedure(owner.Armor);
    }
}
