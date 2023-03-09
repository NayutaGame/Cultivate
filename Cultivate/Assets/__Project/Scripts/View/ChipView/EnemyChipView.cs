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

        EnemyChipSlot slot = RunManager.Get<EnemyChipSlot>(GetIndexPath());

        gameObject.SetActive(true);
        if(slot.Chip == null)
        {
            LevelText.text = "";
            NameText.text = "空";
            PowerText.text = $"{slot.GetPowerString()}";
            return;
        }
        else
        {
            LevelText.text = $"{slot.Chip.Level}";
            NameText.text = $"{slot.Chip.GetName()}";
            PowerText.text = $"{slot.GetPowerString()}";
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
