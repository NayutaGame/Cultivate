using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArenaEquippedView : RunChipView
{
    public override void OnDrop(PointerEventData eventData) { }

    public override void Refresh()
    {
        base.Refresh();

        EnemyChipSlot slot = RunManager.Get<EnemyChipSlot>(GetIndexPath());
        bool reveal = slot.IsReveal();

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
}
