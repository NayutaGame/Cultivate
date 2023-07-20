
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

public class StageManager : Singleton<StageManager>, GDictionary
{
    public Transform VFXPool;
    public GameObject FlowTextVFXPrefab;
    public GameObject[] PiercingVFXFromWuXing;
    public GameObject[] HitVFXFromWuXing;
    public GameObject BuffVFXPrefab;
    public GameObject DebuffVFXPrefab;
    public GameObject HealVFXPrefab;

    public EntitySlot[] _slots;

    public StageEnvironment CurrEnv;
    public StageEnvironment EndEnv;

    public StageAnimationDelegate Anim;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "CurrEnv",               () => CurrEnv },
            { "EndEnv",                () => EndEnv },
        };

        Anim = new();
    }

    private Task _task;

    public async Task Enter()
    {
        CombatDetails d = RunManager.Instance.CombatDetails;

        EndEnv = new StageEnvironment(d.Home, d.Away, useTimeline: true, useSb: true);
        CurrEnv = new StageEnvironment(d.Home, d.Away, useTween: true);
        EndEnv.Simulate().GetAwaiter().GetResult();
        EndEnv.WriteResult();
        RunManager.Instance.Report = EndEnv.Report;

        if (!d.UseAnim)
        {
            AppManager.Pop();
            return;
        }

        CanvasManager.Instance.StageCanvas.InitialSetup();
        _task = CurrEnv.Simulate();
        await _task;
        AppManager.Pop();
    }

    public static StageReport SimulateBrief(RunEntity home, RunEntity away)
    {
        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate().GetAwaiter().GetResult();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static bool[] ManaSimulate(RunEntity home, RunEntity away)
    {
        Instance.EndEnv = new StageEnvironment(home, away);
        return Instance.EndEnv.InnerManaSimulate().GetAwaiter().GetResult();
    }

    public void Exit()
    {
        DisableVFX();

        CombatDetails d = RunManager.Instance.CombatDetails;
        if (!d.FireSignal)
            return;

        Signal signal = new BattleResultSignal(EndEnv.Report.HomeVictory
            ? BattleResultSignal.BattleResultState.Win
            : BattleResultSignal.BattleResultState.Lose);
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(signal);
        RunCanvas.Instance.SetNodeState(panelDescriptor);
        EndEnv.WriteEffect();
    }

    public void Pause()
    {
        Anim.PauseTween();
    }

    public void Resume()
    {
        Anim.ResumeTween();
    }

    public void SetSpeed(float speed)
    {
        Anim.SetSpeed(speed);
    }

    public void Skip()
    {
        Anim.Skip();
    }

    public void DisableVFX()
    {
        for (int i = 0; i < VFXPool.childCount; i++)
        {
            VFXPool.GetChild(i).gameObject.SetActive(false);
        }
    }
}
