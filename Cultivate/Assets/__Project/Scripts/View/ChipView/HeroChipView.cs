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

        HeroChipSlot slot = RunManager.Get<HeroChipSlot>(GetIndexPath());

        gameObject.SetActive(true);
        if(slot.AcquiredRunChip == null)
        {
            LevelText.text = "";
            NameText.text = "空";
            PowerText.text = $"{slot.GetPowerString()}";
            return;
        }
        else
        {
            LevelText.text = $"{slot.GetLevel()}";
            NameText.text = $"{slot.GetName()}";
            PowerText.text = $"{slot.GetPowerString()}";
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
