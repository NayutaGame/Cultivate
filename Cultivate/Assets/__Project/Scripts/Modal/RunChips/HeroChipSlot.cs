using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class HeroChipSlot : ICardModel
{
    public int SlotIndex;
    public Tile XueWei;
    public AcquiredRunChip AcquiredRunChip;

    public bool RunConsumed;

    public bool TryConsume()
    {
        if (!RunConsumed)
            return false;

        AcquiredRunChip toConsume = AcquiredRunChip;
        TryUnequip();
        toConsume.Unplug();
        return true;
    }

    public HeroChipSlot(int slotIndex)
    {
        SlotIndex = slotIndex;
    }

    public RunChip RunChip => AcquiredRunChip?.Chip;

    public Color GetManaCostColor()
        => IsManaShortage() ? Color.red : Color.black;

    public string GetName() => AcquiredRunChip?.GetName() ?? "空";

    public string GetAnnotatedDescription(string evaluated = null)
        => AcquiredRunChip?.GetAnnotatedDescription(evaluated ?? GetDescription()) ?? "";

    public SkillTypeCollection GetSkillTypeCollection()
        => (AcquiredRunChip?.Chip._entry as WaiGongEntry)?.SkillTypeCollection ?? SkillTypeCollection.None;

    public Color GetColor()
        => AcquiredRunChip?.Chip.GetColor() ?? CanvasManager.Instance.JingJieColors[JingJie.LianQi];

    public Sprite GetCardFace()
        => AcquiredRunChip?.Chip.GetCardFace();

    public string GetDescription()
    {
        if (AcquiredRunChip == null)
            return null;

        int[] powers = new int[5];
        WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
        return AcquiredRunChip.Chip._entry.Evaluate(GetJingJie().Value, GetJingJie().Value - AcquiredRunChip.Chip._entry.JingJieRange.Start);
    }

    public string GetAnnotationText()
    {
        if (AcquiredRunChip == null)
            return null;

        StringBuilder sb = new();
        foreach (IAnnotation annotation in AcquiredRunChip.Chip._entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public int GetLevel() => AcquiredRunChip.GetLevel();

    public JingJie? GetJingJie()
    {
        if (AcquiredRunChip == null)
            return null;
        return AcquiredRunChip.GetJingJie();
    }
    public int GetPower(WuXing wuXing)
    {
        int power = 0;
        if (XueWei != null)
            power += XueWei.Powers[wuXing];
        if (AcquiredRunChip != null)
            power += AcquiredRunChip.GetPower(wuXing);
        return power;
    }

    public int GetManaCost()
    {
        if (AcquiredRunChip?.Chip._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), GetJingJie().Value, GetJingJie().Value - AcquiredRunChip.Chip._entry.JingJieRange.Start, powers);
        }

        return 0;
    }

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetPowerString()
    {
        StringBuilder sb = new();
        for (int i = 0; i < WuXing.Length; i++)
        {
            if (GetPower(i) == 0) continue;
            sb.Append($"{GetPower(i)}{(WuXing)i} ");
        }

        return sb.ToString();
    }

    public bool GetReveal()
    {
        int start = RunManager.Instance.Hero.HeroSlotInventory.Start;
        int end = RunManager.Instance.Hero.HeroSlotInventory.Start + RunManager.Instance.Hero.HeroSlotInventory.Limit;
        return start <= SlotIndex && SlotIndex < end;
    }

    public bool TryUnequip()
    {
        AcquiredRunChip toUnequip = AcquiredRunChip;
        if (toUnequip == null)
            return false;

        AcquiredRunChip = null;
        RunManager.Instance.AcquiredInventory.Add(toUnequip);
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryUnequip(AcquiredRunChip acquired)
    {
        AcquiredRunChip toUnequip = AcquiredRunChip;
        if (toUnequip == null)
            return false;

        AcquiredRunChip = acquired;
        int i = RunManager.Instance.AcquiredInventory.IndexOf(acquired);
        RunManager.Instance.AcquiredInventory.RemoveAt(i);
        RunManager.Instance.AcquiredInventory.Insert(i, toUnequip);
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryEquip(AcquiredRunChip toEquip)
    {
        int index = RunManager.Instance.AcquiredInventory.IndexOf(toEquip);

        if (AcquiredRunChip != null)
        {
            RunManager.Instance.AcquiredInventory[index] = AcquiredRunChip;
        }
        else
        {
            RunManager.Instance.AcquiredInventory.RemoveAt(index);
        }

        AcquiredRunChip = toEquip;

        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool IsManaShortage()
    {
        return RunManager.Instance.ManaShortageBrief[SlotIndex];
    }
}
