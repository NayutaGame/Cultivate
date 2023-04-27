
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using DG.Tweening;

public class StageManager : Singleton<StageManager>, GDictionary
{
    public EntitySlot[] _slots;
    private Sequence Seq;

    public StageEnvironment CurrEnv;
    public StageEnvironment EndEnv;

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
    }

    public void Enter()
    {
        EndEnv = new StageEnvironment(useTimeline: true, useSb: true);
        CurrEnv = new StageEnvironment(useTween: true);
        EndEnv.Simulate();
        EndEnv.WriteResult();
        RunManager.Instance.Report = EndEnv.Report;
        EndEnv.WriteEffect();

        if (!RunManager.Instance.IsStream)
        {
            AppManager.Pop();
            return;
        }

        CurrEnv.Simulate();
        CanvasManager.Instance.StageCanvas.Play();
    }

    public static StageReport SimulateBrief(RunHero home, RunEnemy away)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static StageReport SimulateBrief(RunEnemy home, RunEnemy away)
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment(home, away, useSb: true);
        Instance.EndEnv.Simulate();
        Instance.EndEnv.WriteResult();
        return Instance.EndEnv.Report;
    }

    public static bool[] ManaSimulate()
    {
        AppManager.Instance.StageManager.gameObject.SetActive(true);
        AppManager.Instance.StageManager.gameObject.SetActive(false);

        Instance.EndEnv = new StageEnvironment();
        return Instance.EndEnv.InnerManaSimulate();
    }

    public void Exit()
    {
        BattlePanelDescriptor battlePanelDescriptor = RunManager.Instance.TryGetCurrentNode()?.CurrentPanel as BattlePanelDescriptor;
        if (battlePanelDescriptor == null)
            return;

        battlePanelDescriptor.ReceiveSignal(new BattleResultSignal(EndEnv.Report.HomeVictory ? BattleResultSignal.BattleResultState.Win : BattleResultSignal.BattleResultState.Lose));
    }

    public void ResetTween()
    {
        Seq.Kill();
        Seq = DOTween.Sequence().SetAutoKill();
    }

    public void AppendTween(Tween tween)
    {
        Seq.Append(tween);
    }

    public async Task PlayTween()
    {
        if (Seq == null)
            return;
        Seq.Restart();
        await Seq.AsyncWaitForCompletion();
    }

    public async Task AsyncWaitForCompletion()
    {
        if(Seq.IsPlaying())
            await Seq.AsyncWaitForCompletion();
    }
}
