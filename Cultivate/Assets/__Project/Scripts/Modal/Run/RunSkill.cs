using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class RunSkill : EmulatedSkill, ISerializationCallbackReceiver
{
    [SerializeField] private SkillSlot _skillSlot;
    public SkillSlot GetSkillSlot() => _skillSlot;
    public void SetSkillSlot(SkillSlot value) => _skillSlot = value;

    [SerializeField] private SkillEntry _entry;
    public SkillEntry GetEntry() => _entry;

    [SerializeField] private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set => _jingJie = value;
    }

    [SerializeField] protected int _runUsedTimes;
    public int GetRunUsedTimes() => _runUsedTimes;
    public void SetRunUsedTimes(int value) => _runEquippedTimes = value;

    [SerializeField] protected int _runEquippedTimes;
    public int GetRunEquippedTimes() => _runEquippedTimes;
    public void SetRunEquippedTimes(int value) => _runEquippedTimes = value;

    public static RunSkill From(SkillEntry entry, JingJie jingJie)
        => new(entry, jingJie);

    private RunSkill(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = Mathf.Clamp(jingJie, _entry.LowestJingJie, _entry.HighestJingJie);
    }

    private RunSkill(RunSkill prototype)
    {
        _entry = prototype._entry;
        _jingJie = prototype._jingJie;
        _runUsedTimes = prototype._runUsedTimes;
        _runEquippedTimes = prototype._runEquippedTimes;
    }

    public Sprite GetSprite()
        => _entry.Sprite;

    public string GetName()
        => _entry.GetName();

    public SkillTypeComposite GetSkillTypeComposite()
        => _entry.SkillTypeComposite;

    public JingJie GetJingJie()
        => JingJie;

    public Color GetColor()
        => CanvasManager.Instance.JingJieColors[GetJingJie()];

    public Sprite GetCardFace()
        => _entry.CardFace;

    public Sprite GetJingJieSprite()
        => CanvasManager.Instance.JingJieSprites[GetJingJie()];

    public Sprite GetWuXingSprite()
        => CanvasManager.Instance.GetWuXingSprite(_entry.WuXing);

    public string GetDescription()
        => _entry.DescriptionFromJingJie(JingJie);

    public int GetManaCost()
        => _entry.GetManaCost(JingJie, JingJie - _entry.LowestJingJie, GetSkillSlot()?.JiaShiIndicator ?? false);

    public int GetChannelTime()
        => _entry.GetChannelTime(JingJie, JingJie - _entry.LowestJingJie, GetSkillSlot()?.JiaShiIndicator ?? false);

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetAnnotatedDescription(string evaluated = null)
        => _entry.GetAnnotatedDescription(evaluated ?? GetDescription());

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in GetEntry().GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetAnnotatedDescription()}\n\n");

        return sb.ToString();
    }

    public string GetTrivia()
        => GetEntry().GetTrivia();

    public RunSkill Clone()
        => new(this);

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.SkillCategory[_entry.GetName()];
    }

    public bool TryIncreaseJingJie(bool loop = true)
    {
        if (GetEntry().JingJieContains(JingJie + 1))
        {
            JingJie += 1;
        }
        else if (loop)
        {
            JingJie = GetEntry().LowestJingJie;
        }

        return true;
    }

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;
}
