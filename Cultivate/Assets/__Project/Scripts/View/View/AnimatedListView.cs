
using System.Linq;
using Cysharp.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
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

    protected override void InitItemBehaviour(ItemBehaviour itemBehaviour, int prefabIndex)
    {
        base.InitItemBehaviour(itemBehaviour, prefabIndex);
        ReparentSimpleView(itemBehaviour);
    }

    private void ReparentSimpleView(ItemBehaviour itemBehaviour)
    {
        RectTransform displayTransform = itemBehaviour.GetView().RectTransform;
        displayTransform.SetParent(Displays);
        displayTransform.name = Traversal().Count().ToString();
    }

    protected override ItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        ItemBehaviour itemBehaviour = base.EnableItemBehaviour(prefabIndex, orderInPool, index);

        RectTransform displayTransform = itemBehaviour.GetView().RectTransform;
        displayTransform.SetSiblingIndex(index);
        displayTransform.gameObject.SetActive(true);

        return itemBehaviour;
    }

    protected override ItemBehaviour DisableItemBehaviour(int index)
    {
        ItemBehaviour itemBehaviour = base.DisableItemBehaviour(index);

        RectTransform displayTransform = itemBehaviour.GetView().RectTransform;
        displayTransform.gameObject.SetActive(false);
        displayTransform.SetAsLastSibling();

        return itemBehaviour;
    }

    protected override void DisableAllItemBehaviours()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            ItemBehaviour itemBehaviour = _activePool[index];

            itemBehaviour.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemBehaviour.PrefabIndex].Insert(0, itemBehaviour);

            RectTransform displayTransform = itemBehaviour.GetView().RectTransform;
            displayTransform.gameObject.SetActive(false);
        }
    }

    #endregion

    public void RefreshPivots()
        => _activePool.Do(itemBehaviour =>
        {
            XBehaviourPivot xBehaviourPivot = itemBehaviour.GetBehaviour<XBehaviourPivot>();
            if (xBehaviourPivot != null)
                xBehaviourPivot.RefreshPivots();
        });
}
