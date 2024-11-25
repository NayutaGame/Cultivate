
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class LegacyAnimatedListView : LegacyListView
{
    [SerializeField] private RectTransform Displays;

    protected override UniTask InsertItem(int index, object item)
    {
        UniTask task = base.InsertItem(index, item);
        RefreshPivots();
        return task;
    }

    protected override UniTask RemoveAt(int index)
    {
        var task = base.RemoveAt(index);
        RefreshPivots();
        return task;
    }

    #region Atomic Operations

    protected override void InitItemBehaviour(LegacyItemBehaviour itemBehaviour, int prefabIndex)
    {
        base.InitItemBehaviour(itemBehaviour, prefabIndex);
        ReparentSimpleView(itemBehaviour);
    }

    private void ReparentSimpleView(LegacyItemBehaviour itemBehaviour)
    {
        RectTransform displayTransform = itemBehaviour.GetDisplayTransform();
        displayTransform.SetParent(Displays);
        displayTransform.name = Traversal().Count().ToString();
    }

    protected override LegacyItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        LegacyItemBehaviour itemBehaviour = base.EnableItemBehaviour(prefabIndex, orderInPool, index);

        RectTransform displayTransform = itemBehaviour.GetDisplayTransform();
        displayTransform.SetSiblingIndex(index);
        displayTransform.gameObject.SetActive(true);

        return itemBehaviour;
    }

    protected override LegacyItemBehaviour DisableItemBehaviour(int index)
    {
        LegacyItemBehaviour itemBehaviour = base.DisableItemBehaviour(index);

        RectTransform displayTransform = itemBehaviour.GetDisplayTransform();
        displayTransform.gameObject.SetActive(false);
        displayTransform.SetAsLastSibling();

        return itemBehaviour;
    }

    protected override void DisableAllItemBehaviours()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            LegacyItemBehaviour itemBehaviour = _activePool[index];

            itemBehaviour.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemBehaviour.PrefabIndex].Insert(0, itemBehaviour);

            RectTransform displayTransform = itemBehaviour.GetDisplayTransform();
            displayTransform.gameObject.SetActive(false);
        }
    }

    #endregion

    public void RefreshPivots()
        => _activePool.Do(itemBehaviour =>
        {
            LegacyPivotBehaviour pivotBehaviour = itemBehaviour.GetBehaviour<LegacyPivotBehaviour>();
            if (pivotBehaviour != null)
                pivotBehaviour.RefreshPivots();
        });
}
