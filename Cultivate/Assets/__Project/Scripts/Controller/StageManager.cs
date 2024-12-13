
using System;
using System.Collections.Generic;
using UnityEngine;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

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

    public Transform HomeAnchor;
    private PrefabEntry HomePrefabEntry;
    private GameObject HomeGameObject;
    [HideInInspector] public IStageModel HomeModel;
    
    public Transform AwayAnchor;
    private PrefabEntry AwayPrefabEntry;
    private GameObject AwayGameObject;
    [HideInInspector] public IStageModel AwayModel;

    public StageAnimationController StageAnimationController;
    private StageEnvironment _environment;
    public StageTimeline Timeline;
    private UniTask _task;

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

    public async UniTask Enter()
    {
        _task = _environment.CoreProcedure();
        await _task;
        AppManager.Pop();
    }

    public async UniTask Exit()
    {
        DisableVFX();

        if (!_environment.Result.WriteResult)
            return;

        Signal signal = new BattleResultSignal(_environment.Result.Flag == 1);
        CanvasManager.Instance.RunCanvas.LegacySetPanelSFromSignal(signal);
        CanvasManager.Instance.Curtain.GetAnimator().SetState(1);
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

    public void SetHomeFromCharacterProfile(CharacterProfile characterProfile)
    {
        // _slots[0].SR.sprite = characterProfile.GetEntry().GetSprite();
        SetHomeModel(characterProfile.GetEntry().GetStagePrefabEntry());
    }

    public void SetAwayFromRunEntity(RunEntity runEntity)
    {
        // _slots[1].SR.sprite = runEntity.GetEntry().GetSprite();
        SetAwayModel(runEntity.GetEntry().GetStageModelPrefabEntry());
    }
    
    private void SetHomeModel(PrefabEntry targetPrefabEntry)
    {
        if (HomePrefabEntry == targetPrefabEntry)
            return;
        
        if (HomeGameObject != null)
            Destroy(HomeGameObject);

        HomePrefabEntry = targetPrefabEntry;
        HomeGameObject = Instantiate(HomePrefabEntry.Prefab, HomeAnchor);
        HomeModel = HomeGameObject.GetComponent<IStageModel>();

        HomeModel.BaseTransform = HomeAnchor;
    }
    
    private void SetAwayModel(PrefabEntry targetPrefabEntry)
    {
        if (AwayPrefabEntry == targetPrefabEntry)
            return;
        
        if (AwayGameObject != null)
            Destroy(AwayGameObject);

        AwayPrefabEntry = targetPrefabEntry;
        AwayGameObject = Instantiate(AwayPrefabEntry.Prefab, AwayAnchor);
        AwayModel = AwayGameObject.GetComponent<IStageModel>();
        
        AwayModel.BaseTransform = AwayAnchor;
    }
}
