
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
    
    private Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> _cost;
    private Func<JingJie, int, CostResult, CostDescription> _costDescription;
    public CostDescription GetCostDescription() => GetCostDescription(LowestJingJie);
    public CostDescription GetCostDescription(JingJie j, CostResult costResult = null)
        => _costDescription(j, j - LowestJingJie, costResult);

    #endregion

    public SkillTypeComposite SkillTypeComposite { get; private set; }

    #region Description
    
    private Func<JingJie, int, CastResult, string> _description;
    
    public string GetDescription() => GetDescription(LowestJingJie);
    public string GetDescription(JingJie j, CastResult castResult = null)
        => _description(j, j - LowestJingJie, castResult);

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

    public string GetHighlight(JingJie jingJie, CastResult castResult)
        => GetHighlight(GetDescription(jingJie, castResult));
    
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
    
    private Func<StageEntity, StageSkill, bool, Task<CastResult>> _cast;

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
        Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> cost = null,
        Func<JingJie, int, CostResult, CostDescription> costDescription = null,
        SkillTypeComposite skillTypeComposite = null,
        Func<JingJie, int, CastResult, string> description = null,
        string trivia = null,
        bool withinPool = true,
        Func<StageEntity, StageSkill, bool, Task<CastResult>> cast = null
        ) : base(name)
    {
        _wuXing = wuXing;
        _jingJieRange = jingJieRange;
        _cost = cost ?? CostResult.Empty;
        _costDescription = costDescription ?? DefaultCostDescription;
        SkillTypeComposite = skillTypeComposite ?? 0;
        _description = description;
        _trivia = trivia;
        _withinPool = withinPool;
        _cast = cast ?? DefaultCast;

        _spriteEntry = name;
    }

    public static implicit operator SkillEntry(string name) => Encyclopedia.SkillCategory[name];

    public async Task<CostResult> Cost(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
    {
        CostResult result = await _cost(env, caster, skill, recursive);
        result.Env = env;
        result.Entity = caster;
        result.Skill = skill;
        return result;
    }

    public async Task<CastResult> Cast(StageEntity caster, StageSkill skill, bool recursive)
        => await _cast(caster, skill, recursive);

    private async Task<CastResult> DefaultCast(StageEntity caster, StageSkill skill, bool recursive) => new();
    
    private CostDescription DefaultCostDescription(JingJie j, int dj, CostResult costResult) => CostDescription.Default();
}
