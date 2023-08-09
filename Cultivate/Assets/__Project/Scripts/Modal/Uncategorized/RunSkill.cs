using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class RunSkill : ISkillModel, ISerializationCallbackReceiver
{
    [SerializeField] private SkillEntry _entry;
    public SkillEntry Entry => _entry;
    [SerializeField] private JingJie _jingJie;
    public JingJie JingJie
    {
        get => _jingJie;
        set => _jingJie = value;
    }
    [SerializeField] protected int _runUsedTimes;
    public int RunUsedTimes => _runUsedTimes;
    [SerializeField] protected int _runEquippedTimes;
    public int RunEquippedTimes => _runEquippedTimes;

    public RunSkill(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = jingJie;
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
        => _entry.Name;

    public string GetAnnotatedDescription(string evaluated = null)
        => _entry.GetAnnotatedDescription(evaluated ?? GetDescription());

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
        => _entry.Evaluate(JingJie, JingJie - _entry.JingJieRange.Start);

    public int GetManaCost()
        => _entry.GetManaCost(JingJie, JingJie - _entry.JingJieRange.Start);

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public RunSkill Clone()
        => new(this);

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.Name) ? null : Encyclopedia.SkillCategory[_entry.Name];
    }

    public bool TryIncreaseJingJie(bool loop = true)
    {
        if (Entry.JingJieRange.Contains(JingJie + 1))
        {
            JingJie += 1;
        }
        else if (loop)
        {
            JingJie = Entry.JingJieRange.Start;
        }

        return true;
    }
}
