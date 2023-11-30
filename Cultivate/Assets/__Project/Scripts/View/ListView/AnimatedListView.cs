
using System.Threading.Tasks;
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
            FetchItemView(out ItemView itemView, prefabIndex);
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

    protected override void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        base.BindItemView(itemView, prefabIndex);

        if (itemView is AnimatedItemView animatedItemView)
        {
            if (animatedItemView.InteractBehaviour != null)
                return;

            InteractBehaviour interactBehaviour = Instantiate(PivotPrefab, PivotList).GetComponent<InteractBehaviour>();
            animatedItemView.InteractBehaviour = interactBehaviour;
            interactBehaviour.AddressBehaviour = itemView;
        }
    }

    private void RefreshPivots()
    {
        foreach (var itemView in _activePool)
            if (itemView is AnimatedItemView animatedItemView)
                animatedItemView.InteractBehaviour.SetPivot(animatedItemView.InteractBehaviour.PivotBehaviour.IdlePivot);
    }
}
