
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class LegacyListView : LegacySimpleView
{
    public RectTransform Container;
    public GameObject[] Prefabs;

    protected List<LegacyItemBehaviour> _activePool;
    protected List<LegacyItemBehaviour>[] _inactivePools;

    #region Accessors
    
    public List<LegacyItemBehaviour> ActivePool => _activePool;
    public List<LegacyItemBehaviour>[] InactivePools => _inactivePools;

    public IEnumerable<LegacyItemBehaviour> Traversal()
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour;
        foreach (var itemBehaviourList in _inactivePools)
        foreach (var itemBehaviour in itemBehaviourList)
            yield return itemBehaviour;
    }

    public IEnumerable<LegacyItemBehaviour> TraversalActive()
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour;
    }

    public int? IndexFromItemBehaviour(LegacyItemBehaviour toGetIndex)
    {
        if (toGetIndex == null)
            return null;
        return _activePool.FirstIdx(itemBehaviour => itemBehaviour == toGetIndex);
    }

    public LegacyItemBehaviour ItemBehaviourFromIndex(int i)
        => _activePool[i];

    #endregion

    #region List Operations

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _activePool = new List<LegacyItemBehaviour>();
        _inactivePools = new List<LegacyItemBehaviour>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<LegacyItemBehaviour>();

        RegisterExists();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        CheckAwake();
        Sync();
    }

    public override void Refresh()
    {
        base.Refresh();
        _activePool.Do(itemBehaviour =>
            itemBehaviour.GetSimpleView().Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            LegacyItemBehaviour itemBehaviour = RegisterItemBehaviour(Container.GetChild(i).gameObject);
            InitItemBehaviour(itemBehaviour, itemBehaviour.PrefabIndex);
        }
    }

    public virtual void Sync()
    {
        DisableAllItemBehaviours();

        Model = Get<IListModel>();
        for (int i = 0; i < Model.Count(); i++)
            InsertItem(i, GetAddress().Append($"#{i}").Get<object>());

        Refresh();
    }

    #endregion

    #region Atomic Operations

    private LegacyItemBehaviour AllocItemBehaviour(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], Container).GetComponent<LegacyItemBehaviour>();

    private LegacyItemBehaviour RegisterItemBehaviour(GameObject go)
        => go.GetComponent<LegacyItemBehaviour>();

    protected virtual void InitItemBehaviour(LegacyItemBehaviour itemBehaviour, int prefabIndex)
    {
        itemBehaviour.GetComponent<LegacyView>().AwakeFunction();
        itemBehaviour.gameObject.SetActive(false);

        itemBehaviour.PrefabIndex = prefabIndex;
        _inactivePools[prefabIndex].Add(itemBehaviour);
        BindInteractBehaviour(itemBehaviour.GetInteractBehaviour());
        itemBehaviour.name = Traversal().Count().ToString();
    }

    protected virtual LegacyItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        List<LegacyItemBehaviour> pool = _inactivePools[prefabIndex];
        LegacyItemBehaviour itemBehaviour = pool[orderInPool];

        pool.RemoveAt(orderInPool);
        _activePool.Insert(index, itemBehaviour);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].GetSimpleView().SetAddress(GetAddress().Append($"#{i}"));
            _activePool[i].GetSimpleView().Refresh();
        }

        itemBehaviour.transform.SetSiblingIndex(index);
        itemBehaviour.gameObject.SetActive(true);

        return itemBehaviour;
    }

    protected virtual LegacyItemBehaviour DisableItemBehaviour(int index)
    {
        LegacyItemBehaviour itemBehaviour = _activePool[index];

        itemBehaviour.gameObject.SetActive(false);
        itemBehaviour.transform.SetAsLastSibling();

        _activePool.RemoveAt(index);
        _inactivePools[itemBehaviour.PrefabIndex].Add(itemBehaviour);

        for (int i = index; i < _activePool.Count; i++)
            _activePool[i].GetSimpleView().SetAddress(GetAddress().Append($"#{i}"));

        return itemBehaviour;
    }

    protected virtual void DisableAllItemBehaviours()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            LegacyItemBehaviour itemBehaviour = _activePool[index];

            itemBehaviour.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemBehaviour.PrefabIndex].Insert(0, itemBehaviour);
        }
    }

    private int FetchItemBehaviour(int prefabIndex)
    {
        List<LegacyItemBehaviour> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;

        LegacyItemBehaviour itemBehaviour = AllocItemBehaviour(prefabIndex);
        InitItemBehaviour(itemBehaviour, prefabIndex);
        return 0;
    }

    #endregion

    #region Prefab Provider

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];
    protected int GetPrefabIndex(object model) => _prefabProvider?.Invoke(model) ?? 0;

    #endregion

    #region Model Delegates

    private IListModel _model;
    protected IListModel Model
    {
        get => _model;
        set
        {
            if (_model != null)
            {
                _model.InsertEvent -= InvokeInsertGate;
                _model.RemoveAtEvent -= InvokeRemoveAtGate;
                _model.ModifiedEvent -= InvokeModifiedGate;
                _model.ResyncEvent -= InvokeResyncGate;
            }
            _model = value;
            if (_model != null)
            {
                _model.InsertEvent += InvokeInsertGate;
                _model.RemoveAtEvent += InvokeRemoveAtGate;
                _model.ModifiedEvent += InvokeModifiedGate;
                _model.ResyncEvent += InvokeResyncGate;
            }
        }
    }

    private event Func<int, object, UniTask> InsertGate;
    private async UniTask InvokeInsertGate(int index, object item) { if (InsertGate != null) await InsertGate(index, item); }
    private event Func<int, UniTask> RemoveAtGate;
    private async UniTask InvokeRemoveAtGate(int index) { if (RemoveAtGate != null) await RemoveAtGate(index); }
    private event Func<int, UniTask> ModifiedGate;
    private async UniTask InvokeModifiedGate(int index) { if (ModifiedGate != null) await ModifiedGate(index); }
    private event Func<UniTask> ResyncGate;
    private async UniTask InvokeResyncGate() { if (ResyncGate != null) await ResyncGate(); }

    private void OnEnable()
    {
        InsertGate += InsertItem;
        RemoveAtGate += RemoveAt;
        ModifiedGate += Modified;
        ResyncGate += Resync;
    }
    private void OnDisable()
    {
        InsertGate -= InsertItem;
        RemoveAtGate -= RemoveAt;
        ModifiedGate -= Modified;
        ResyncGate -= Resync;
    }

    protected virtual async UniTask InsertItem(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItemBehaviour(prefabIndex, orderInPool, index);
    }

    protected virtual async UniTask RemoveAt(int index)
    {
        DisableItemBehaviour(index);
    }

    protected virtual async UniTask Modified(int index)
    {
        _activePool[index].GetSimpleView().Refresh();
    }

    protected virtual async UniTask Resync()
        => Sync();

    #endregion

    #region Interact Behaviour

    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> DroppingNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DropNeuron = new();

    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DraggingMoveNeuron = new();

    private void BindInteractBehaviour(LegacyInteractBehaviour ib)
    {
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(PointerEnterNeuron);
        ib.PointerExitNeuron.Join(PointerExitNeuron);
        ib.PointerMoveNeuron.Join(PointerMoveNeuron);
        ib.BeginDragNeuron.Join(BeginDragNeuron);
        ib.EndDragNeuron.Join(EndDragNeuron);
        ib.DragNeuron.Join(DragNeuron);
        ib.LeftClickNeuron.Join(LeftClickNeuron);
        ib.RightClickNeuron.Join(RightClickNeuron);
        ib.DroppingNeuron.Join(DroppingNeuron);
        ib.DropNeuron.Join(DropNeuron);
        
        ib.DraggingEnterNeuron.Join(DraggingEnterNeuron);
        ib.DraggingExitNeuron.Join(DraggingExitNeuron);
        ib.DraggingMoveNeuron.Join(DraggingMoveNeuron);
    }

    #endregion
}
