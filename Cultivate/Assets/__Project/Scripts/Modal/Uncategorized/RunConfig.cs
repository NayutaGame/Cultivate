
using System;

public class RunConfig
{
    private Action<Map> _initMapPools;
    private Action<RunEnvironment> _execute;

    public RunConfig(Action<Map> initMapPools = null, Action<RunEnvironment> execute = null)
    {
        _initMapPools = initMapPools;
        _execute = execute;
    }

    public void InitMapPools(Map map)
    {
        _initMapPools?.Invoke(map);
    }

    public void Execute(RunEnvironment env)
    {
        _execute?.Invoke(env);
    }
}
