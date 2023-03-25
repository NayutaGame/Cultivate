using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroChipView : RunChipView
{
    private bool _reveal;

    public override void Refresh()
    {
        base.Refresh();

        HeroChipSlot slot = RunManager.Get<HeroChipSlot>(GetIndexPath());
        _reveal = slot.IsReveal();

        gameObject.SetActive(_reveal);
        if (!_reveal) return;

        gameObject.SetActive(true);
        if(slot.AcquiredRunChip == null)
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
            LevelText.text = $"{slot.GetLevel()}";
            ManacostText.text = $"{slot.GetManaCost()}";

            bool manaShortage = slot.IsManaShortage();
            ManacostText.color = manaShortage ? Color.red : Color.black;

            NameText.text = $"{slot.GetName()}";
            PowerText.text = $"{slot.GetPowerString()}";

            SetColorFromJingJie(slot.RunChip.GetJingJie());
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        IIndexPath drop = eventData.pointerDrag.GetComponent<IIndexPath>();
        if (drop == null) return;
        if (GetIndexPath().Equals(drop.GetIndexPath())) return;

        HeroChipSlot to = RunManager.Get<HeroChipSlot>(GetIndexPath());

        AcquiredRunChip fromAcquired = RunManager.Get<AcquiredRunChip>(drop.GetIndexPath());
        if (fromAcquired != null)
        {
            if (to.TryEquip(fromAcquired)) return;
            return;
        }

        HeroChipSlot fromHeroChipSlot = RunManager.Get<HeroChipSlot>(drop.GetIndexPath());
        if (fromHeroChipSlot != null)
        {
            if (RunManager.Instance.Hero.HeroSlotInventory.Swap(fromHeroChipSlot, to)) return;
            return;
        }
    }
}
