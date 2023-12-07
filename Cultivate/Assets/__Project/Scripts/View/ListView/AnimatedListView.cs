
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class AnimatedListView : ListView
{
    [SerializeField] private Transform PivotHolder;

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

    public override void SetHandler(InteractHandler interactHandler)
    {
        _interactHandler = interactHandler;
        Traversal().Do(itemView => itemView.GetComponent<AnimatedItemView>().InteractBehaviour.SetHandler(_interactHandler));
    }

    #region Atomic Operations

    protected override void InitItemView(ItemView itemView, int prefabIndex)
    {
        base.InitItemView(itemView, prefabIndex);

        AnimatedItemView animatedItemView = itemView.GetComponent<AnimatedItemView>();
        if (animatedItemView == null)
            return;

        InteractBehaviour interactBehaviour = animatedItemView.InteractBehaviour;
        interactBehaviour.gameObject.transform.SetParent(PivotHolder);
        interactBehaviour.gameObject.name = Traversal().Count().ToString();
        interactBehaviour.SetHandler(_interactHandler);
    }

    protected override ItemView EnableItemView(int prefabIndex, int orderInPool, int index)
    {
        ItemView itemView = base.EnableItemView(prefabIndex, orderInPool, index);

        AnimatedItemView animatedItemView = itemView as AnimatedItemView;
        animatedItemView.PivotTransform.SetSiblingIndex(index);
        animatedItemView.PivotTransform.gameObject.SetActive(true);

        return itemView;
    }

    protected override ItemView DisableItemView(int index)
    {
        ItemView itemView = base.DisableItemView(index);

        AnimatedItemView animatedItemView = itemView as AnimatedItemView;
        animatedItemView.PivotTransform.SetAsLastSibling();
        animatedItemView.PivotTransform.gameObject.SetActive(false);

        return itemView;
    }

    protected override void DisableAllItemViews()
    {
        while (_activePool.Count != 0)
        {
            int index = _activePool.Count - 1;
            ItemView itemView = _activePool[index];

            itemView.gameObject.SetActive(false);

            _activePool.RemoveAt(index);
            _inactivePools[itemView.PrefabIndex].Insert(0, itemView);

            AnimatedItemView animatedItemView = itemView as AnimatedItemView;
            animatedItemView.PivotTransform.gameObject.SetActive(false);
        }
    }

    #endregion

    private void RefreshPivots()
    {
        foreach (var addressBehaviour in _activePool)
        {
            AnimatedItemView animatedItemView = addressBehaviour.GetComponent<AnimatedItemView>();

            if(animatedItemView != null)
                animatedItemView.InteractBehaviour.SetPivot(animatedItemView.InteractBehaviour.PivotBehaviour.IdlePivot);
        }
    }
}
