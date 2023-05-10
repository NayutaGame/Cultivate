
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class StageManager : Singleton<StageManager>, GDictionary
{
    public Transform VFXPool;
    public GameObject FlowTextVFXPrefab;

    public EntitySlot[] _slots;

    public StageEnvironment CurrEnv;
    public StageEnvironment EndEnv;

    public StageAnimationDelegate Anim;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (string key in indexPath.Values)
        {
            if (int.TryParse(key, out int i))
            {
                IList l = curr as IList;
                if (l.Count <= i)
                    return default;
                curr = l[i];
            }
            else
            {
                curr = (curr as GDictionary).GetAccessors()[key]();
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

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
        EndEnv.WriteEffect();

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
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate().GetAwaiter().GetResult();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static bool[] ManaSimulate(RunEntity home, RunEntity away)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away);
        return Instance.EndEnv.InnerManaSimulate().GetAwaiter().GetResult();
    }

    public void Exit()
    {
        BattlePanelDescriptor battlePanelDescriptor = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel as BattlePanelDescriptor;
        if (battlePanelDescriptor == null)
            return;

        CombatDetails d = RunManager.Instance.CombatDetails;
        if (!d.FireSignal)
            return;

        battlePanelDescriptor.ReceiveSignal(new BattleResultSignal(EndEnv.Report.HomeVictory ? BattleResultSignal.BattleResultState.Win : BattleResultSignal.BattleResultState.Lose));
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
}
