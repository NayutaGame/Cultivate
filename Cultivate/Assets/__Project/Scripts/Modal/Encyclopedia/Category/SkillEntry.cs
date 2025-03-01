
using System;
using System.Linq;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, IAnnotation, ISkill
{
    private string _name;
    
    private WuXing? _wuXing;
    public WuXing? WuXing => _wuXing;
    
    private CLLibrary.Bound _jingJieBound;
    public bool JingJieContains(JingJie jingJie) => _jingJieBound.Contains(jingJie);
    public JingJie LowestJingJie => _jingJieBound.Start;
    public JingJie HighestJingJie => _jingJieBound.End - 1;

    private SkillTypeComposite _skillTypeComposite;

    
    public StageClosure[] Closures;
    

    private Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> _cost;
    public async UniTask<CostResult> Cost(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive)
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
    
    private Func<CastDetails, UniTask> _cast;
    public async UniTask Cast(CastDetails d)
        => await _cast(d);
    private async UniTask DefaultCast(CastDetails d) { }

    private Func<StartStageCastDetails, UniTask> _startStageCast;
    public async UniTask StartStageCast(StartStageCastDetails d)
        => await _startStageCast(d);
    private async UniTask DefaultStartStageCast(StartStageCastDetails d) { }

    public bool HasStartStageCast() => _startStageCast != DefaultStartStageCast;

    private Func<JingJie, int, CostResult, CastResult, string> _castDescription;
    public string GetDescription(JingJie j, CostResult costResult = null, CastResult castResult = null)
        => _castDescription(j, j - LowestJingJie, costResult, castResult);
    public string GetDescription() => GetDescription(LowestJingJie);

    private AnnotationArray _annotationArray;
    public void GenerateAnnotations()
    {
        CostDescription costDescription = _costDescription(LowestJingJie, 0, null);
        _annotationArray = AnnotationArray.FromDescriptionAndCostType(GetDescription(), costDescription.Type);
    }

    public string GetHighlight(string description)
        => _annotationArray.HighlightFromDescription(description);
    public string GetHighlight()
        => GetHighlight(GetDescription());
    public string GetHighlight(JingJie jingJie, CostResult costResult, CastResult castResult)
        => GetHighlight(GetDescription(jingJie, costResult, castResult));
    
    private string _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;

    private MergeRule _overridingMergeRule;
    public MergeRule OverridingMergeRule => _overridingMergeRule;

    private SpriteEntry _spriteEntry;

    public SkillEntry(string id,
        string name,
        CLLibrary.Bound jingJieBound,
        WuXing? wuXing = null,
        SkillTypeComposite skillTypeComposite = null,
        
        StageClosure[] closures = null,
        
        Func<StageEnvironment, StageEntity, StageSkill, bool, UniTask<CostResult>> cost = null,
        Func<JingJie, int, CostResult, CostDescription> costDescription = null,
        Func<CastDetails, UniTask> cast = null,
        Func<JingJie, int, CostResult, CastResult, string> castDescription = null,
        
        Func<StartStageCastDetails, UniTask> startStageCast = null,
        
        string trivia = null,
        bool withinPool = true,
        MergeRule overridingMergeRule = null
        ) : base(id)
    {
        _name = name;
        _jingJieBound = jingJieBound;
        _wuXing = wuXing;
        _skillTypeComposite = skillTypeComposite ?? 0;

        Closures = closures ?? Array.Empty<StageClosure>();
        
        _cost = cost ?? CostResult.Empty;
        _costDescription = costDescription ?? CostDescription.Empty;
        _cast = cast ?? DefaultCast;
        _castDescription = castDescription;

        _startStageCast = startStageCast ?? DefaultStartStageCast;
        
        _trivia = trivia;
        _withinPool = withinPool;

        _overridingMergeRule = overridingMergeRule ?? MergeRule.Trivial;

        _spriteEntry = $"Skill{GetName()}";
    }

    public static implicit operator SkillEntry(string id) => Encyclopedia.SkillCategory[id];

    public static SkillEntry FromName(string name)
        => Encyclopedia.SkillCategory.Traversal.FirstObj(e => e._name == name) ?? Encyclopedia.SkillCategory.DefaultEntry();
    
    public static SkillEntry FromNameOrId(string nameOrId)
        => Encyclopedia.SkillCategory.Traversal.FirstObj(e => e._name == nameOrId) ?? Encyclopedia.SkillCategory[nameOrId] ?? Encyclopedia.SkillCategory.DefaultEntry();

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public WuXing? GetWuXing() => WuXing;
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
    
    public bool MatchSearchText(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return true;
            
        searchText = searchText.ToLower().Trim();
        
        // 1. 直接匹配
        if (_name.ToLower().Contains(searchText) || 
            GetId().ToLower().Contains(searchText))
            return true;
            
        // 2. 五行匹配
        if (_wuXing.HasValue)
        {
            string wuXingName = _wuXing.Value.ToString().ToLower();
            if (wuXingName.Contains(searchText))
                return true;
        }
        
        // 3. 境界匹配
        string jingJieRange = $"{LowestJingJie}到{HighestJingJie}".ToLower();
        if (jingJieRange.Contains(searchText))
            return true;
            
        // 4. 技能类型匹配
        if (_skillTypeComposite.ToString().ToLower().Contains(searchText))
            return true;
            
        // 5. 描述文本匹配
        string description = GetDescription().ToLower();
        if (description.Contains(searchText))
            return true;
            
        // 6. 背景故事匹配
        if (!string.IsNullOrEmpty(_trivia) && 
            _trivia.ToLower().Contains(searchText))
            return true;
            
        // 7. 多关键词匹配（用空格分割）
        string[] keywords = searchText.Split(new[] { ' ' }, 
            StringSplitOptions.RemoveEmptyEntries);
        if (keywords.Length > 1)
        {
            return keywords.All(MatchSearchText);
        }
        
        return false;
    }
}
