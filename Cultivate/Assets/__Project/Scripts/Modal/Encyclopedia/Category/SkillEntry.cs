
using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, IAnnotation
{
    // name
    private string _name;
    public string GetName() => _name;
    
    // wuXing
    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;
    
    // jingJieRange
    private CLLibrary.Range _jingJieRange;
    public bool JingJieContains(JingJie jingJie) => _jingJieRange.Contains(jingJie);
    public JingJie LowestJingJie => _jingJieRange.Start;
    public JingJie HighestJingJie => _jingJieRange.End - 1;

    // skillTypeComposite
    private SkillTypeComposite _skillTypeComposite;
    public SkillTypeComposite SkillTypeComposite => _skillTypeComposite;

    // cost
    private Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> _cost;
    public async Task<CostResult> Cost(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
    {
        CostResult result = await _cost(env, caster, skill, recursive);
        result.Env = env;
        result.Entity = caster;
        result.Skill = skill;
        return result;
    }
    
    // costDescription
    private Func<JingJie, int, CostResult, CostDescription> _costDescription;
    public CostDescription GetCostDescription() => GetCostDescription(LowestJingJie);
    public CostDescription GetCostDescription(JingJie j, CostResult costResult = null)
        => _costDescription(j, j - LowestJingJie, costResult);
    
    // cast
    private Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CastResult>> _cast;
    public async Task<CastResult> Cast(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
        => await _cast(env, caster, skill, recursive);
    private async Task<CastResult> DefaultCast(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
        => new();

    // castDescription
    private Func<JingJie, int, CostResult, CastResult, string> _castDescription;
    public string GetDescription(JingJie j, CostResult costResult = null, CastResult castResult = null)
        => _castDescription(j, j - LowestJingJie, costResult, castResult);
    public string GetDescription() => GetDescription(LowestJingJie);

    private AnnotationArray _annotationArray;
    public void GenerateAnnotations()
        => _annotationArray = AnnotationArray.FromDescription(GetDescription());
    public string GetHighlight(string description)
        => _annotationArray.HighlightFromDescription(description);
    public string GetHighlight()
        => GetHighlight(GetDescription());
    public string GetHighlight(JingJie jingJie, CostResult costResult, CastResult castResult)
        => GetHighlight(GetDescription(jingJie, costResult, castResult));
    
    public string GetExplanation()
        => _annotationArray.GetExplanation();
    
    private string _trivia;
    public string GetTrivia() => _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;

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

    public SkillEntry(string id,
        string name,
        WuXing? wuXing,
        CLLibrary.Range jingJieRange,
        SkillTypeComposite skillTypeComposite = null,
        
        Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> cost = null,
        Func<JingJie, int, CostResult, CostDescription> costDescription = null,
        Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CastResult>> cast = null,
        Func<JingJie, int, CostResult, CastResult, string> castDescription = null,
        
        string trivia = null,
        bool withinPool = true
        ) : base(id)
    {
        _name = name;
        _wuXing = wuXing;
        _jingJieRange = jingJieRange;
        _skillTypeComposite = skillTypeComposite ?? 0;
        
        _cost = cost ?? CostResult.Empty;
        _costDescription = costDescription ?? CostDescription.Empty;
        _cast = cast ?? DefaultCast;
        _castDescription = castDescription;
        
        _trivia = trivia;
        _withinPool = withinPool;

        _spriteEntry = GetName();
    }

    public static implicit operator SkillEntry(string id) => Encyclopedia.SkillCategory[id];
}
