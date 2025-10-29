
using System;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

[Serializable]
public sealed class SkillReference : AnnotatableSkill, ISerializationCallbackReceiver
{
    [SerializeField] private SkillEntry _entry;
    [SerializeField] private JingJie _jingJie;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    private SkillReference(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        _jingJie = jingJie;
    }

    public SkillEntry GetEntry() => _entry;
    public JingJie GetJingJie() => _jingJie;

    public static SkillReference FromEntry(SkillEntry entry)
        => new(entry, entry.LowestJingJie);

    public static SkillReference FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry, jingJie);

    public static SkillReference FromRunSkill(RunSkill runSkill)
        => new(runSkill.GetEntry(), runSkill.GetJingJie());

    public SkillReference Clone()
        => new(_entry, _jingJie);

    public JingJie GetLowestJingJie() => _entry.GetLowestJingJie();
    public JingJie GetHighestJingJie() => _entry.GetHighestJingJie();
    public UnityEngine.Sprite GetCardIllustration() => _entry.GetCardIllustration();
    public UnityEngine.Sprite GetBarIllustration() => _entry.GetBarIllustration();
    public string GetName() => _entry.GetName();
    public Description GetDescription(JingJie showingJingJie) => _entry.GetDescription(showingJingJie);
    public string GetTrivia() => _entry.GetTrivia();
    public UnityEngine.Sprite GetJingJieSprite(JingJie showingJingJie) => _entry.GetJingJieSprite(showingJingJie);
    public TagComposite GetTagComposite() => _entry.GetTagComposite();
    public PackEntry GetPackEntry() => _entry.GetPackEntry();
    public bool CanShowAnnotation() => _entry != null;
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => _entry?.GetLiteralCostDescription(showingJingJie) ?? CostDescription.Empty;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.SkillCategory.FromId(_entry.GetId());
        _jingJie = string.IsNullOrEmpty(_jingJie.GetId()) ? null : Encyclopedia.JingJieCategory.FromId(_jingJie.GetId());
    }
}