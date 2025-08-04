
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, Addressable
{
    [NonSerialized] public RunEnvironment Environment;

    public SpriteRenderer BackgroundRenderer;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Environment",                thisObject => ((RunManager)thisObject).Environment },
    };
    public object Get(string s) => Accessor[s](this);

    public void SetEnvironmentFromConfig(RunConfig config)
    {
        Environment?.Unregister();
        Environment = RunEnvironment.FromConfig(config);
        Environment.Register();
        Environment.StartRunProcedure(new StartRunDetails());
    }

    public void SetEnvironmentFromSaved(RunEnvironment env)
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
        SetBackground(j.GetBackgroundSprite());
    }
    
    private void SetBackground(Sprite backgroundSprite)
    {
        BackgroundRenderer.sprite = backgroundSprite;
    }
}
