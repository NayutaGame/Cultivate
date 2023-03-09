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

        HeroRunChip heroRunChip = RunManager.Get<HeroRunChip>(GetIndexPath());

        gameObject.SetActive(true);
        if(heroRunChip == null)
        {
            LevelText.text = "";
            NameText.text = "空";
            PowerText.text = "";
            return;
        }
        else
        {
            LevelText.text = $"{heroRunChip.GetLevel()}";
            NameText.text = $"{heroRunChip.GetName()}";
            PowerText.text = $"{heroRunChip.GetPowerString()}";
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
