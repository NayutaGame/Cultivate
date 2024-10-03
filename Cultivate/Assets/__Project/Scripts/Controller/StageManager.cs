
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CLLibrary;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class StageManager : Singleton<StageManager>, Addressable
{
    public Transform VFXPool;
    public GameObject FloatTextVFXPrefab;
    public GameObject[] PiercingVFXFromWuXing;
    public GameObject[] HitVFXFromWuXing;
    public GameObject BuffVFXPrefab;
    public GameObject DebuffVFXPrefab;
    public GameObject HealVFXPrefab;
    public GameObject LingQiVFXPrefab;
    public GameObject GainArmorVFXPrefab;
    public GameObject GuardedVFXPrefab;
    public GameObject FragileVFXPrefab;
    public EntitySlot[] _slots;

    public StageAnimationController StageAnimationController;
    private StageEnvironment _environment;
    public StageTimeline Timeline;
    private Task _task;

    public SpriteRenderer BackgroundRenderer;

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

        StageAnimationController = new();
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

        Signal signal = new BattleResultSignal(_environment.Result.Flag == 1);
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(signal);
        PanelS panelS = PanelS.FromPanelDescriptor(panelDescriptor);
        await CanvasManager.Instance.RunCanvas.SetPanelSAsync(panelS);
        _environment.WriteResult();
    }

    public void Pause()
        => StageAnimationController.Pause();

    public void Pause(InteractBehaviour ib, PointerEventData eventData)
        => Pause();

    public void Resume()
        => StageAnimationController.Resume();

    public void Resume(InteractBehaviour ib, PointerEventData eventData)
        => Resume();

    public void SetSpeed(float speed)
        => StageAnimationController.SetSpeed(speed);

    public void Skip()
        => StageAnimationController.Skip();

    public void DisableVFX()
    {
        for (int i = 0; i < VFXPool.childCount; i++)
        {
            VFXPool.GetChild(i).gameObject.SetActive(false);
        }
    }
}
