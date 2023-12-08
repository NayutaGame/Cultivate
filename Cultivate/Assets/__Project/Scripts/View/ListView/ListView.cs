
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class ListView : AddressBehaviour
{
    public Transform Container;
    public GameObject[] Prefabs;

    protected List<ItemView> _activePool;
    public List<ItemView> ActivePool => _activePool;
    protected List<ItemView>[] _inactivePools;

    public IEnumerable<ItemView> Traversal()
    {
        foreach (var itemView in _activePool)
            yield return itemView;
        foreach (var itemViewList in _inactivePools)
        foreach (var itemView in itemViewList)
            yield return itemView;
    }

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];
    protected int GetPrefabIndex(object model) => _prefabProvider?.Invoke(model) ?? 0;

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

    protected InteractHandler _interactHandler;
    public InteractHandler GetHandler() => _interactHandler;
    public virtual void SetHandler(InteractHandler interactHandler)
    {
        _interactHandler = interactHandler;
        if (IsInited())
            Traversal().Do(itemView => itemView.GetComponent<InteractBehaviour>()?.SetHandler(_interactHandler));
    }

    #region Atomic Operations

    private ItemView AllocItemView(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], Container).GetComponent<ItemView>();

    private ItemView RegisterItemView(GameObject go)
    {
        go.SetActive(false);
        return go.GetComponent<ItemView>();
    }

    protected virtual void InitItemView(ItemView itemView, int prefabIndex)
    {
        itemView.PrefabIndex = prefabIndex;
        _inactivePools[prefabIndex].Add(itemView);
        itemView.GetComponent<InteractBehaviour>()?.SetHandler(_interactHandler);
        itemView.gameObject.name = Traversal().Count().ToString();
    }

    protected virtual ItemView EnableItemView(int prefabIndex, int orderInPool, int index)
    {
        List<ItemView> pool = _inactivePools[prefabIndex];
        ItemView itemView = pool[orderInPool];

        pool.RemoveAt(orderInPool);
        _activePool.Insert(index, itemView);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].AddressBehaviour.SetAddress(GetAddress().Append($"#{i}"));
            _activePool[i].AddressBehaviour.Refresh();
        }

        itemView.transform.SetSiblingIndex(index);
        itemView.gameObject.SetActive(true);

        return itemView;
    }

    protected virtual ItemView DisableItemView(int index)
    {
        ItemView itemView = _activePool[index];

        itemView.gameObject.SetActive(false);
        itemView.transform.SetAsLastSibling();

        _activePool.RemoveAt(index);
        _inactivePools[itemView.PrefabIndex].Add(itemView);

        for (int i = index; i < _activePool.Count; i++)
            _activePool[i].AddressBehaviour.SetAddress(GetAddress().Append($"#{i}"));

        return itemView;
    }

    protected virtual void DisableAllItemViews()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            ItemView itemView = _activePool[index];

            itemView.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemView.PrefabIndex].Insert(0, itemView);
        }
    }

    #endregion

    // helper operations

    private int FetchItemView(int prefabIndex)
    {
        List<ItemView> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;

        ItemView itemView = AllocItemView(prefabIndex);
        InitItemView(itemView, prefabIndex);
        return 0;
    }

    protected virtual async Task InsertItem(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemView(prefabIndex);
        EnableItemView(prefabIndex, orderInPool, index);
    }

    protected virtual async Task RemoveAt(int index)
    {
        DisableItemView(index);
    }

    protected virtual async Task Modified(int index)
    {
        _activePool[index].AddressBehaviour.Refresh();
    }

    // operations for list

    private bool IsInited()
        => _activePool != null;

    private void Init()
    {
        Prefabs.Do(prefab => prefab.SetActive(false));

        _activePool = new List<ItemView>();
        _inactivePools = new List<ItemView>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<ItemView>();

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
        _activePool.Do(itemView => itemView.AddressBehaviour.Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            ItemView itemView = RegisterItemView(Container.GetChild(i).gameObject);
            InitItemView(itemView, itemView.PrefabIndex);
        }
    }

    public virtual void Sync()
    {
        DisableAllItemViews();

        for (int i = 0; i < _model.Count(); i++)
            InsertItem(i, GetAddress().Append($"#{i}").Get<object>());

        Refresh();
    }
}
