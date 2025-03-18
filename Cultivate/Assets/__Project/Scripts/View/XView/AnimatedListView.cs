
using System.Linq;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
{
    [SerializeField] private RectTransform _viewContainer;
    
    protected override void InitContainer()
    {
        base.InitContainer();
        if (_viewContainer == null)
            _viewContainer = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public override void InsertItem(int index)
    {
        base.InsertItem(index);
        RefreshPivotsAsync();
    }

    public override void RemoveItemAt(int index)
    {
        base.RemoveItemAt(index);
        RefreshPivotsAsync();
    }

    #region Atomic Operations

    protected override void InitItem(XView item, int prefabIndex)
    {
        base.InitItem(item, prefabIndex);
        DelegatingView delegatingView = item as DelegatingView;
        delegatingView.SetParentListView(this);
        ReparentItem(item);
    }

    private void ReparentItem(XView item)
    {
        XView delegatedView = (item as DelegatingView).GetDelegatedView();
        RectTransform rect = delegatedView.GetRect();
        rect.SetParent(_viewContainer);
        rect.name = Traversal().Count().ToString();
        rect.gameObject.SetActive(false);
    }

    protected override XView EnableItem(int prefabIndex, int orderInPool, int index)
    {
        XView item = base.EnableItem(prefabIndex, orderInPool, index);

        XView delegatedView = (item as DelegatingView).GetDelegatedView();
        RectTransform rect = delegatedView.GetRect();
        rect.SetSiblingIndex(index);
        rect.gameObject.SetActive(true);

        return item;
    }

    private bool CheckLayoutIsApplied()
    {
        // 检测DelegatingView是否受到了Layout的影响
        foreach (var view in _activePool)
        {
            DelegatingView delegatingView = view as DelegatingView;
            RectTransform delegatingRect = delegatingView.GetRect();
            Vector2 anchoredPosition = delegatingRect.anchoredPosition;
            
            // 如果DelegatingView的位置不为0，说明受到了Layout的影响
            if (anchoredPosition != Vector2.zero)
                return false;
        }

        return true;
    }

    protected override XView DisableItem(int index)

    {
        XView item = base.DisableItem(index);
        
        XView delegatedView = (item as DelegatingView).GetDelegatedView();
        RectTransform rect = delegatedView.GetRect();
        rect.gameObject.SetActive(false);
        rect.SetAsLastSibling();

        return item;
    }

    protected override void DisableAllItems()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            XView item = _activePool[index];

            item.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[item.GetComponent<ItemBehaviour>().PrefabIndex].Insert(0, item);

            XView delegatedView = (item as DelegatingView).GetDelegatedView();
            RectTransform rect = delegatedView.GetRect();
            rect.gameObject.SetActive(false);
        }
    }

    #endregion

    public virtual void RefreshPivotsAsync()
        => _activePool.Do(view =>
        {
            DelegatingView delegatingView = view as DelegatingView;
            delegatingView.GetAnimator().SetStateAsync(1);
        });
    
    public virtual void RefreshPivots()
        => _activePool.Do(view =>
        {
            DelegatingView delegatingView = view as DelegatingView;
            delegatingView.GetAnimator().SetState(1);
        });

    public void RecoverDelegatingView(DelegatingView delegatingView)
    {
        delegatingView.GetDelegatedView().GetRect().SetParent(_viewContainer);
        int? index = IndexFromView(delegatingView);
        if (!index.HasValue)
            return;
        int siblingIndex = index.Value;
        delegatingView.GetDelegatedView().GetRect().SetSiblingIndex(siblingIndex);
    }
}
