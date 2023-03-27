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
        bool reveal = slot.IsReveal;

        gameObject.SetActive(reveal);
        if (!reveal) return;

        if(slot.Chip == null)
        {
            LevelText.text = "";
            ManacostText.text = "";
            NameText.text = "空";
            PowerText.text = $"{slot.GetPowerString()}";
            SetColorFromJingJie(JingJie.LianQi);
            return;
        }
        else
        {
            LevelText.text = $"{slot.Chip.Level}";
            ManacostText.text = "";
            NameText.text = $"{slot.Chip.GetName()}";
            PowerText.text = $"{slot.GetPowerString()}";
            SetColorFromJingJie(slot.Chip.GetJingJie());
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
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        EnemyChipSlot to = RunManager.Get<EnemyChipSlot>(GetIndexPath());

        RunChip fromArenaChip = RunManager.Get<RunChip>(drop.GetIndexPath());
        if (fromArenaChip != null)
        {
            if (to.TryWrite(fromArenaChip)) return;
            return;
        }

        AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
        if (fromAcquired != null)
        {
            if (to.TryWrite(fromAcquired)) return;
            return;
        }

        HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
        if (fromHeroChipSlot != null)
        {
            if (to.TryWrite(fromHeroChipSlot)) return;
            return;
        }
    }
}
