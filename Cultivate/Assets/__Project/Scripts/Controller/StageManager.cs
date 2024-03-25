
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using UnityEngine.EventSystems;

public class StageManager : Singleton<StageManager>, Addressable
{
    public Transform VFXPool;
    public GameObject FlowTextVFXPrefab;
    public GameObject[] PiercingVFXFromWuXing;
    public GameObject[] HitVFXFromWuXing;
    public GameObject BuffVFXPrefab;
    public GameObject DebuffVFXPrefab;
    public GameObject HealVFXPrefab;
    public EntitySlot[] _slots;

    public StageAnimationDelegate Anim;
    private StageEnvironment _environment;
    public StageTimeline Timeline;
    private Task _task;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "Environment", () => _environment },
            { "Timeline",    () => Timeline },
        };

        Anim = new();
    }

    public void SetEnvironmentFromConfig(StageConfig config)
    {
        _environment = StageEnvironment.FromConfig(config);
        Timeline = StageEnvironment.CalcTimeline(config);
    }

    public async Task Enter()
    {
        _task = _environment.CoreProcedure();
        await _task;
        AppManager.Pop();
    }

    public async Task Exit()
    {
        DisableVFX();

        if (!_environment.Result.WriteResult)
            return;

        Signal signal = new BattleResultSignal(_environment.Result.HomeVictory
            ? BattleResultSignal.BattleResultState.Win
            : BattleResultSignal.BattleResultState.Lose);
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(signal);
        await CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        _environment.WriteResult();
    }

    public void Pause()
        => Anim.PauseTween();

    public void Pause(InteractBehaviour ib, PointerEventData eventData)
        => Pause();

    public void Resume()
        => Anim.ResumeTween();

    public void Resume(InteractBehaviour ib, PointerEventData eventData)
        => Resume();

    public void SetSpeed(float speed)
        => Anim.SetSpeed(speed);

    public void Skip()
        => Anim.Skip();

    public void DisableVFX()
    {
        for (int i = 0; i < VFXPool.childCount; i++)
        {
            VFXPool.GetChild(i).gameObject.SetActive(false);
        }
    }
}
