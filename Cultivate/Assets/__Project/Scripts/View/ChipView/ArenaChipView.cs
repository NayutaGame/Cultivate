using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArenaChipView : RunChipView
{
    public override void OnDrop(PointerEventData eventData) { }

    public override void Refresh()
    {
        base.Refresh();

        RunChip chip = RunManager.Get<RunChip>(GetIndexPath());

        gameObject.SetActive(chip != null);
        if (chip == null) return;

        LevelText.text = $"{chip.Level}";
        ManacostText.text = $"{chip.GetManaCost()}";
        NameText.text = $"{chip.GetName()}";
        PowerText.text = "";
        SetColorFromJingJie(chip.GetJingJie());
    }
}
