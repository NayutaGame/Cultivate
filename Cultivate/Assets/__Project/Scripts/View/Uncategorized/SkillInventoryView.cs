using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInventoryView : InventoryView<AbstractSkillView>, IDropHandler
{
    public Button[] DrawButtons;
    public Button[] SortButtons;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        DrawButtons.Length.Do(i =>
        {
            JingJie jingJie = i;
            DrawButtons[i].onClick.AddListener(() => DrawJingJie(jingJie));
        });

        SortButtons.Length.Do(i =>
        {
            int comparisonId = i;
            SortButtons[i].onClick.AddListener(() => SortByComparisonId(comparisonId));
        });
    }

    public void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        IInteractable from = RunManager.Get<IInteractable>(drop.GetIndexPath());
        IInteractable to = RunManager.Get<IInteractable>(GetIndexPath());

        from.GetInteractDelegate().DragDrop(from, to);
    }

    private void DrawJingJie(JingJie jingJie)
    {
        SkillInventory inventory = RunManager.Get<SkillInventory>(GetIndexPath());
        inventory.TryDrawSkill(out _, jingJie: jingJie);
        RunCanvas.Instance.Refresh();
    }

    private void SortByComparisonId(int i)
    {
        SkillInventory inventory = RunManager.Get<SkillInventory>(GetIndexPath());
        inventory.SortByComparisonId(i);
        RunCanvas.Instance.Refresh();
    }

    // private void ClearChip()
    // {
    //     RunManager.Instance.ClearChip();
    //     Refresh();
    // }
}
