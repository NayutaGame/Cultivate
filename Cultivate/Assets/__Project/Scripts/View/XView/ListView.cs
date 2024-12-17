
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListView : XView
{
    [SerializeField] private RectTransform _container;
    
    public GameObject[] Prefabs;

    protected List<XView> _activePool;
    protected List<XView>[] _inactivePools;

    private bool _autoSync;

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

    public XView LastView()
        => _activePool[^1];

    public bool IsAutoSync() => _autoSync;

    public void SetAutoSync(bool autoSync)
    {
        _autoSync = autoSync;
        CheckNeurons();
    }

    #endregion

    #region Core

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        InitContainer();
        
        _activePool = new List<XView>();
        _inactivePools = new List<XView>[Prefabs.Length];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<XView>();

        RegisterExists();
    }

    protected virtual void InitContainer()
    {
        if (_container == null)
            _container = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);
        Sync();
    }

    public override void Refresh()
    {
        base.Refresh();
        _activePool.Do(view => view.Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < _container.childCount; i++)
        {
            XView item = _container.GetChild(i).GetComponent<XView>();
            InitItem(item, item.GetItemBehaviour()?.PrefabIndex ?? 0);
        }
    }

    public virtual void Sync()
    {
        DisableAllItems();

        _model = Get<IListModel>();
        for (int i = 0; i < _model.Count(); i++)
            InsertItem(i);

        Refresh();
    }

    private void OnEnable()
    {
        CheckNeurons();
    }

    private void OnDisable()
    {
        CheckNeurons();
    }

    public void ForceLayoutRebuild()
    {
        LayoutGroup layoutGroup = _container.GetComponent<GridLayoutGroup>();
        if (layoutGroup == null)
            return;
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.CalculateLayoutInputVertical();
        layoutGroup.SetLayoutHorizontal();
        layoutGroup.SetLayoutVertical();
    }

    #endregion

    #region Atomic Operations

    private XView AllocItem(int prefabIndex)
        => Instantiate(Prefabs[prefabIndex], _container).GetComponent<XView>();

    protected virtual void InitItem(XView item, int prefabIndex)
    {
        item.GetOrAddComponent<ItemBehaviour>().PrefabIndex = prefabIndex;
        
        item.CheckAwake();
        item.gameObject.SetActive(false);

        _inactivePools[prefabIndex].Add(item);
        BindInteractBehaviour(item.GetInteractBehaviour());
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

    protected virtual void DisableAllItems()
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
    
    protected IListModel Model
    {
        get => _model;
        set
        {
            _model = value;
            CheckNeurons();
        }
    }

    private void CheckNeurons()
    {
        // _model.InsertEvent -= InsertItem;
        // _model.RemoveAtEvent -= RemoveItemAt;
        // _model.ModifiedEvent -= Modified;
        // _model.ResyncEvent -= Resync;
        
        bool isActive = gameObject.activeInHierarchy;
        bool hasModel = _model != null;
        bool autoSync = _autoSync;

        if (!isActive || !hasModel || !autoSync)
            return;
        
        // _model.InsertEvent += InsertItem;
        // _model.RemoveAtEvent += RemoveItemAt;
        // _model.ModifiedEvent += Modified;
        // _model.ResyncEvent += Resync;
    }

    public virtual void AddItem()
    {
        InsertItem(_activePool.Count);
    }

    public virtual void InsertItem(int index)
    {
        object item = _model.Get(index);
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItem(prefabIndex, orderInPool, index);
    }

    public virtual void RemoveItemAt(int index)
    {
        DisableItem(index);
    }

    public virtual void Modified(int index)
    {
        _activePool[index].Refresh();
    }

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
    public Neuron<InteractBehaviour, PointerEventData> DroppingNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropNeuron = new();
    
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingEnterNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingExitNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DraggingMoveNeuron = new();
    
    private void BindInteractBehaviour(InteractBehaviour ib)
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
