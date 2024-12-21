
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, Addressable
{
    [NonSerialized] public RunEnvironment Environment;
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
        Environment.StartRunProcedure(new StartRunDetails());
    }

    public void SetEnvironmentFromProfile(RunEnvironment env)
    {
        Environment?.Unregister();
        Environment = env;
        Environment.Register();
        Environment.ContinueRunProcedure(new ContinueRunDetails());
    }

    public void SetEnvironmentToNull()
    {
        Environment?.Unregister();
        Environment = null;
    }

    public void ReturnToTitle()
    {
        AppManager.Instance.Pop();
    }

    public void SetBackgroundFromJingJie(JingJie j)
    {
        SetBackground($"{j.Name}背景");
    }
    
    private void SetBackground(SpriteEntry background)
    {
        BackgroundRenderer.sprite = background.Sprite;
    }
}
