
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using JetBrains.Annotations;
using UnityEngine;

public class RunManager : Singleton<RunManager>, GDictionary
{
    public static readonly int SkillLimit = 12;
    public static readonly int[] SkillLimitFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };
    public static readonly int[] SkillStartFromJingJie = new[] { 9, 6, 4, 2, 0, 0 };
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
    public object Get(string s) => _accessors[s]();
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

    public void AddHealth(int health)
    {
        Battle.Hero.SetDHealth(Battle.Hero.GetDHealth() + health);
    }

    public MingYuan GetMingYuan()
        => Battle.Hero.MingYuan;

    public void SetDMingYuan(int value)
    {
        Battle.Hero.MingYuan.SetDiff(value);
    }

    #endregion

    public void ForceDrawSkill(Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
        => ForceDrawSkill(new DrawSkillDetails(pred, wuXing, jingJie));

    public void ForceDrawSkills(Predicate<SkillEntry> pred = null, WuXing? wuXing = null,
        JingJie? jingJie = null, int count = 1, bool distinct = true, bool consume = true)
        => ForceDrawSkills(new DrawSkillsDetails(pred, wuXing, jingJie, count, distinct, consume));

    public void ForceDrawSkill(DrawSkillDetails d)
    {
        bool success = Instance.SkillPool.TryDrawSkill(out RunSkill skill, d);
        if (!success)
            throw new Exception();

        Instance.Battle.SkillInventory.AddSkill(skill);
    }

    public void ForceDrawSkills(DrawSkillsDetails d)
    {
        bool success = Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, d);
        if (!success)
            throw new Exception();

        Instance.Battle.SkillInventory.AddSkills(skills);
    }

    public void ForceAddSkill(AddSkillDetails d)
        => Instance.Battle.SkillInventory.AddSkill(RunSkill.From(d._entry, d._jingJie));

    public void ForceAddMech([CanBeNull] MechType mechType = null, int count = 1)
        => ForceAddMech(new(mechType, count));
    public void ForceAddMech(AddMechDetails d)
    {
        MechType mechType = d._mechType ?? MechType.FromIndex(RandomManager.Range(0, MechType.Length));
        Instance.Battle.MechBag.AddMech(mechType, d._count);
    }

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
