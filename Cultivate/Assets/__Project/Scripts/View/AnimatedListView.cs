
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatedListView : ListView, IDropHandler
{
    [SerializeField] private Transform PivotHolder;
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
        foreach (var itemView in _activePool)
            if (itemView is HandSkillView handSkillView)
                handSkillView.GoToPivot(handSkillView.HandPivot.IdlePivot);
        return task;
    }

    protected override Task RemoveAt(int index)
    {
        var task = base.RemoveAt(index);
        foreach (var itemView in _activePool)
            if (itemView is HandSkillView handSkillView)
                handSkillView.GoToPivot(handSkillView.HandPivot.IdlePivot);
        return task;
    }

    protected override Task Modified(int index)
    {
        return base.Modified(index);
    }

    protected override void BindItemView(ItemView itemView, int prefabIndex = 0)
    {
        base.BindItemView(itemView, prefabIndex);

        HandPivot pivot = Instantiate(PivotPrefab, PivotHolder).GetComponent<HandPivot>();
        (itemView as HandSkillView).HandPivot = pivot;
        pivot.BindingView = itemView.GetComponent<IInteractable>();
    }

    public virtual void OnDrop(PointerEventData eventData) => GetDelegate()?.DragDrop(eventData.pointerDrag.GetComponent<IInteractable>(), this);
}
