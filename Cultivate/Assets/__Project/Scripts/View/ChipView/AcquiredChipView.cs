using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AcquiredChipView : RunChipView
{
    public override void Refresh()
    {
        base.Refresh();

        AcquiredChip chip = RunManager.Get<AcquiredChip>(IndexPath);

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

        if (drop.IndexPath._str == "TryGetUnequipped")
        {
            if (RunManager.Instance.SwapUnequipped(drop.IndexPath, IndexPath)) return;
        }
    }
}
