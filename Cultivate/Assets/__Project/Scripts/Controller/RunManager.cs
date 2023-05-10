
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, GDictionary
{
    public static readonly int NeiGongLimit = 4;
    public static readonly int WaiGongLimit = 12;

    public static readonly int[] NeiGongLimitFromJingJie = new[] { 0, 1, 2, 3, 4, 4 };
    public static readonly int[] WaiGongLimitFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };

    public static readonly int[] WaiGongStartFromJingJie = new[] { 9, 6, 4, 2, 0, 0 };

    public static readonly float EUREKA_DISCOUNT_RATE = 0.5f;

    public event Action<StageCommitDetails> StageCommitEvent;
    public void StageCommit(StageCommitDetails d) => StageCommitEvent?.Invoke(d);

    public event Action<GainSkillDetails> GainSkillEvent;
    public void Acquire(GainSkillDetails d) => GainSkillEvent?.Invoke(d);

    public event Action<StatusChangedDetails> StatusChangedEvent;
    public void StatusChanged(StatusChangedDetails d) => StatusChangedEvent?.Invoke(d);

    public SkillPool SkillPool { get; private set; }
    public EntityPool EntityPool { get; private set; }

    public TechInventory TechInventory { get; private set; }
    public Map Map { get; private set; }

    public BattleRunEnvironment Battle { get; private set; }
    public SimulateRunEnvironment Simulate { get; private set; }
    public Arena Arena;

    private int _mingYuan;
    public int MingYuan
    {
        get => _mingYuan;
        set => _mingYuan = value;
    }

    // 灵根
    // 神识
    // 遁速
    // 心境

    public Modifier Modifier;

    private int _turn;
    public int Turn => _turn;

    private float _xiuWei;
    public float XiuWei => _xiuWei;

    private float _chanNeng;
    public float ChanNeng => _chanNeng;

    public float TurnXiuWei => Modifier.Value.ForceGet("turnXiuWeiAdd") * (1 + Modifier.Value.ForceGet("turnXiuWeiMul"));

    public float TurnChanNeng => Modifier.Value.ForceGet("turnChanNengAdd") * (1 + Modifier.Value.ForceGet("turnChanNengMul"));

    public CombatDetails CombatDetails;
    public StageReport Report;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (string key in indexPath.Values)
        {
            if (int.TryParse(key, out int i))
            {
                IList l = curr as IList;
                if (l.Count <= i)
                    return default;
                curr = l[i];
            }
            else
            {
                curr = (curr as GDictionary).GetAccessors()[key]();
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

    public override void DidAwake()
    {
        base.DidAwake();

        SkillPool = new();
        EntityPool = new();

        TechInventory = new();
        Map = new();
        Battle = new();
        Simulate = new();
        Arena = new();

        _accessors = new()
        {
            { "TechInventory",         () => TechInventory },
            { "Map",                   () => Map },
            { "Battle",                () => Battle },
            { "Simulate",              () => Simulate },
            { "Arena",                 () => Arena },
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;

        DesignerEnvironment.EnterRun();
    }

    public void Enter()
    {
        Simulate.Enter();
        Battle.Enter();
    }

    public void CExit()
    {
        Battle.Hero.TryConsume();
        RunCanvas.Instance.Refresh();
    }

    public RunNode TryGetCurrentNode() => Map.TryGetCurrentNode();

    public void AddTurn()
    {
        _turn += 1;
        _xiuWei += TurnXiuWei;
        _chanNeng += TurnChanNeng;
    }

    public void AddXiuWei(int xiuWei = 10)
    {
        _xiuWei += xiuWei;
    }

    public void AddChanNeng(int chanNeng = 10)
    {
        _chanNeng += chanNeng;
    }

    public void AddMingYuan(int mingYuan = 10)
    {
        _mingYuan += mingYuan;
    }

    // public void RefreshChip()
    // {
    //     SkillInventory.RefreshChip();
    // }
    //
    // public void ClearChip()
    // {
    //     SkillInventory.Clear();
    // }

    public bool CanAffordTech(IndexPath indexPath)
    {
        RunTech runTech = Get<RunTech>(indexPath);
        return runTech.GetCost() <= _xiuWei;
    }

    public bool TrySetDoneTech(IndexPath indexPath)
    {
        if (!CanAffordTech(indexPath))
            return false;

        RunTech runTech = Get<RunTech>(indexPath);
        _xiuWei -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
    }

    public bool TryClickNode(IndexPath indexPath)
    {
        RunNode runNode = Get<RunNode>(indexPath);
        if (runNode.State != RunNode.RunNodeState.ToChoose || !Map.Selecting)
            return false;

        Map.SelectedNode(runNode);
        return true;
    }

    public void Combat(bool useAnim, RunEnvironment environment)
    {
        CombatDetails = new CombatDetails(useAnim, environment is BattleRunEnvironment, environment.Hero, environment.Enemy);
        AppManager.Push(new AppStageS());
    }
}
