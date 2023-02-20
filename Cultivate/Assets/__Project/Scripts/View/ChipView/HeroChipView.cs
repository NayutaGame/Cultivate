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

        AcquiredChip chip = RunManager.Get<AcquiredChip>(IndexPath);

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
        if (eventData.pointerDrag == null)
            return;

        RunChipView drop = eventData.pointerDrag.GetComponent<RunChipView>();
        if (drop == null)
            return;

        if (IndexPath.Equals(drop.IndexPath))
            return;

        if (drop.IndexPath._str == "TryGetUnequipped")
        {
            if (IndexPath._str == "GetHeroNeiGong")
            {
                RunManager.Instance.TryEquipNeiGong(drop.IndexPath, IndexPath);
            }
            else if (IndexPath._str == "GetHeroWaiGong")
            {
                RunManager.Instance.TryEquipWaiGong(drop.IndexPath, IndexPath);
            }
            return;
        }

        if (drop.IndexPath._str == IndexPath._str)
        {
            if (IndexPath._str == "GetHeroNeiGong")
            {
                RunManager.Instance.SwapNeiGong(drop.IndexPath, IndexPath);
            }
            else if (IndexPath._str == "GetHeroWaiGong")
            {
                RunManager.Instance.SwapWaiGong(drop.IndexPath, IndexPath);
            }
            return;
        }
    }
}
