
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, IAnnotation
{
    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;
    
    private CLLibrary.Range _jingJieRange;
    public bool JingJieContains(JingJie jingJie) => _jingJieRange.Contains(jingJie);
    public JingJie LowestJingJie => _jingJieRange.Start;
    public JingJie HighestJingJie => _jingJieRange.End - 1;

    #region Cost
    
    private ManaCostEvaluator _manaCostEvaluator;
    public int GetManaCost(JingJie jingJie, int dJingJie, bool jiaShi) => _manaCostEvaluator.Eval(jingJie, dJingJie, jiaShi);
    public int GetBaseManaCost(JingJie jingJie) => _manaCostEvaluator.Eval(jingJie, jingJie - LowestJingJie, false);
    public int GetBaseManaCost() => GetBaseManaCost(LowestJingJie);
    private ChannelTimeEvaluator _channelTimeEvaluator;
    public int GetChannelTime(JingJie jingJie, int dJingJie, bool jiaShi) => _channelTimeEvaluator.Eval(jingJie, dJingJie, jiaShi);

    #endregion

    public SkillTypeComposite SkillTypeComposite { get; private set; }

    private SkillDescription _description;
    public string DescriptionFromJingJie(JingJie j) => _description.FromJDJ(j, j - LowestJingJie);
    public string DescriptionFromLowestJingJie() => DescriptionFromJingJie(LowestJingJie);
    public string DescriptionFromIndicator(JingJie j, int dj, Dictionary<string, object> indicator) =>
        _description.FromIndicator(j, dj, indicator);

    private string _trivia;
    public string GetTrivia() => _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;
    
    private Func<StageEntity, StageSkill, bool, Task> _execute;

    private IAnnotation[] _annotations;
    public IAnnotation[] GetAnnotations() => _annotations;

    private SpriteEntry _spriteEntry;
    public Sprite Sprite => _spriteEntry?.Sprite;

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

    public SkillEntry(string name,
        WuXing? wuXing,
        CLLibrary.Range jingJieRange,
        ChannelTimeEvaluator channelTimeEvaluator = null,
        ManaCostEvaluator manaCostEvaluator = null,
        SkillTypeComposite skillTypeComposite = null,
        SkillDescription description = null,
        string trivia = null,
        bool withinPool = true,
        Func<StageEntity, StageSkill, bool, Task> execute = null
        ) : base(name)
    {
        _wuXing = wuXing;
        _jingJieRange = jingJieRange;
        _channelTimeEvaluator = channelTimeEvaluator ?? 0;
        _manaCostEvaluator = manaCostEvaluator ?? 0;
        SkillTypeComposite = skillTypeComposite ?? 0;
        _description = description;
        _trivia = trivia;
        _withinPool = withinPool;
        _execute = execute ?? DefaultExecute;

        _spriteEntry = name;
    }

    public static implicit operator SkillEntry(string name) => Encyclopedia.SkillCategory[name];

    public void Generate()
    {
        string evaluated = DescriptionFromLowestJingJie();

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!evaluated.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!evaluated.Contains(buffEntry.GetName()))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetAnnotatedDescription(string evaluated = null)
    {
        string toRet = evaluated ?? DescriptionFromLowestJingJie();
        foreach (var annotation in _annotations)
            toRet = toRet.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return toRet;
    }

    public async Task Channel(StageEntity caster, ChannelDetails d)
    {
        await caster.Env.TryPlayTween(new ShiftTweenDescriptor());

        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}吟唱了{GetName()} 进度: {d.GetCounter()}//{d.GetChannelTime()}");
        r.TryAppendChannelNote(caster.Index, d);
        r.TryAppend($"\n");
    }

    public async Task ChannelWithoutTween(StageEntity caster, ChannelDetails d)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}吟唱了{GetName()} 进度: {d.GetCounter()}//{d.GetChannelTime()}");
        r.TryAppend($"\n");
    }

    public async Task Execute(StageEntity caster, StageSkill skill, bool recursive)
    {
        await caster.Env.TryPlayTween(new ShiftTweenDescriptor());

        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{GetName()}");
        r.TryAppendNote(caster.Index, skill);
        await _execute(caster, skill, recursive);
        r.TryAppend($"\n");
    }

    public async Task ExecuteWithoutTween(StageEntity caster, StageSkill skill, bool recursive)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{GetName()}");
        await _execute(caster, skill, recursive);
        r.TryAppend($"\n");
    }

    private async Task DefaultExecute(StageEntity caster, StageSkill skill, bool recursive) { }
}
