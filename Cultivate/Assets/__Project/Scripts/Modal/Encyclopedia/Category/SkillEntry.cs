
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
    
    private Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> _costProcedure;
    private Func<JingJie, int, CostResult, CostDescription> _costDescription;
    public CostDescription GetCostDescription() => GetCostDescription(LowestJingJie);
    public CostDescription GetCostDescription(JingJie j, CostResult costResult = null)
        => _costDescription(j, j - LowestJingJie, costResult);

    #endregion

    public SkillTypeComposite SkillTypeComposite { get; private set; }

    #region Description
    
    private Func<JingJie, int, ExecuteResult, string> _description;
    
    public string GetDescription() => GetDescription(LowestJingJie);
    public string GetDescription(JingJie j, ExecuteResult executeResult = null)
        => _description(j, j - LowestJingJie, executeResult);

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

    public string GetHighlight(JingJie jingJie, ExecuteResult executeResult)
        => GetHighlight(GetDescription(jingJie, executeResult));
    
    public string GetExplanation()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _annotations)
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>\n{annotation.GetHighlight()}\n\n");

        return sb.ToString();
    }
    
    #endregion
    
    private string _trivia;
    public string GetTrivia() => _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;
    
    private Func<StageEntity, StageSkill, bool, Task<ExecuteResult>> _executeProcedure;

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
        Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> costProcedure = null,
        Func<JingJie, int, CostResult, CostDescription> costDescription = null,
        SkillTypeComposite skillTypeComposite = null,
        Func<JingJie, int, ExecuteResult, string> description = null,
        string trivia = null,
        bool withinPool = true,
        Func<StageEntity, StageSkill, bool, Task<ExecuteResult>> executeProcedure = null
        ) : base(name)
    {
        _wuXing = wuXing;
        _jingJieRange = jingJieRange;
        _costProcedure = costProcedure ?? CostResult.Empty;
        _costDescription = costDescription;
        SkillTypeComposite = skillTypeComposite ?? 0;
        _description = description;
        _trivia = trivia;
        _withinPool = withinPool;
        _executeProcedure = executeProcedure ?? DefaultExecuteProcedure;

        _spriteEntry = name;
    }

    public static implicit operator SkillEntry(string name) => Encyclopedia.SkillCategory[name];

    public async Task<CostResult> CostProcedure(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
    {
        CostResult result = await _costProcedure(env, caster, skill, recursive);
        result.Env = env;
        result.Entity = caster;
        result.Skill = skill;
        return result;
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
        
        ExecuteResult result = await _executeProcedure(caster, skill, recursive);
        r.TryAppendNote(caster.Index, skill, result);
        
        // write result to slot, here
        r.TryAppend($"\n");
    }

    public async Task ExecuteWithoutTween(StageEntity caster, StageSkill skill, bool recursive)
    {
        StageResult r = caster.Env.Result;
        r.TryAppend($"{caster.GetName()}使用了{GetName()}");
        
        await _executeProcedure(caster, skill, recursive);
        
        // write result
        r.TryAppend($"\n");
    }

    private async Task<ExecuteResult> DefaultExecuteProcedure(StageEntity caster, StageSkill skill, bool recursive) => null;
}
