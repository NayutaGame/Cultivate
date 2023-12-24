
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;

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
    public StageEnvironment Environment;
    public StageTimeline Timeline;
    private Task _task;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "Environment", () => Environment },
            { "Timeline",    () => Timeline },
        };

        Anim = new();
    }

    public async Task Enter()
    {
        CanvasManager.Instance.StageCanvas.InitialSetup();
        _task = Environment.Simulate();
        await _task;
        AppManager.Pop();
    }

    public async Task Exit()
    {
        DisableVFX();

        if (!Environment.Config.WriteResult)
            return;

        Signal signal = new BattleResultSignal(Environment.Result.HomeVictory
            ? BattleResultSignal.BattleResultState.Win
            : BattleResultSignal.BattleResultState.Lose);
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(signal);
        await CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
        Environment.WriteResult();
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
