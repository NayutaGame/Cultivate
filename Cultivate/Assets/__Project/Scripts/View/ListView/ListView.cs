
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListView : SimpleView
{
    public RectTransform Container;
    public GameObject[] Prefabs;

    protected List<ItemBehaviour> _activePool;
    public List<ItemBehaviour> ActivePool => _activePool;
    protected List<ItemBehaviour>[] _inactivePools;

    #region List Operations

    public IEnumerable<ItemBehaviour> Traversal()
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour;
        foreach (var itemBehaviourList in _inactivePools)
        foreach (var itemBehaviour in itemBehaviourList)
            yield return itemBehaviour;
    }

    public IEnumerable<ItemBehaviour> TraversalActive()
    {
        foreach (var itemBehaviour in _activePool)
            yield return itemBehaviour;
    }

    private bool IsInited()
        => _activePool != null;

    private void Init()
    {
        _activePool = new List<ItemBehaviour>();
        _inactivePools = new List<ItemBehaviour>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<ItemBehaviour>();

        RegisterExists();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        if (!IsInited())
            Init();

        Model = Get<IListModel>();
        Sync();
    }

    public override void Refresh()
    {
        base.Refresh();
        _activePool.Do(itemBehaviour => itemBehaviour.GetSimpleView().Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            ItemBehaviour itemBehaviour = RegisterItemBehaviour(Container.GetChild(i).gameObject);
            InitItemBehaviour(itemBehaviour, itemBehaviour.PrefabIndex);
        }
    }

    public virtual void Sync()
    {
        DisableAllItemBehaviours();

        for (int i = 0; i < _model.Count(); i++)
            InsertItem(i, GetAddress().Append($"#{i}").Get<object>());

        Refresh();
    }

    #endregion

    #region Atomic Operations

    private ItemBehaviour AllocItemBehaviour(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], Container).GetComponent<ItemBehaviour>();

    private ItemBehaviour RegisterItemBehaviour(GameObject go)
        => go.GetComponent<ItemBehaviour>();

    protected virtual void InitItemBehaviour(ItemBehaviour itemBehaviour, int prefabIndex)
    {
        itemBehaviour.GetComponent<CLView>().Awake();
        itemBehaviour.gameObject.SetActive(false);

        itemBehaviour.PrefabIndex = prefabIndex;
        _inactivePools[prefabIndex].Add(itemBehaviour);
        BindInteractBehaviour(itemBehaviour);
        itemBehaviour.name = Traversal().Count().ToString();
    }

    protected virtual ItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        List<ItemBehaviour> pool = _inactivePools[prefabIndex];
        ItemBehaviour itemBehaviour = pool[orderInPool];

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

    protected virtual ItemBehaviour DisableItemBehaviour(int index)
    {
        ItemBehaviour itemBehaviour = _activePool[index];

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
            ItemBehaviour itemBehaviour = _activePool[index];

            itemBehaviour.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemBehaviour.PrefabIndex].Insert(0, itemBehaviour);
        }
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
            }
            _model = value;
            if (_model != null)
            {
                _model.InsertEvent += InvokeInsertGate;
                _model.RemoveAtEvent += InvokeRemoveAtGate;
                _model.ModifiedEvent += InvokeModifiedGate;
            }
        }
    }

    private event Func<int, object, Task> InsertGate;
    private async Task InvokeInsertGate(int index, object item) { if (InsertGate != null) await InsertGate(index, item); }
    private event Func<int, Task> RemoveAtGate;
    private async Task InvokeRemoveAtGate(int index) { if (RemoveAtGate != null) await RemoveAtGate(index); }
    private event Func<int, Task> ModifiedGate;
    private async Task InvokeModifiedGate(int index) { if (ModifiedGate != null) await ModifiedGate(index); }

    private void OnEnable()
    {
        InsertGate += InsertItem;
        RemoveAtGate += RemoveAt;
        ModifiedGate += Modified;
    }
    private void OnDisable()
    {
        InsertGate -= InsertItem;
        RemoveAtGate -= RemoveAt;
        ModifiedGate -= Modified;
    }

    protected virtual async Task InsertItem(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItemBehaviour(prefabIndex, orderInPool, index);
    }

    protected virtual async Task RemoveAt(int index)
    {
        DisableItemBehaviour(index);
    }

    protected virtual async Task Modified(int index)
    {
        _activePool[index].GetSimpleView().Refresh();
    }

    #endregion

    #region Helper Operations

    private int FetchItemBehaviour(int prefabIndex)
    {
        List<ItemBehaviour> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;

        ItemBehaviour itemBehaviour = AllocItemBehaviour(prefabIndex);
        InitItemBehaviour(itemBehaviour, prefabIndex);
        return 0;
    }

    public int? IndexFromItemBehaviour(ItemBehaviour toGetIndex)
    {
        if (toGetIndex == null)
            return null;
        return _activePool.FirstIdx(itemBehaviour => itemBehaviour == toGetIndex);
    }

    public ItemBehaviour ItemBehaviourFromIndex(int i)
        => _activePool[i];

    #endregion

    #region Interact Behaviour

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();

    private void BindInteractBehaviour(ItemBehaviour itemBehaviour)
    {
        InteractBehaviour ib = itemBehaviour.GetInteractBehaviour();
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
        ib.DropNeuron.Join(DropNeuron);
    }

    #endregion
}
