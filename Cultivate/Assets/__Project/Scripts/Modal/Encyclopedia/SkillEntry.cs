using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, IAnnotation
{
    public string GetName()
        => Name;

    private CLLibrary.Range _jingJieRange;
    public CLLibrary.Range JingJieRange => _jingJieRange;

    private SkillDescription _description;

    private IAnnotation[] _annotations;
    public IAnnotation[] GetAnnotations() => _annotations;

    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;

    private Sprite _cardFace;
    public Sprite CardFace
    {
        get
        {
            if (_cardFace != null)
                return _cardFace;

            _cardFace = _wuXing.HasValue ? CanvasManager.Instance.CardFaces[_wuXing.Value] : null;
            return _cardFace;
        }
    }

    private ManaCost _manaCost;
    public int GetManaCost(JingJie jingJie, int dJingJie) => _manaCost.Eval(jingJie, dJingJie);

    public SkillTypeCollection SkillTypeCollection { get; private set; }
    private Func<StageEntity, StageSkill, bool, Task> _execute;

    public SkillEntry(string name,
        CLLibrary.Range jingJieRange,
        SkillDescription description,
        WuXing? wuXing = null,
        ManaCost manaCost = null,
        SkillTypeCollection skillTypeCollection = null,
        Func<StageEntity, StageSkill, bool, Task> execute = null
        ) : base(name)
    {
        _jingJieRange = jingJieRange;
        _description = description;
        _wuXing = wuXing;
        _manaCost = manaCost ?? 0;
        SkillTypeCollection = skillTypeCollection ?? SkillTypeCollection.None;
        _execute = execute ?? DefaultExecute;
    }

    public static implicit operator SkillEntry(string name) => Encyclopedia.SkillCategory[name];

    public string Evaluate(int j, int dj) => _description.Eval(j, dj);

    public void Generate()
    {
        string evaluated = Evaluate(0, 0);

        List<IAnnotation> annotations = new();

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (evaluated.Contains(buffEntry.Name))
                annotations.Add(buffEntry);
        }

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (evaluated.Contains(keywordEntry.Name))
                annotations.Add(keywordEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? Evaluate(0, 0);
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return toRet;
    }

    public async Task Execute(StageEntity caster, StageSkill skill, bool recursive)
    {
        StageReport r = caster.Env.Report;
        if (r.UseTween)
            await r.PlayTween(new ShiftTweenDescriptor());
        r.Append($"{caster.GetName()}使用了{Name}");
        r.AppendNote(caster.Index, skill);
        await _execute(caster, skill, recursive);
        r.Append($"\n");
    }

    private async Task DefaultExecute(StageEntity caster, StageSkill skill, bool recursive) { }
}
