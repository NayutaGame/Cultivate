
using System;
using System.Collections.Generic;
using System.Text;
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

    public string GetDescription() => GetDescription(LowestJingJie);
    public string GetDescription(JingJie j, Dictionary<string, string> indicator = null) =>
        _description.Get(j, j - LowestJingJie, indicator);

    public void GenerateAnnotations()
    {
        string description = GetDescription();

        List<IAnnotation> annotations = new();

        foreach (KeywordEntry keywordEntry in Encyclopedia.KeywordCategory.Traversal)
        {
            if (!description.Contains(keywordEntry.GetName()))
                continue;

            annotations.Add(keywordEntry);
        }

        foreach (BuffEntry buffEntry in Encyclopedia.BuffCategory.Traversal)
        {
            if (!description.Contains(buffEntry.GetName()))
                continue;

            IAnnotation duplicate = annotations.FirstObj(annotation => annotation.GetName() == buffEntry.GetName());
            if (duplicate != null)
                continue;

            annotations.Add(buffEntry);
        }

        _annotations = annotations.ToArray();
    }

    public string GetHighlight() => GetHighlight(GetDescription());
    
    public string GetHighlight(string description)
    {
        StringBuilder sb = new(description);
        foreach (IAnnotation annotation in _annotations)
            sb = sb.Replace(annotation.GetName(), $"<style=\"Highlight\">{annotation.GetName()}</style>");

        return sb.ToString();
    }

    public string GetHighlight(JingJie jingJie, Dictionary<string, string> indicator)
        => GetHighlight(GetDescription(jingJie, indicator));
    
    public string GetExplanation()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _annotations)
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetHighlight()}\n\n");

        return sb.ToString();
    }

    private string _trivia;
    public string GetTrivia() => _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;
    
    private Func<StageEntity, StageSkill, bool, Task<Dictionary<string, string>>> _execute;

    private IAnnotation[] _annotations;

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
        Func<StageEntity, StageSkill, bool, Task<Dictionary<string, string>>> execute = null
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
    
    // Cost Procedure

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
        Dictionary<string, string> indicator = await _execute(caster, skill, recursive);
        if (indicator != null)
        {
            
        }
        r.TryAppendNote(caster.Index, skill, indicator);
        // write indicator to slot, here
        r.TryAppend($"\n");
    }

    public async Task ExecuteWithoutTween(StageEntity caster, StageSkill skill, bool recursive)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{GetName()}");
        await _execute(caster, skill, recursive);
        // indicator
        r.TryAppend($"\n");
    }

    private async Task<Dictionary<string, string>>
        DefaultExecute(StageEntity caster, StageSkill skill, bool recursive) => null;
}
