
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class ListView : AddressBehaviour
{
    public Transform Container;
    public GameObject[] Prefabs;

    protected List<ItemView> _activePool;
    public List<ItemView> ActivePool => _activePool;
    private List<ItemView>[] _inactivePools;

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

    protected virtual async Task InsertItem(int index, object item)
    {
        Address address = GetAddress().Append($"#{index}");
        int prefabIndex = GetPrefabIndex(address.Get<object>());
        FetchItemView(out ItemView itemView, prefabIndex);

        _activePool.Insert(index, itemView);
        _activePool[index].SetAddress(address);
        _activePool[index].Refresh();
        itemView.gameObject.SetActive(true);

        for (int i = index + 1; i < _activePool.Count; i++)
        {
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
        }
    }

    protected virtual async Task RemoveAt(int index)
    {
        PutIntoPool(index);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
        }
    }

    protected virtual async Task Modified(int index)
    {
        _activePool[index].Refresh();
    }

    private InteractHandler _interactHandler;
    public InteractHandler GetHandler() => _interactHandler;
    public virtual void SetHandler(InteractHandler interactHandler)
    {
        _interactHandler = interactHandler;
        Traversal().Do(v => v.GetComponent<InteractBehaviour>()?.SetHandler(_interactHandler));
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        _activePool = new List<ItemView>();
        _inactivePools = new List<ItemView>[Prefabs.Length];

        Prefabs.Do(prefab => prefab.SetActive(false));

        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<ItemView>();
        RegisterExists();
        Model = Get<IListModel>();
        Sync();
    }

    private void RegisterExists()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            ItemView itemView = Container.GetChild(i).GetComponent<ItemView>();
            itemView.gameObject.SetActive(false);

            BindItemView(itemView, itemView.PrefabIndex);
            _activePool.Add(itemView);
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        _activePool.Do(v => v.Refresh());
    }

    public virtual void Sync()
    {
        PutAllIntoPool();

        for (int i = 0; i < _model.Count(); i++)
        {
            Address address = GetAddress().Append($"#{i}");
            int prefabIndex = GetPrefabIndex(address.Get<object>());
            FetchItemView(out ItemView itemView, prefabIndex);
            EnableItemView(itemView, i);
            itemView.SetAddress(address);
        }

        Refresh();
    }

    private void PutIntoPool(int index)
    {
        ItemView itemView = _activePool[index];
        _activePool.RemoveAt(index);

        DisableItemView(itemView);
    }

    protected void PutAllIntoPool()
    {
        for (int i = 0; i < Container.childCount; i++)
            while (_activePool.Count != 0)
            {
                ItemView itemView = _activePool[0];
                _activePool.RemoveAt(0);

                DisableItemView(itemView);
            }
    }

    protected bool FetchItemView(out ItemView itemView, int prefabIndex)
    {
        var pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
        {
            itemView = pool[0];
            pool.RemoveAt(0);
            return true;
        }

        GameObject prefab = Prefabs[prefabIndex];
        itemView = Instantiate(prefab, Container).GetComponent<ItemView>();
        BindItemView(itemView, prefabIndex);
        return false;
    }

    protected virtual void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        itemView.PrefabIndex = prefabIndex;
        itemView.GetComponent<InteractBehaviour>()?.SetHandler(_interactHandler);
    }

    protected void EnableItemView(ItemView itemView, int siblingIndex)
    {
        itemView.transform.SetSiblingIndex(siblingIndex);
        _activePool.Add(itemView);
        itemView.gameObject.SetActive(true);
    }

    protected void DisableItemView(ItemView itemView)
    {
        itemView.gameObject.SetActive(false);
        _inactivePools[itemView.PrefabIndex].Add(itemView);
        itemView.transform.SetAsLastSibling();
    }
}
