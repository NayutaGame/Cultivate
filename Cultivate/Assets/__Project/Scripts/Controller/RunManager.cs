
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, Addressable
{
    public static readonly int MaxSlotCount = 12;
    public static readonly int[] SlotCountFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };
    public static readonly float EUREKA_DISCOUNT_RATE = 0.5f;

    // public RunAnimationController AnimationController;
    public RunEnvironment Environment;
    public Arena Arena;

    public SpriteRenderer BackgroundRenderer;

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

    public void SetEnvironmentFromConfig(RunConfig config)
    {
        Environment?.Unregister();
        Environment = RunEnvironment.FromConfig(config);
        Environment.Register();
        Environment.StartRunProcedure(new RunDetails());
    }

    public void SetEnvironmentToNull()
    {
        Environment?.Unregister();
        Environment = null;
    }

    public void ReturnToTitle()
    {
        AppManager.Pop();
    }

    public event Action<StageCommitDetails> StageCommitEvent;
    public void StageCommit(StageCommitDetails d) => StageCommitEvent?.Invoke(d);

    public event Action<StatusChangedDetails> StatusChangedEvent;
    public void StatusChanged(StatusChangedDetails d) => StatusChangedEvent?.Invoke(d);

    public void SetBackgroundFromJingJie(JingJie j)
    {
        SetBackground($"{j.Name}背景");
    }
    
    private void SetBackground(SpriteEntry background)
    {
        BackgroundRenderer.sprite = background.Sprite;
    }
}
