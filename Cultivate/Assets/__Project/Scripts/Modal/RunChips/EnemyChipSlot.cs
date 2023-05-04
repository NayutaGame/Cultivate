using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChipSlot : ICardModel
{
    public int SlotIndex;
    private int[] _powers;
    public RunChip Chip;

    public bool IsReveal;

    public bool GetReveal()
        => IsReveal;

    public EnemyChipSlot(int slotIndex)
    {
        SlotIndex = slotIndex;
        _powers = new int[WuXing.Length];
        IsReveal = true;
    }

    public int GetManaCost()
    {
        if (Chip?._entry is WaiGongEntry waigongEntry)
        {
            int[] powers = new int[5];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
            return waigongEntry.GetManaCost(GetLevel(), GetJingJie(), GetJingJie() - Chip._entry.JingJieRange.Start, powers);
        }

        return 0;
    }

    public Color GetManaCostColor()
        => Color.black;

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetName()
        => Chip?.GetName() ?? "空";

    public string GetAnnotatedDescription(string evaluated = null)
        => Chip?.GetAnnotatedDescription(evaluated ?? GetDescription()) ?? "";

    public SkillTypeCollection GetSkillTypeCollection()
        => (Chip?._entry as WaiGongEntry)?.SkillTypeCollection ?? SkillTypeCollection.None;

    public Color GetColor()
        => Chip?.GetColor() ?? CanvasManager.Instance.JingJieColors[JingJie.LianQi];

    public Sprite GetCardFace()
        => Chip?._entry.CardFace;

    public string GetDescription()
    {
        if (Chip == null)
            return null;

        int[] powers = new int[5];
        WuXing.Traversal.Do(wuXing => powers[wuXing] = GetPower(wuXing));
        return Chip._entry.Evaluate(GetJingJie(), GetJingJie() - Chip._entry.JingJieRange.Start);
    }

    public string GetAnnotationText()
    {
        if (Chip == null)
            return null;

        StringBuilder sb = new();
        foreach (IAnnotation annotation in Chip._entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public int GetLevel() => Chip.Level;
    public int GetPower(WuXing wuXing) => _powers[wuXing];
    public void SetPower(WuXing wuXing, int value) => _powers[wuXing] = value;
    public JingJie GetJingJie()
        => Chip.JingJie;

    public string GetJingJieString()
        => Chip?.GetJingJie()._index.ToString() ?? "null";

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

    public bool TryWrite(RunChip chip)
    {
        Chip = chip.Clone();
        // RunManager.Instance.EquippedChanged();
        return true;
    }

    public bool TryWrite(AcquiredRunChip acquired)
    {
        Chip = acquired.Chip.Clone();
        WuXing.Traversal.Do(wuXing => SetPower(wuXing, acquired.GetPower(wuXing)));
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryWrite(HeroChipSlot heroChipSlot)
    {
        Chip = heroChipSlot.RunChip?.Clone();
        WuXing.Traversal.Do(wuXing => SetPower(wuXing, heroChipSlot.GetPower(wuXing)));
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }

    public bool TryIncreseJingJie()
    {
        if (Chip == null)
            return false;

        JingJie curr = Chip.JingJie;
        JingJie next = curr + 1;
        if (!Chip._entry.JingJieRange.Contains(next))
            next = Chip._entry.JingJieRange.Start;
        Chip.JingJie = next;
        RunManager.Instance.StageEnvironmentChanged();
        return true;
    }
}
