
using System;
using UnityEngine;

[Serializable]
public class RunResult : Result
{
    public enum RunResultState
    {
        Unfinished,
        Defeat,
        Victory,
    }

    [SerializeField] private RunResultState _state;
    public RunResultState GetState() => _state;
    public void SetState(RunResultState state) => _state = state;

    public RunResult()
    {
        _state = RunResultState.Unfinished;
    }
}
