
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
    public GameObject SlotPrefab;
    public GameObject[] Prefabs;

    public bool AllowHover = true;
    public bool AllowDrag = true;
    
    public bool AutoUpdateItem = true;
    public bool AutoUpdateLayout = true;
    
    private RectTransform _slotContainer;
    private RectTransform _viewContainer;
    private LayoutGroup _layoutGroup;

    protected List<SlotView> _activePool;
    private List<SlotView>[] _inactivePools;

    #region Accessors

    public LayoutGroup GetLayoutGroup() => _layoutGroup;

    public IEnumerable<SlotView> Traversal()
    {
        foreach (var view in _activePool)
            yield return view;
        foreach (var viewList in _inactivePools)
        foreach (var view in viewList)
            yield return view;
    }

    public IEnumerable<SlotView> TraversalActive()
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

    public int? IndexFromView(SlotView view)
    {
        if (view == null)
            return null;
        return _activePool.FirstIdx(v => v == view);
    }

    public int? IndexFromView(Predicate<SlotView> pred)
    {
        return _activePool.FirstIdx(pred);
    }

    public SlotView ViewFromIndex(int i)
        => _activePool[i];

    public SlotView LastView()
        => _activePool[^1];

    public bool IsAutoSync() => AutoUpdateItem;
    public void SetAutoSync(bool autoSync)
    {
        AutoUpdateItem = autoSync;
        CheckNeurons();
    }

    public void EnableAutoUpdateLayout() => AutoUpdateLayout = true;
    public void DisableAutoUpdateLayout() => AutoUpdateLayout = false;

    public int GetCount() => _activePool.Count;

    #endregion

    #region Interface

    public Neuron ItemCountChanged = new();

    #endregion
    
    #region Core

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _activePool = new List<SlotView>();
        _inactivePools = new List<SlotView>[Prefabs.Length.ClampLower(1)];
        for (int i = 0; i < _inactivePools.Length; i++)
            _inactivePools[i] = new List<SlotView>();

        InitContainer();
        RegisterExists();
    }

    protected virtual void InitContainer()
    {
        if (_slotContainer == null)
            _slotContainer = transform.GetChild(0).GetComponent<RectTransform>();
        
        if (_viewContainer == null)
            _viewContainer = transform.GetChild(1).GetComponent<RectTransform>();
        
        _layoutGroup ??= _slotContainer.GetComponent<LayoutGroup>();
    }

    private void OnEnable()
    {
        Sync();
    }

    public override void Refresh()
    {
        base.Refresh();
        _activePool.Do(view => view.Refresh());
    }

    private void RegisterExists()
    {
        for (int i = 0; i < _slotContainer.childCount; i++)
        {
            XView view = _slotContainer.GetChild(0).GetComponent<XView>();
            if (view is SlotView slotView)
                InitItemWithComposed(slotView);
            else
                InitItemWithContentView(view);
        }
    }

    private void InitItemWithComposed(SlotView slotView)
    {
        XView contentView = slotView.GetContentView();
        contentView.gameObject.SetActive(false);
        contentView.CheckAwake();
        
        slotView.gameObject.SetActive(false);
        slotView.CheckAwake();
        BindItemBehaviour(slotView);
        
        BindSlotAndContent(slotView, contentView);
    }

    private void InitItemWithContentView(XView contentView)
    {
        contentView.gameObject.SetActive(false);
        contentView.CheckAwake();
        
        SlotView slotView = AllocSlotView();
        BindItemBehaviour(slotView);
        
        BindSlotAndContent(slotView, contentView);
    }

    private void InitDynamically(int prefabIndex)
    {
        XView contentView = AllocContentView(prefabIndex);
        
        SlotView slotView = AllocSlotView();
        BindItemBehaviour(slotView, prefabIndex);
        
        BindSlotAndContent(slotView, contentView);
    }
    
    

    public virtual void Sync()
    {
        DisableAllItems();

        _model = Get<IListModel>();
        if (_model == null)
            return;
        for (int i = 0; i < _model.Count(); i++)
            InnerInsertItem(i);

        Refresh();
        
        ItemCountChanged.Invoke();
        if (AutoUpdateLayout)
            RefreshPivotsAsync();
    }

    public void AddItem()
    {
        InsertItem(_activePool.Count);
    }

    public void InsertItem(int index)
    {
        InnerInsertItem(index);
        ItemCountChanged.Invoke();
        if (AutoUpdateLayout)
            RefreshPivotsAsync();
    }

    private void InnerInsertItem(int index)
    {
        object item = _model.Get(index);
        int prefabIndex = GetPrefabIndex(item);
        int orderInPool = FetchItemBehaviour(prefabIndex);
        EnableItem(prefabIndex, orderInPool, index);
    }

    public void RemoveItemAt(int index)
    {
        InnerRemoveItemAt(index);
        ItemCountChanged.Invoke();
        if (AutoUpdateLayout)
            RefreshPivotsAsync();
    }

    public void RemoveLast()
        => RemoveItemAt(_activePool.Count - 1);

    private void InnerRemoveItemAt(int index)
    {
        DisableItem(index);
    }

    public void Modified(int index)
    {
        _activePool[index].Refresh();
    }

    public void ForceLayoutRebuild()
    {
        if (_layoutGroup == null)
            return;
            
        _layoutGroup.CalculateLayoutInputHorizontal();
        _layoutGroup.CalculateLayoutInputVertical();
        _layoutGroup.SetLayoutHorizontal();
        _layoutGroup.SetLayoutVertical();
    }

    #endregion

    #region Atomic Operations

    private SlotView AllocSlotView()
    {
        SlotView slotView = Instantiate(SlotPrefab, _slotContainer).GetComponent<SlotView>();
        slotView.gameObject.SetActive(false);
        slotView.CheckAwake();
        return slotView;
    }

    private XView AllocContentView(int prefabIndex)
    {
        XView contentView = Instantiate(Prefabs[prefabIndex], _viewContainer).GetComponent<XView>();
        contentView.gameObject.SetActive(false);
        contentView.CheckAwake();
        return contentView;
    }

    private void BindItemBehaviour(SlotView slotView, int? prefabIndex = null)
    {
        ItemBehaviour itemBehaviour = slotView.GetOrAddComponent<ItemBehaviour>();
        itemBehaviour.PrefabIndex = prefabIndex ?? 0;
        slotView.JoinBehaviour(itemBehaviour);
    }

    private void BindSlotAndContent(SlotView slotView, XView contentView)
    {
        slotView.AllowHover = AllowHover;
        slotView.AllowDrag = AllowDrag;
        slotView.SetContentView(contentView);
        
        RectTransform slotRect = slotView.GetRect();
        RectTransform contentRect = contentView.GetRect();

        slotRect.sizeDelta = contentRect.sizeDelta;
        slotRect.anchorMin = contentRect.anchorMin;
        slotRect.anchorMax = contentRect.anchorMax;
        slotRect.pivot = contentRect.pivot;
        slotRect.anchoredPosition = contentRect.anchoredPosition;

        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);

        int prefabIndex = slotView.GetBehaviour<ItemBehaviour>().PrefabIndex;
        _inactivePools[prefabIndex].Add(slotView);
        BindInteractBehaviour(slotView.GetInteractBehaviour());
        slotView.SetParentListView(this);
        ReparentItem(slotRect, contentRect);
    }
    
    private void ReparentItem(RectTransform slotRect, RectTransform contentRect)
    {
        slotRect.SetParent(_slotContainer);
        slotRect.SetAsLastSibling();
        slotRect.name = Traversal().Count().ToString();
        contentRect.SetParent(_viewContainer);
        contentRect.SetAsLastSibling();
        contentRect.name = Traversal().Count().ToString();
    }

    protected virtual XView EnableItem(int prefabIndex, int orderInPool, int index)
    {
        List<SlotView> pool = _inactivePools[prefabIndex];
        SlotView slotView = pool[orderInPool];

        pool.RemoveAt(orderInPool);
        _activePool.Insert(index, slotView);

        for (int i = index; i < _activePool.Count; i++)
        {
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
            _activePool[i].Refresh();
        }
        
        slotView.transform.SetSiblingIndex(index);
        slotView.gameObject.SetActive(true);
        
        

        
        
        XView contentView = slotView.GetContentView();
        RectTransform contentRect = contentView.GetRect();
        contentRect.SetSiblingIndex(index);
        contentView.gameObject.SetActive(true);

        return slotView;
    }

    protected virtual SlotView DisableItem(int index)
    {
        SlotView slotView = _activePool[index];

        slotView.gameObject.SetActive(false);
        // grabber
        slotView.transform.SetAsLastSibling();

        _activePool.RemoveAt(index);
        _inactivePools[slotView.GetComponent<ItemBehaviour>().PrefabIndex].Add(slotView);

        for (int i = index; i < _activePool.Count; i++)
            _activePool[i].SetAddress(GetAddress().Append($"#{i}"));
        
        
        
        
        
        
        
        XView contentView = slotView.GetContentView();
        RectTransform contentRect = contentView.GetRect();
        contentView.gameObject.SetActive(false);
        contentRect.SetAsLastSibling();

        return slotView;
    }
    
    protected virtual void DisableAllItems()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            SlotView slotView = _activePool[index];

            slotView.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[slotView.GetComponent<ItemBehaviour>().PrefabIndex].Insert(0, slotView);

            XView contentView = slotView.GetContentView();
            RectTransform rect = contentView.GetRect();
            rect.gameObject.SetActive(false);
        }
    }

    private int FetchItemBehaviour(int prefabIndex)
    {
        List<SlotView> pool = _inactivePools[prefabIndex];
        if (pool.Count != 0)
            return 0;
        
        InitDynamically(prefabIndex);
        return 0;
    }

    public void RecoverSlotView(SlotView slotView)
    {
        slotView.GetContentView().GetRect().SetParent(_viewContainer);
        int? index = IndexFromView(slotView);
        if (!index.HasValue)
            return;
        int siblingIndex = index.Value;
        slotView.GetContentView().GetRect().SetSiblingIndex(siblingIndex);
    }

    public virtual void RefreshPivotsAsync()
        => _activePool.Do(view =>
        {
            view.GetAnimator().SetStateAsync(1);
        });
    
    public virtual void RefreshPivots()
        => _activePool.Do(view =>
        {
            view.GetAnimator().SetState(1);
        });

    #endregion

    #region Prefab Provider

    private Func<object, int> _prefabProvider;
    public void SetPrefabProvider(Func<object, int> prefabProvider) => _prefabProvider = prefabProvider;
    private GameObject GetPrefab(object item) => Prefabs[_prefabProvider?.Invoke(item) ?? 0];
    protected int GetPrefabIndex(object model) => _prefabProvider?.Invoke(model) ?? 0;

    #endregion

    #region Model Delegates

    private IListModel _model;

    private void CheckNeurons()
    {
        // _model.InsertEvent -= InsertItem;
        // _model.RemoveAtEvent -= RemoveItemAt;
        // _model.ModifiedEvent -= Modified;
        // _model.ResyncEvent -= Resync;
        
        bool isActive = gameObject.activeInHierarchy;
        bool hasModel = _model != null;
        bool autoSync = AutoUpdateItem;

        if (!isActive || !hasModel || !autoSync)
            return;
        
        // _model.InsertEvent += InsertItem;
        // _model.RemoveAtEvent += RemoveItemAt;
        // _model.ModifiedEvent += Modified;
        // _model.ResyncEvent += Resync;
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
