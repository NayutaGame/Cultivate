using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroChipView : RunChipView
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

    public override void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null)
            return;

        if (GetIndexPath().Equals(drop.GetIndexPath()))
            return;

        if (RunManager.Instance.TryDrag(drop.GetIndexPath(), GetIndexPath())) return;
    }
}
