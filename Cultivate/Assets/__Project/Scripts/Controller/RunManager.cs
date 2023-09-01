
using System;
using System.Collections.Generic;
using CLLibrary;

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

    public RunEnvironment Battle { get; private set; }
    public RunEnvironment Simulate { get; private set; }
    public Arena Arena;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        Battle = new();
        Simulate = new();
        Arena = new();

        _accessors = new()
        {
            { "Battle",                () => Battle },
            { "Simulate",              () => Simulate },
            { "Arena",                 () => Arena },
        };
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
}
