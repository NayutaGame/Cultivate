
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

    public void RefreshPivotsAsync()
        => _activePool.Do(view =>
        {
            DelegatingView delegatingView = view as DelegatingView;
            delegatingView.GetAnimator().SetStateAsync(1);
        });
    
    public void RefreshPivots()
        => _activePool.Do(view =>
        {
            DelegatingView delegatingView = view as DelegatingView;
            delegatingView.GetAnimator().SetState(1);
        });

    public void RecoverDelegatingView(DelegatingView delegatingView)
    {
        delegatingView.GetDelegatedView().GetRect().SetParent(_viewContainer);
        int siblingIndex = IndexFromView(delegatingView).Value;
        delegatingView.GetDelegatedView().GetRect().SetSiblingIndex(siblingIndex);
    }
}
