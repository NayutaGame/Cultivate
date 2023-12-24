
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using Unity.VisualScripting;
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

    private string _trivia;

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

    private ManaCostEvaluator _manaCostEvaluator;
    public int GetManaCost(JingJie jingJie, int dJingJie, bool jiaShi) => _manaCostEvaluator.Eval(jingJie, dJingJie, jiaShi);

    public int GetBaseManaCost() => _manaCostEvaluator.Eval(_jingJieRange.Start, 0, false);

    private ChannelTimeEvaluator _channelTimeEvaluator;
    public int GetChannelTime(JingJie jingJie, int dJingJie, bool jiaShi) => _channelTimeEvaluator.Eval(jingJie, dJingJie, jiaShi);

    public SkillTypeComposite SkillTypeComposite { get; private set; }
    private Func<StageEntity, StageSkill, bool, Task> _execute;

    private bool _withinPool;
    public bool WithinPool => _withinPool;

    private SpriteEntry _spriteEntry;
    public Sprite Sprite => _spriteEntry?.Sprite;

    public SkillEntry(string name,
        CLLibrary.Range jingJieRange,
        SkillDescription description,
        string trivia = null,
        WuXing? wuXing = null,
        ManaCostEvaluator manaCostEvaluator = null,
        ChannelTimeEvaluator channelTimeEvaluator = null,
        SkillTypeComposite skillTypeComposite = null,
        bool withinPool = true,
        Func<StageEntity, StageSkill, bool, Task> execute = null
        ) : base(name)
    {
        _jingJieRange = jingJieRange;
        _description = description;
        _trivia = trivia;
        _wuXing = wuXing;
        _manaCostEvaluator = manaCostEvaluator ?? 0;
        _channelTimeEvaluator = channelTimeEvaluator ?? 0;
        SkillTypeComposite = skillTypeComposite ?? 0;
        _withinPool = withinPool;
        _execute = execute ?? DefaultExecute;

        _spriteEntry = name;
    }

    public static implicit operator SkillEntry(string name) => Encyclopedia.SkillCategory[name];

    public string Evaluate(int j, int dj) => _description.Eval(j, dj);

    public void Generate()
    {
        string evaluated = Evaluate(0, 0);

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!evaluated.Contains(keywordEntry.Name))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!evaluated.Contains(buffEntry.Name))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.Name);
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
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

    public string GetTrivia()
        => _trivia;

    public async Task Channel(StageEntity caster, ChannelDetails d)
    {
        await caster.Env.TryPlayTween(new ShiftTweenDescriptor());

        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}吟唱了{Name} 进度: {d.GetCounter()}//{d.GetChannelTime()}");
        r.TryAppendChannelNote(caster.Index, d);
        r.TryAppend($"\n");
    }

    public async Task ChannelWithoutTween(StageEntity caster, ChannelDetails d)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}吟唱了{Name} 进度: {d.GetCounter()}//{d.GetChannelTime()}");
        r.TryAppend($"\n");
    }

    public async Task Execute(StageEntity caster, StageSkill skill, bool recursive)
    {
        await caster.Env.TryPlayTween(new ShiftTweenDescriptor());

        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{Name}");
        r.TryAppendNote(caster.Index, skill);
        await _execute(caster, skill, recursive);
        r.TryAppend($"\n");
    }

    public async Task ExecuteWithoutTween(StageEntity caster, StageSkill skill, bool recursive)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{Name}");
        await _execute(caster, skill, recursive);
        r.TryAppend($"\n");
    }

    private async Task DefaultExecute(StageEntity caster, StageSkill skill, bool recursive) { }
}
