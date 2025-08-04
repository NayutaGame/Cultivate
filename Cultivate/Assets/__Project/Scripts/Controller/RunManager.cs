
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, Addressable
{
    [NonSerialized] public RunEnvironment Environment;

    [NonSerialized] public Neuron<RunEnvironment> UnregisteredRunEnvironmentNeuron = new();
    [NonSerialized] public Neuron<RunEnvironment> RegisteredRunEnvironmentNeuron = new();

    public SpriteRenderer BackgroundRenderer;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Environment",                thisObject => ((RunManager)thisObject).Environment },
    };
    public object Get(string s) => Accessor[s](this);

    private void SetEnvironment(RunEnvironment newEnvironment)
    {
        if (Environment != null)
        {
            Environment.Unregister();
            UnregisteredRunEnvironmentNeuron.Invoke(Environment);
        }
        Environment = newEnvironment;
        if (Environment != null)
        {
            Environment.Register();
            RegisteredRunEnvironmentNeuron.Invoke(Environment);
        }
    }

    public void SetEnvironmentFromConfig(RunConfig config)
    {
        RunEnvironment newEnvironment = RunEnvironment.FromConfig(config);
        SetEnvironment(newEnvironment);
        Environment.StartRunProcedure(new StartRunDetails());
    }

    public void SetEnvironmentFromSaved(RunEnvironment env)
    {
        SetEnvironment(env);
        Environment.ContinueRunProcedure(new ContinueRunDetails());
    }

    public void SetEnvironmentToNull()
    {
        SetEnvironment(null);
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
