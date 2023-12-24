
using System;
using System.Collections.Generic;
using CLLibrary;

public class RunManager : Singleton<RunManager>, Addressable
{
    public static readonly int SkillLimit = 12;
    public static readonly int[] SkillLimitFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };
    public static readonly int[] SkillStartFromJingJie = new[] { 9, 6, 4, 2, 0, 0 };
    public static readonly float EUREKA_DISCOUNT_RATE = 0.5f;

    public RunAnimationDelegate Anim;
    public RunEnvironment Environment;
    public Arena Arena;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "Environment",           () => Environment },
            { "Arena",                 () => Arena },
        };

        Arena = new();
    }

    public void SetEnvironment(RunConfig config)
    {
        Environment = RunEnvironment.FromConfig(config);
    }

    public void Exit()
    {
        Environment = null;
    }

    public void CExit()
    {
        CanvasManager.Instance.RunCanvas.Refresh();
    }

    public void ReturnToTitle()
    {
        AppManager.Pop();
    }

    public event Action<StageCommitDetails> StageCommitEvent;
    public void StageCommit(StageCommitDetails d) => StageCommitEvent?.Invoke(d);

    public event Action<GainSkillDetails> GainSkillEvent;
    public void Acquire(GainSkillDetails d) => GainSkillEvent?.Invoke(d);

    public event Action<StatusChangedDetails> StatusChangedEvent;
    public void StatusChanged(StatusChangedDetails d) => StatusChangedEvent?.Invoke(d);
}
