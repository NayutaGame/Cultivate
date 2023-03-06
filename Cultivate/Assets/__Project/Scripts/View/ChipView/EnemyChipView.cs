using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyChipView : RunChipView
{
    public override void Refresh()
    {
        base.Refresh();

        RunChip chip = RunManager.Get<RunChip>(IndexPath);

        gameObject.SetActive(true);
        if(chip == null)
        {
            InfoText.text = "空";
            return;
        }
        else
        {
            InfoText.text = $"{chip.GetName()}[{chip.Level}]";
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag = null;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (IndexPath.Equals(drop.IndexPath))
            return;

        if (RunManager.Instance.TryCopy(drop.IndexPath, IndexPath)) return;
    }
}
