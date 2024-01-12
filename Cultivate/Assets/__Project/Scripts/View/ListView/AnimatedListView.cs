
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
{
    [SerializeField] private RectTransform SimpleViews;

    protected override Task InsertItem(int index, object item)
    {
        Task task = base.InsertItem(index, item);
        RefreshPivots();
        return task;
    }

    protected override Task RemoveAt(int index)
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
        RectTransform baseTransform = itemBehaviour.GetSimpleView().GetDisplayTransform();
        baseTransform.SetParent(SimpleViews);
        baseTransform.name = Traversal().Count().ToString();
    }

    protected override ItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        ItemBehaviour itemBehaviour = base.EnableItemBehaviour(prefabIndex, orderInPool, index);
        RectTransform baseTransform = itemBehaviour.GetBaseTransform();
        baseTransform.SetSiblingIndex(index);
        baseTransform.gameObject.SetActive(true);

        return itemBehaviour;
    }

    protected override ItemBehaviour DisableItemBehaviour(int index)
    {
        ItemBehaviour itemBehaviour = base.DisableItemBehaviour(index);
        RectTransform baseTransform = itemBehaviour.GetBaseTransform();
        baseTransform.SetSiblingIndex(index);
        baseTransform.gameObject.SetActive(true); // TODO: suspicious code

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

            RectTransform baseTransform = itemBehaviour.GetBaseTransform();
            baseTransform.gameObject.SetActive(false);
        }
    }

    #endregion

    public void RefreshPivots()
        => _activePool.Do(itemBehaviour =>
        {
            StateBehaviourPivot stateBehaviourPivot = itemBehaviour.GetSimpleView().GetStateBehaviour() as StateBehaviourPivot;
            if (stateBehaviourPivot != null)
                stateBehaviourPivot.RefreshPivots();
        });
}
