
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedListView : ListView, IDropHandler
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

    protected override Task Modified(int index)
    {
        return base.Modified(index);
    }

    protected override void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        base.BindItemView(itemView, prefabIndex);

        if (itemView is HandSkillView handSkillView)
        {
            if (handSkillView.PivotPropagate != null)
                return;

            PivotPropagate pivotPropagate = Instantiate(PivotPrefab, PivotList).GetComponent<PivotPropagate>();
            handSkillView.PivotPropagate = pivotPropagate;
            pivotPropagate.BindingView = handSkillView.GetComponent<IInteractable>();
        }
    }

    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);

    private void RefreshPivots()
    {
        foreach (var itemView in _activePool)
            if (itemView is HandSkillView handSkillView)
                handSkillView.GoToPivot(handSkillView.PivotPropagate.IdlePivot);
    }
}
