
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;

public class DesignerEnvironment
{
    public static RunConfig GetConfig()
    {
        // return new RunConfig(StandardInitMapPools, Standard);
        return new RunConfig(CustomInitMapPools, Standard);
    }

    private static void StandardInitMapPools(Map map)
    {
        map.EntityPool = new();
        map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
        map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is BattleNodeEntry).ToList());
        map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is RewardNodeEntry).ToList());
        map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is AdventureNodeEntry).ToList());

        map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
        {
            { JingJie.LianQi   , new NodeEntry[] { null/*Tutorial*/, null, null, null, null, null, null, null, null, null } },
            { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null/*Wishlist*/ } },
            { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
        };

        map._normalPools = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
        };
    }

    private static void CustomInitMapPools(Map map)
    {
        bool firstTime = true;
        bool exhibitionVersion = true;

        map.EntityPool = new();
        map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
        map._b = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is BattleNodeEntry).ToList());
        map._r = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is RewardNodeEntry).ToList());
        map._a = new(Encyclopedia.NodeCategory.Traversal.FilterObj(n => n.Normal && n is AdventureNodeEntry).ToList());

        map._priorityNodes = new Dictionary<JingJie, NodeEntry[]>()
        {
            { JingJie.LianQi   , new NodeEntry[] { firstTime ? "初入蓬莱" : null, null, null, null, null, null, null, null, null, null } },
            { JingJie.ZhuJi    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.JinDan   , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null/*Wishlist*/ } },
            { JingJie.YuanYing , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.HuaShen  , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
            { JingJie.FanXu    , new NodeEntry[] { null, null, null, null, null, null, null, null, null, null } },
        };

        map._normalPools = new Dictionary<JingJie, AutoPool<NodeEntry>[]>()
        {
            { JingJie.LianQi   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.ZhuJi    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.JinDan   , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.YuanYing , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.HuaShen  , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
            { JingJie.FanXu    , new[] { map._b, map._r, map._b, map._a, map._b, map._r, map._b, map._a, map._b, map._r } },
        };
    }

    private static void Standard(RunEnvironment env)
    {
        bool firstTime = true;
        bool exhibitionVersion = true;

        env.Map.JingJie = JingJie.LianQi;
        env.AddXiuWei(50);
        if (firstTime)
        {

        }
        else
        {
            env.ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
        }
    }

    private static void Custom(RunEnvironment env)
    {
        env.Map.JingJie = JingJie.HuaShen;
        env.Home.SetJingJie(JingJie.HuaShen);
        env.ForceDrawSkills(jingJie: JingJie.HuaShen, count: 12);
    }

    public static async Task DefaultStartTurn(StageEntity owner, EventDetails eventDetails)
    {
        TurnDetails d = (TurnDetails)eventDetails;
        // if (d.Owner == owner)
        //     await owner.ArmorLoseSelfProcedure(owner.Armor);
    }
}
