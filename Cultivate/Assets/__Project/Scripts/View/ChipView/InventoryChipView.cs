using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryChipView : RunChipView
{
    public override void Refresh()
    {
        base.Refresh();

        RunChip chip = RunManager.Get<RunChip>(GetIndexPath());

        gameObject.SetActive(chip != null);
        if (chip == null) return;

        LevelText.text = $"{chip.Level}";
        ManacostText.text = $"{chip.GetManaCost()}";
        NameText.text = $"{chip.GetName()}";
        PowerText.text = "";
        SetColorFromJingJie(chip.JingJie);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        // if (eventData.pointerDrag == null) return;
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        RunChip to = RunManager.Get<RunChip>(GetIndexPath());

        RunChip from = RunManager.Get<RunChip>(drop.GetIndexPath());
        if (from != null)
        {
            if (RunManager.Instance.ChipInventory.Swap(from, to)) return;
        }
    }
}
