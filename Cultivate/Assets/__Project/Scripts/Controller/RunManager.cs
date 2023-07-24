
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

    public RunEnvironment Battle { get; private set; }
    public RunEnvironment Simulate { get; private set; }
    public Arena Arena;

    public CombatDetails CombatDetails;
    public StageReport Report;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public override void DidAwake()
    {
        base.DidAwake();

        SkillPool = new();
        EntityPool = new();

        TechInventory = new();
        Battle = new();
        Simulate = new();
        Arena = new();
        Map = new();

        _accessors = new()
        {
            { "TechInventory",         () => TechInventory },
            { "Map",                   () => Map },
            { "Battle",                () => Battle },
            { "Simulate",              () => Simulate },
            { "Arena",                 () => Arena },
            { "CurrentNode",           TryGetCurrentNode },
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;
    }

    public void Enter()
    {
        Simulate.Enter();
        Battle.Enter();

        DesignerEnvironment.EnterRun();
    }

    public void CExit()
    {
        Battle.Hero.TryExhaust();
        RunCanvas.Instance.Refresh();
    }

    public RunNode TryGetCurrentNode() => Map.TryGetCurrentNode();

    #region Resource

    public Modifier Modifier;

    private int _mingYuan;
    public int MingYuan
    {
        get => _mingYuan;
        set => _mingYuan = value;
    }

    private int _turn;
    public int Turn => _turn;

    private float _xiuWei;
    public float XiuWei => _xiuWei;

    private float _chanNeng;
    public float ChanNeng => _chanNeng;

    public float TurnXiuWei => Modifier.Value.ForceGet("turnXiuWeiAdd") * (1 + Modifier.Value.ForceGet("turnXiuWeiMul"));

    public float TurnChanNeng => Modifier.Value.ForceGet("turnChanNengAdd") * (1 + Modifier.Value.ForceGet("turnChanNengMul"));

    public void AddXiuWei(int xiuWei = 10)
    {
        _xiuWei += xiuWei;
    }

    public void RemoveXiuWei(int value)
    {
        _xiuWei -= value;
    }

    public void AddMingYuan(int mingYuan = 1)
    {
        _mingYuan += mingYuan;
    }

    public void AddHealth(int health)
    {
        Battle.Hero.SetDHealth(Battle.Hero.GetDHealth() + health);
    }

    #endregion

    public bool CanAffordTech(IndexPath indexPath)
    {
        RunTech runTech = DataManager.Get<RunTech>(indexPath);
        return runTech.GetCost() <= _xiuWei;
    }

    public bool TrySetDoneTech(IndexPath indexPath)
    {
        if (!CanAffordTech(indexPath))
            return false;

        RunTech runTech = DataManager.Get<RunTech>(indexPath);
        _xiuWei -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
    }

    public void Combat(bool useAnim, RunEnvironment environment)
    {
        CombatDetails = new CombatDetails(useAnim, environment == Battle, environment.Hero, environment.Enemy);
        AppManager.Push(new AppStageS());
    }
}
