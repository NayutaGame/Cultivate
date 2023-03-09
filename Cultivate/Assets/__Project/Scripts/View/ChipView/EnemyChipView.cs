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
            LevelText.text = "";
            NameText.text = "空";
            PowerText.text = "";
            return;
        }
        else
        {
            LevelText.text = $"{chip.Level}";
            NameText.text = $"{chip.GetName()}";
            // PowerText.text = $"{chip.GetPowerString()}";
            PowerText.text = $"";
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
