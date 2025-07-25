
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
    public GameObject LoseArmorVFXPrefab;
    public GameObject DodgeVFXPrefab;

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
    public StageEnvironment Environment => _environment;
    public StageTimeline Timeline;
    private UniTask _task;

    private AnnotatableLine GetSkipButtonInactiveHint()
        => new ("通关一次后解锁");

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Environment",                thisObject => ((StageManager)thisObject)._environment },
        { "Timeline",                   thisObject => ((StageManager)thisObject).Timeline },
        { "SkipButtonInactiveHint",     thisObject => ((StageManager)thisObject).GetSkipButtonInactiveHint() },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        StageAnimationController = new();
    }

    public void SetEnvironmentFromConfig(StageConfig config)
    {
        _environment = StageEnvironment.FromConfig(config);
        Timeline = StageResult.FromConfig(StageConfig.ForTimeline(config.Home, config.Away, config.RunConfig)).Timeline;
    }

    public async UniTask Enter()
    {
        _task = _environment.CoreProcedure();
        await _task;
        AppManager.Instance.Pop();
    }

    public async UniTask Exit()
    {
        DisableVFX();

        if (!_environment.Result.WillEffectResult)
            return;

        StageConfig config = _environment.GetConfig();
        StageResult result = StageResult.FromConfig(StageConfig.ForCombatOnlyResult(config.Home, config.Away, config.RunConfig));
        RunManager.Instance.Environment.ReceiveSignalProcedure(new BattleResultSignal(result.Flag == 1));
        CanvasManager.Instance.Curtain.GetAnimator().SetState(1);
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
    {
        _environment?.SetShouldSkip();
        // StageAnimationController.Skip();
    }

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
    
    
    
    
    
    
    private Dictionary<GameObject, Queue<GameObject>> _objectPool = new();
    
    /// <summary>
    /// 从对象池获取对象
    /// </summary>
    /// <param name="prefab">需要获取的预制体</param>
    /// <returns>可用的游戏对象</returns>
    public GameObject FetchObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("尝试获取空预制体对象");
            return null;
        }

        if (!_objectPool.ContainsKey(prefab))
        {
            _objectPool[prefab] = new Queue<GameObject>();
        }

        var poolQueue = _objectPool[prefab];
        GameObject obj;

        if (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, VFXPool);
        }

        return obj;
    }

    /// <summary>
    /// 归还对象到对象池
    /// </summary>
    /// <param name="prefab">原始预制体</param>
    /// <param name="obj">需要归还的对象</param>
    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        if (prefab == null || obj == null)
        {
            Debug.LogError("尝试归还空对象");
            return;
        }

        if (!_objectPool.ContainsKey(prefab))
        {
            _objectPool[prefab] = new Queue<GameObject>();
        }

        obj.transform.SetParent(VFXPool);
        obj.SetActive(false);

        _objectPool[prefab].Enqueue(obj);
    }
}
