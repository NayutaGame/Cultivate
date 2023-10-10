
using System;

public class RunConfig
{
    private Action<RunEnvironment> _execute;

    public RunConfig(Action<RunEnvironment> execute)
    {
        _execute = execute;
    }

    public void Execute(RunEnvironment env)
    {
        _execute?.Invoke(env);
    }
}
