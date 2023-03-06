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

        RunChip chip = RunManager.Get<RunChip>(GetIndexPath());

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

        RunCanvas.Instance.ChipPreview.Configure(null);
        RunCanvas.Instance.ChipPreview.Refresh();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null)
            return;

        if (GetIndexPath().Equals(drop.GetIndexPath()))
            return;

        if (RunManager.Instance.TryWrite(drop.GetIndexPath(), GetIndexPath())) return;
    }
}
