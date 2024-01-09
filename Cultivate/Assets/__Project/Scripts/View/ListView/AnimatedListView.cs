
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
{
    [SerializeField] private RectTransform AddressBehaviourContainer;

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
        ReparentAddressBehaviour(itemBehaviour);
    }

    private void ReparentAddressBehaviour(ItemBehaviour itemBehaviour)
    {
        RectTransform addressTransform = itemBehaviour.GetAddressBehaviour().Base;
        addressTransform.SetParent(AddressBehaviourContainer);
        addressTransform.name = Traversal().Count().ToString();
    }

    protected override ItemBehaviour EnableItemBehaviour(int prefabIndex, int orderInPool, int index)
    {
        ItemBehaviour itemBehaviour = base.EnableItemBehaviour(prefabIndex, orderInPool, index);
        RectTransform addressTransform = itemBehaviour.GetAddressBehaviour().Base;
        addressTransform.SetSiblingIndex(index);
        addressTransform.gameObject.SetActive(true);

        return itemBehaviour;
    }

    protected override ItemBehaviour DisableItemBehaviour(int index)
    {
        ItemBehaviour itemBehaviour = base.DisableItemBehaviour(index);
        RectTransform addressTransform = itemBehaviour.GetAddressBehaviour().Base;
        addressTransform.SetSiblingIndex(index);
        addressTransform.gameObject.SetActive(true); // TODO: suspicious code

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

            RectTransform addressTransform = itemBehaviour.GetAddressBehaviour().Base;
            addressTransform.gameObject.SetActive(false);
        }
    }

    #endregion

    public void RefreshPivots()
        => _activePool.Do(itemBehaviour => itemBehaviour.RefreshPivots());
}
