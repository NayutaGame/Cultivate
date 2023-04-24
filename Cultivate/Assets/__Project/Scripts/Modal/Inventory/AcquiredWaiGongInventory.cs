using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquiredWaiGongInventory: Inventory<AcquiredRunChip>
{
    public bool TryMerge(AcquiredRunChip lhs, AcquiredRunChip rhs)
    {
        if (lhs.GetJingJie() != rhs.GetJingJie())
            return false;

        JingJie jingJie = lhs.GetJingJie();
        WuXing? lWuXing = lhs.Chip._entry.WuXing;
        WuXing? rWuXing = rhs.Chip._entry.WuXing;

        bool upgrade;

        if (jingJie == JingJie.FanXu) {
            upgrade = false;
        } else if (jingJie == JingJie.HuaShen) {
            // upgrade = RandomManager.value < 0.05;
            upgrade = false;
        } else {
            upgrade = true;
        }

        JingJie newJingJie = jingJie + (upgrade ? 1 : 0);

        if (lhs.Chip._entry == rhs.Chip._entry && upgrade && lhs.Chip._entry.JingJieRange.Contains(lhs.Chip.JingJie + 1))
        {
            lhs.Chip.JingJie = newJingJie;
            rhs.Unplug();
        }
        else if (!lWuXing.HasValue || !rWuXing.HasValue)
        {
            return false;
        }
        else if (WuXing.SameWuXing(lWuXing, rWuXing))
        {
            RunManager.Instance.TryDrawAcquired(chipEntry =>
                chipEntry is WaiGongEntry waiGongEntry && waiGongEntry.JingJieRange.Contains(newJingJie) &&
                waiGongEntry.WuXing == lWuXing && waiGongEntry != lhs.Chip._entry && waiGongEntry != rhs.Chip._entry,
                newJingJie);

            lhs.Unplug();
            rhs.Unplug();
        }
        else if (WuXing.XiangSheng(lWuXing, rWuXing))
        {
            RunManager.Instance.TryDrawAcquired(chipEntry =>
                chipEntry is WaiGongEntry waiGongEntry && waiGongEntry.JingJieRange.Contains(newJingJie) &&
                WuXing.XiangShengNext(lWuXing, rWuXing).Value == waiGongEntry.WuXing,
                newJingJie);

            lhs.Unplug();
            rhs.Unplug();
        }
        else
        {
            RunManager.Instance.TryDrawAcquired(chipEntry =>
                chipEntry is WaiGongEntry waiGongEntry && waiGongEntry.JingJieRange.Contains(newJingJie) &&
                waiGongEntry.WuXing.HasValue && waiGongEntry.WuXing != lWuXing && waiGongEntry.WuXing != rWuXing,
                newJingJie);

            lhs.Unplug();
            rhs.Unplug();
        }

        return true;
    }
}
