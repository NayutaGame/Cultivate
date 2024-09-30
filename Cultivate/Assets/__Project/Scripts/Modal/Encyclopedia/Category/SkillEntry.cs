
using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, IAnnotation, ISkillModel
{
    private string _name;
    
    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;
    
    private CLLibrary.Bound _jingJieBound;
    public bool JingJieContains(JingJie jingJie) => _jingJieBound.Contains(jingJie);
    public JingJie LowestJingJie => _jingJieBound.Start;
    public JingJie HighestJingJie => _jingJieBound.End - 1;
    
    public Color GetColor(JingJie jingJie)
        => CanvasManager.Instance.JingJieColors[jingJie];

    private SkillTypeComposite _skillTypeComposite;



    public StageClosure[] Closures;
    
    

    private Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> _cost;
    public async Task<CostResult> Cost(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
    {
        CostResult result = await _cost(env, caster, skill, recursive);
        result.Env = env;
        result.Entity = caster;
        result.Skill = skill;
        return result;
    }
    
    private Func<JingJie, int, CostResult, CostDescription> _costDescription;
    public CostDescription GetCostDescription(JingJie showingJingJie, CostResult costResult)
        => _costDescription(showingJingJie, showingJingJie - LowestJingJie, costResult);
    
    private Func<CastDetails, Task> _cast;
    public async Task Cast(CastDetails d)
        => await _cast(d);
    private async Task DefaultCast(CastDetails d) { }

    private Func<StartStageCastDetails, Task> _startStageCast;
    public async Task StartStageCast(StartStageCastDetails d)
        => await _startStageCast(d);
    private async Task DefaultStartStageCast(StartStageCastDetails d) { }

    public bool HasStartStageCast() => _startStageCast != DefaultStartStageCast;

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
    
    private string _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;

    private SpriteEntry _spriteEntry;

    public SkillEntry(string id,
        string name,
        WuXing? wuXing,
        CLLibrary.Bound jingJieBound,
        SkillTypeComposite skillTypeComposite = null,
        
        StageClosure[] closures = null,
        
        Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> cost = null,
        Func<JingJie, int, CostResult, CostDescription> costDescription = null,
        Func<CastDetails, Task> cast = null,
        Func<JingJie, int, CostResult, CastResult, string> castDescription = null,
        
        Func<StartStageCastDetails, Task> startStageCast = null,
        
        string trivia = null,
        bool withinPool = true
        ) : base(id)
    {
        _name = name;
        _wuXing = wuXing;
        _jingJieBound = jingJieBound;
        _skillTypeComposite = skillTypeComposite ?? 0;

        Closures = closures ?? Array.Empty<StageClosure>();
        
        _cost = cost ?? CostResult.Empty;
        _costDescription = costDescription ?? CostDescription.Empty;
        _cast = cast ?? DefaultCast;
        _castDescription = castDescription;

        _startStageCast = startStageCast ?? DefaultStartStageCast;
        
        _trivia = trivia;
        _withinPool = withinPool;

        _spriteEntry = GetName();
    }

    public static implicit operator SkillEntry(string id) => Encyclopedia.SkillCategory[id];

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public WuXing? GetWuXing() => WuXing;
    public Sprite GetWuXingSprite() => CanvasManager.Instance.GetWuXingSprite(WuXing);
    public string GetName() => _name;
    public SkillTypeComposite GetSkillTypeComposite() => _skillTypeComposite;
    public string GetExplanation() => _annotationArray.GetExplanation();
    public string GetTrivia() => _trivia;

    public JingJie GetJingJie() => LowestJingJie;
    public CostDescription GetCostDescription(JingJie showingJingJie) => GetCostDescription(showingJingJie, null);
    public string GetHighlight(JingJie showingJingJie) => GetHighlight(showingJingJie, null, null);
    public Sprite GetJingJieSprite(JingJie showingJingJie) => CanvasManager.Instance.JingJieSprites[showingJingJie];
    public JingJie NextJingJie(JingJie showingJingJie)
    {
        int next = showingJingJie + 1;
        if (JingJieContains(next))
            return next;

        return LowestJingJie;
    }

    public Color GetColor() => GetColor(LowestJingJie);
}
