
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class ListView : XView
{
    public GameObject[] Prefabs;
    
    private RectTransform _viewContainer;

    protected List<XView> _activePool;
    protected List<XView>[] _inactivePools;

    #region Accessors

    public IEnumerable<XView> Traversal()
    {
        foreach (var view in _activePool)
            yield return view;
        foreach (var viewList in _inactivePools)
        foreach (var view in viewList)
            yield return view;
    }

    public IEnumerable<XView> TraversalActive()
    {
        foreach (var view in _activePool)
            yield return view;
    }

    public IEnumerable<XBehaviour> Traversal<T>() where T : XBehaviour
    {
        foreach (var view in _activePool)
            yield return view.GetBehaviour<T>();
        foreach (var viewList in _inactivePools)
        foreach (var view in viewList)
            yield return view.GetBehaviour<T>();
    }
    
    public IEnumerable<XBehaviour> TraversalActive<T>() where T : XBehaviour
    {
        foreach (var view in _activePool)
            yield return view.GetBehaviour<T>();
    }

    public int? IndexFromView(XView view)
    {
        if (view == null)
            return null;
        return _activePool.FirstIdx(v => v == view);
    }

    public XView ViewFromIndex(int i)
        => _activePool[i];

    #endregion

    #region List Operations

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _activePool = new List<XView>();
        _inactivePools = new List<XView>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<XView>();

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
        _activePool.Do(view => view.Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < _viewContainer.childCount; i++)
        {
            XView item = _viewContainer.GetChild(i).GetComponent<XView>();
            InitItem(item, item.GetComponent<ItemBehaviour>()?.PrefabIndex ?? 0);
        }
    }

    public virtual void Sync()
    {
        DisableAllItemBehaviours();

        _model = Get<IListModel>();
        for (int i = 0; i < _model.Count(); i++)
            InsertItemStaging(i, GetAddress().Append($"#{i}").Get<object>());

        Refresh();
    }

    #endregion

    #region Atomic Operations

    private XView AllocItem(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], _viewContainer).GetComponent<XView>();

    protected virtual void InitItem(XView item, int prefabIndex)
    {
        item.CheckAwake();
        item.gameObject.SetActive(false);

        item.GetOrAddComponent<ItemBehaviour>().PrefabIndex = prefabIndex;
        _inactivePools[prefabIndex].Add(item);
        // BindInteractBehaviour(itemBehaviour.GetInteractBehaviour());
        item.name = Traversal().Count().ToString();
    }

    protected virtual XView EnableItem(int prefabIndex, int orderInPool, int index)
    {
        List<XView> pool = _inactivePools[prefabIndex];
        XView item = pool[orderInPool];

        pool.RemoveAt(orderInPool);
        _activePool.Insert(index, item);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
            _activePool[i].Refresh();
        }
        
        item.transform.SetSiblingIndex(index);
        item.gameObject.SetActive(true);

        return item;
    }

    protected virtual XView DisableItem(int index)
    {
        XView item = _activePool[index];

        item.gameObject.SetActive(false);
        item.transform.SetAsLastSibling();

        _activePool.RemoveAt(index);
        _inactivePools[item.GetComponent<ItemBehaviour>().PrefabIndex].Add(item);

        for (int i = index; i < _activePool.Count; i++)
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));

        return item;
    }

    protected virtual void DisableAllItemBehaviours()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            XView item = _activePool[index];

            item.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[item.GetComponent<ItemBehaviour>().PrefabIndex].Insert(0, item);
        }
    }

    private int FetchItemBehaviour(int prefabIndex)
    {
        List<XView> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;

        XView item = AllocItem(prefabIndex);
        InitItem(item, prefabIndex);
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

    protected virtual async UniTask InsertItemStaging(int index, object item)
    {
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItem(prefabIndex, orderInPool, index);
    }

    protected virtual async UniTask RemoveAtStaging(int index)
    {
        DisableItem(index);
    }

    protected virtual async UniTask ModifiedStaging(int index)
    {
        _activePool[index].Refresh();
    }

    protected virtual async UniTask Resync()
        => Sync();

    #endregion

    // #region Interact Behaviour
    //
    // public Neuron<InteractBehaviour, PointerEventData> PointerEnterNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> PointerExitNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> PointerMoveNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> BeginDragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> EndDragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> DragNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> LeftClickNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> RightClickNeuron = new();
    // public Neuron<InteractBehaviour, PointerEventData> DroppingNeuron = new();
    // public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();
    //
    // public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    // public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    // public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingMoveNeuron = new();
    //
    // private void BindInteractBehaviour(InteractBehaviour ib)
    // {
    //     if (ib == null)
    //         return;
    //
    //     ib.PointerEnterNeuron.Join(PointerEnterNeuron);
    //     ib.PointerExitNeuron.Join(PointerExitNeuron);
    //     ib.PointerMoveNeuron.Join(PointerMoveNeuron);
    //     ib.BeginDragNeuron.Join(BeginDragNeuron);
    //     ib.EndDragNeuron.Join(EndDragNeuron);
    //     ib.DragNeuron.Join(DragNeuron);
    //     ib.LeftClickNeuron.Join(LeftClickNeuron);
    //     ib.RightClickNeuron.Join(RightClickNeuron);
    //     ib.DroppingNeuron.Join(DroppingNeuron);
    //     ib.DropNeuron.Join(DropNeuron);
    //     
    //     ib.DraggingEnterNeuron.Join(DraggingEnterNeuron);
    //     ib.DraggingExitNeuron.Join(DraggingExitNeuron);
    //     ib.DraggingMoveNeuron.Join(DraggingMoveNeuron);
    // }
    //
    // #endregion
}
