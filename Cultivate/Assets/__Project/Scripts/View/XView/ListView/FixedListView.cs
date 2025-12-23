
using System;
using System.Collections.Generic;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class FixedListView : XView, IListView
{
    public GameObject SlotPrefab;

    public bool AllowHover = true;
    public bool AllowDrag = true;
    
    private RectTransform _slotContainer;
    private RectTransform _viewContainer;

    protected List<SlotView> _slotList;
    
    private IListModel _model;
    
    public InteractNeuronBundle NeuronBundle = new();

    #region Accessors

    public IEnumerable<SlotView> TraversalEverything()
    {
        foreach (var view in _slotList)
            yield return view;
    }

    public IEnumerable<SlotView> TraversalActive()
    {
        foreach (var view in _slotList)
            if (view.gameObject.activeSelf)
                yield return view;
    }

    public IEnumerable<XBehaviour> TraversalEverything<T>() where T : XBehaviour
    {
        foreach (var view in _slotList)
            yield return view.GetBehaviour<T>();
    }
    
    public IEnumerable<XBehaviour> TraversalActive<T>() where T : XBehaviour
    {
        foreach (var view in _slotList)
            if (view.gameObject.activeSelf)
                yield return view.GetBehaviour<T>();
    }

    public int? IndexFromView(SlotView view)
    {
        if (view == null)
            return null;
        return _slotList.FirstIdx(v => v == view);
    }

    public int? IndexFromView(Predicate<SlotView> pred)
    {
        return _slotList.FirstIdx(pred);
    }

    public SlotView ViewFromIndex(int i)
        => _slotList[i];

    public SlotView LastView()
        => _slotList[^1];

    public int GetCount() => _slotList.Count;

    #endregion

    #region Core

    protected override void AwakeFunction()
    {
        base.AwakeFunction();

        if (SlotPrefab != null)
            SlotPrefab.SetActive(false);
        
        _slotList = new List<SlotView>();

        InitContainer();
        RegisterExists();
        DisableAllItems();
    }

    protected virtual void InitContainer()
    {
        if (_slotContainer == null)
            _slotContainer = transform.GetChild(0).GetComponent<RectTransform>();
        
        if (_viewContainer == null)
            _viewContainer = transform.GetChild(1).GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Sync();
        RefreshPivots();
    }

    public override void Refresh()
    {
        base.Refresh();
        TraversalActive().Do(slot => slot.Refresh());
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
        BindContentBehaviour(slotView, contentView);
        BindSlotAndContent(slotView, contentView);
    }

    private void InitItemWithContentView(XView contentView)
    {
        contentView.gameObject.SetActive(false);
        contentView.CheckAwake();
        
        SlotView slotView = AllocSlotView();
        BindItemBehaviour(slotView);
        BindContentBehaviour(slotView, contentView);
        BindSlotAndContent(slotView, contentView);
    }

    public virtual void Sync()
    {
        DisableAllItems();

        _model = Get<IListModel>();
        if (_model == null)
            return;

        int itemCount = _model.Count();
        int slotCount = _slotList.Count;
        
        Assert.IsTrue(itemCount <= slotCount);
        for (int i = 0; i < itemCount; i++)
            AlignEnabledState(i);

        Refresh();
        
        RefreshPivotsAsync();
    }

    public void Modified(int index)
    {
        _slotList[index].Refresh();
    }

    public void RemoveItemAt(int index)
    {
        DisableItem(index);
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

    private void BindItemBehaviour(SlotView slotView)
    {
        ItemBehaviour itemBehaviour = slotView.GetOrAddComponent<ItemBehaviour>();
        slotView.JoinBehaviour(itemBehaviour);
    }

    private void BindContentBehaviour(SlotView slotView, XView contentView)
    {
        ContentBehaviour contentBehaviour = contentView.GetOrAddComponent<ContentBehaviour>();
        contentView.JoinBehaviour(contentBehaviour);
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
        
        _slotList.Add(slotView);
        
        slotView.GetInteractBehaviour().NeuronBundle.Join(NeuronBundle);
        slotView.SetParentListView(this);
        ReparentItem(slotRect, contentRect);
    }
    
    private void ReparentItem(RectTransform slotRect, RectTransform contentRect)
    {
        slotRect.SetParent(_slotContainer);
        slotRect.SetAsLastSibling();
        slotRect.name = GetCount().ToString();
        contentRect.SetParent(_viewContainer);
        contentRect.SetAsLastSibling();
        contentRect.name = GetCount().ToString();
    }

    private void AlignEnabledState(int index)
    {
        var item = _model.Get(index);
        
        _slotList[index].SetAddress(GetAddress().Append($"#{index}"));
        if (item != null)
            EnableItem(index);
    }

    protected virtual SlotView EnableItem(int index)
    {
        SlotView slotView = _slotList[index];
        XView contentView = slotView.GetContentView();
        
        slotView.Refresh();
        slotView.gameObject.SetActive(true);
        contentView.gameObject.SetActive(true);
        
        return slotView;
    }

    protected virtual SlotView DisableItem(int index)
    {
        SlotView slotView = _slotList[index];
        XView contentView = slotView.GetContentView();
        slotView.gameObject.SetActive(false);
        contentView.gameObject.SetActive(false);
        return slotView;
    }
    
    protected virtual void DisableAllItems()
    {
        int slotCount = _slotList.Count;
        for (int i = 0; i < slotCount; i++)
            DisableItem(i);
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
        => _slotList.Do(view =>
        {
            view.GetAnimator().SetStateAsync(SlotView.IDLE);
        });
    
    public virtual void RefreshPivots()
        => _slotList.Do(view =>
        {
            view.GetAnimator().SetState(SlotView.IDLE);
        });

    #endregion
}