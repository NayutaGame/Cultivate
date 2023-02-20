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

        RunChip chip = RunManager.Get<RunChip>(IndexPath);

        gameObject.SetActive(chip != null);
        if (chip == null) return;

        InfoText.text = $"{chip.GetName()}[{chip.Level}]";
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (IndexPath.Equals(drop.IndexPath))
            return;

        if (drop.IndexPath._str == "TryGetRunChip")
        {
            if (RunManager.Instance.TryUpgradeInventory(drop.IndexPath, IndexPath)) return;
            if (RunManager.Instance.Swap(drop.IndexPath, IndexPath)) return;
        }
    }
}
