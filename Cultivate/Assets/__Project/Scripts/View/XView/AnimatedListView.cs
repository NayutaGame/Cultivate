
using System.Linq;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
{
    private RectTransform _viewContainer;
    
    protected override void InitContainer()
    {
        base.InitContainer();
        _viewContainer = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public override void InsertItem(int index, object item)
    {
        base.InsertItem(index, item);
        RefreshPivots();
    }

    public override void RemoveItemAt(int index)
    {
        base.RemoveItemAt(index);
        RefreshPivots();
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

    protected override void DisableAllItemBehaviours()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            XView item = _activePool[index];

            item.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[item.GetItemBehaviour().PrefabIndex].Insert(0, item);

            XView delegatedView = (item as DelegatingView).GetDelegatedView();
            RectTransform rect = delegatedView.GetRect();
            rect.gameObject.SetActive(false);
        }
    }

    #endregion

    public void RefreshPivots()
        => _activePool.Do(view =>
        {
            // LegacyPivotBehaviour pivotBehaviour = itemBehaviour.GetBehaviour<LegacyPivotBehaviour>();
            // if (pivotBehaviour != null)
            //     pivotBehaviour.RefreshPivots();
        });

    public void RecoverDelegatingView(DelegatingView delegatingView)
    {
        delegatingView.GetDelegatedView().GetRect().SetParent(_viewContainer);
        int siblingIndex = IndexFromView(delegatingView).Value;
        delegatingView.GetDelegatedView().GetRect().SetSiblingIndex(siblingIndex);
    }
}
