
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedListView : ListView
{
    [SerializeField] private Transform PivotList;
    [SerializeField] private GameObject PivotPrefab;

    public override void Sync()
    {
        PutAllIntoPool();

        for (int i = 0; i < Model.Count(); i++)
        {
            Address address = GetAddress().Append($"#{i}");
            int prefabIndex = GetPrefabIndex(address.Get<object>());
            FetchItemView(out AddressBehaviour itemView, prefabIndex);
            EnableItemView(itemView, i);
            itemView.SetAddress(address);
        }

        Refresh();
    }

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
        Traversal().Do(behaviour => behaviour.GetComponent<AnimatedItemView>().InteractBehaviour.SetHandler(_interactHandler));
    }

    protected override void BindItemView(AddressBehaviour behaviour, int prefabIndex = 0)
    {
        behaviour.GetComponent<ItemView>().PrefabIndex = prefabIndex;

        AnimatedItemView animatedItemView = behaviour.GetComponent<AnimatedItemView>();

        if (animatedItemView == null)
            return;

        if (animatedItemView.InteractBehaviour != null)
            return;

        InteractBehaviour interactBehaviour = Instantiate(PivotPrefab, PivotList).GetComponent<InteractBehaviour>();
        interactBehaviour.SetHandler(_interactHandler);
        animatedItemView.InteractBehaviour = interactBehaviour;
        interactBehaviour.AddressBehaviour = behaviour;
    }

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
