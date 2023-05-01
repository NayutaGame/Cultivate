
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
        EndEnv = new StageEnvironment(useTimeline: true, useSb: true);
        CurrEnv = new StageEnvironment(useTween: true);
        EndEnv.Simulate().GetAwaiter().GetResult();
        EndEnv.WriteResult();
        RunManager.Instance.Report = EndEnv.Report;
        EndEnv.WriteEffect();

        if (!RunManager.Instance.IsStream)
        {
            AppManager.Pop();
            return;
        }

        CanvasManager.Instance.StageCanvas.InitialSetup();
        _task = CurrEnv.Simulate();
        await _task;
    }

    public static StageReport SimulateBrief(RunHero home, RunEnemy away)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate().GetAwaiter().GetResult();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static StageReport SimulateBrief(RunEnemy home, RunEnemy away)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate().GetAwaiter().GetResult();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static bool[] ManaSimulate()
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment();
        return Instance.EndEnv.InnerManaSimulate().GetAwaiter().GetResult();
    }

    public void Exit()
    {
        BattlePanelDescriptor battlePanelDescriptor = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel as BattlePanelDescriptor;
        if (battlePanelDescriptor == null)
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
}
