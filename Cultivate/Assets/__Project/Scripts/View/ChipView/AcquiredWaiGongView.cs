using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AcquiredWaiGongView : RunChipView
{
    public override void Refresh()
    {
        base.Refresh();

        AcquiredRunChip acquiredRunChip = RunManager.Get<AcquiredRunChip>(GetIndexPath());

        gameObject.SetActive(acquiredRunChip != null);
        if (acquiredRunChip == null) return;

        LevelText.text = $"{acquiredRunChip.GetLevel()}";
        ManacostText.text = $"{acquiredRunChip.GetManaCost()}";
        NameText.text = $"{acquiredRunChip.GetName()}";
        PowerText.text = $"{acquiredRunChip.GetPowerString()}";

        SetColorFromJingJie(acquiredRunChip.GetJingJie());
    }

    public override void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        AcquiredRunChip to = RunManager.Get<AcquiredRunChip>(GetIndexPath());

        AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
        if (fromAcquired != null)
        {
            if (RunManager.Instance.AcquiredInventory.Swap(fromAcquired, to)) return;
            return;
        }

        HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
        if (fromHeroChipSlot != null)
        {
            if (fromHeroChipSlot.TryUnequip(to)) return;
            return;
        }
    }
}
