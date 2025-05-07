
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, Annotatable, ISkill
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
    

    private Func<CastDetails, UniTask> _castGenerator;
    private async UniTask DefaultCast(CastDetails d) { }

    private Func<JingJie, int, ResultDict, ResultDict, Description> _descriptionGenerator;
    public Description DescriptionFromEnv(JingJie j, ResultDict costResult = null, ResultDict castResult = null)
    {
        if (_hasCast)
        {
            ProcedureDefinition[] procedureDefinitions = _procedureDefinitionsCache[j - LowestJingJie];
            Description description = new();
            ResultDict tempCastResult = castResult ?? new();
            
            for (int i = 0; i < procedureDefinitions.Length; i++)
            {
                Description subDescription = procedureDefinitions[i].GetDescription(costResult, tempCastResult);
                if (subDescription == null)
                    continue;
                description.Join(subDescription);
            }

            CostDefinition costDefinition = _costDefinitionsCache[j - LowestJingJie];
            Description descriptionFromCost = costDefinition.DefaultGetDescription(costResult);
            if (descriptionFromCost != null)
                description.Join(descriptionFromCost);

            return description;
        }
        
        return _descriptionGenerator(j, j - LowestJingJie, costResult, castResult ?? ResultDict.Default);
    }

    public Description GetDescription()
        => DescriptionFromEnv(LowestJingJie);

    private AnnotationArray _cascade;
    public void GenerateCascade()
    {
        CostDescription costDescription = GetLiteralCostDescription(LowestJingJie);
        _cascade = AnnotationArray.FromDescriptionAndCostType(GetDescription(), costDescription.Type);
    }

    public string GetHighlight(Description description)
        => description.GetHighlight(_cascade);
    public string GetHighlight()
        => GetDescription().GetHighlight(_cascade);
    public string GetHighlight(JingJie jingJie, ResultDict costResult, ResultDict castResult)
        => DescriptionFromEnv(jingJie, costResult, castResult).GetHighlight(_cascade);
    
    private string _trivia;

    private bool _withinPool;
    public bool WithinPool => _withinPool;

    private MergeRule _overridingMergeRule;
    public MergeRule OverridingMergeRule => _overridingMergeRule;

    private bool _hasCost;
    private Func<int, int, CostDefinition> _costDefinitionsFromJingJie;
    private CostDefinition[] _costDefinitionsCache;
    
    public CostDefinition Cost(CostDetails d)
        => _costDefinitionsCache[d.Dj];

    private bool _hasCast;
    private Func<int, int, ProcedureDefinition[]> _procedureDefinitionsFromJingJie;
    private ProcedureDefinition[][] _procedureDefinitionsCache;

    public ProcedureDefinition[] GetProcedureDefinitionsFromJingJie(int jingJie)
        => _procedureDefinitionsCache[jingJie - LowestJingJie];

    private bool _hasStartStageCast;
    public bool HasStartStageCast() => _hasStartStageCast;
    
    public async UniTask Cast(CastDetails d)
    {
        if (_hasCast)
        {
            ProcedureDefinition[] procedureDefinitions = _procedureDefinitionsCache[d.Dj];
            for (int i = 0; i < procedureDefinitions.Length; i++)
            {
                await procedureDefinitions[i].TryCast(d);
            }
            return;
        }
        
        await _castGenerator(d);
    }

    private MutateDefinition[] _mutate;

    public MutateDefinition[] GetMutateDefinitions()
        => _mutate;

    private SpriteEntry _spriteEntry;

    public SkillEntry(string id,
        string name,
        CLLibrary.Bound jingJieBound,
        WuXing? wuXing = null,
        SkillTypeComposite skillTypeComposite = null,
        
        StageClosure[] closures = null,
        
        Func<CastDetails, UniTask> castGenerator = null,
        Func<JingJie, int, ResultDict, ResultDict, Description> descriptionGenerator = null,
        
        string trivia = null,
        bool withinPool = true,
        MergeRule overridingMergeRule = null,
        
        Func<int, int, CostDefinition> cost = null,
        Func<int, int, ProcedureDefinition[]> cast = null,
        
        Func<int, int, MutateDefinition[]> mutate = null
        ) : base(id)
    {
        _name = name;
        _jingJieBound = jingJieBound;
        _wuXing = wuXing;
        _skillTypeComposite = skillTypeComposite ?? 0;

        Closures = closures ?? Array.Empty<StageClosure>();
        
        _castGenerator = castGenerator ?? DefaultCast;
        _descriptionGenerator = descriptionGenerator;
        
        _trivia = trivia;
        _withinPool = withinPool;

        _overridingMergeRule = overridingMergeRule ?? MergeRule.Trivial;

        BuildCostDefinitionCache(cost);
        BuildProcedureDefinitionCache(cast);

        _mutate = mutate(LowestJingJie, 0);
    }

    public bool IsMutator
        => _mutate != null;

    private CostDefinition DefaultCost(int j, int dj)
        => new EmptyCostDefinition();

    private void BuildCostDefinitionCache(Func<int, int, CostDefinition> cost)
    {
        _costDefinitionsFromJingJie = cost ?? DefaultCost;
        _costDefinitionsCache = new CostDefinition[_jingJieBound.Length];
        for (int i = 0; i < _jingJieBound.Length; i++)
        {
            _costDefinitionsCache[i] = _costDefinitionsFromJingJie(LowestJingJie + i, i);
        }
    }

    private void BuildProcedureDefinitionCache(Func<int, int, ProcedureDefinition[]> cast)
    {
        _hasCast = cast != null;
        if (cast == null) return;
        
        _procedureDefinitionsFromJingJie = cast;
        _procedureDefinitionsCache = new ProcedureDefinition[_jingJieBound.Length][];
        for (int i = 0; i < _jingJieBound.Length; i++)
        {
            ProcedureDefinition[] procedureDefinitions = _procedureDefinitionsFromJingJie(LowestJingJie + i, i);
            List<ProcedureDefinition> procedureDefinitionsList = procedureDefinitions.ToList();
            _procedureDefinitionsCache[i] = procedureDefinitionsList.FilterObj(pd => pd.GetPreCondDefinition().Cond(LowestJingJie + i, i)).ToArray();

            for (int j = 0; j < _procedureDefinitionsCache[i].Length; j++)
                _hasStartStageCast |= _procedureDefinitionsCache[i][j].GetPostCondDefinition() == PostCondDefinition.StartStage;
        }
    }

    public static implicit operator SkillEntry(string id) => Encyclopedia.SkillCategory[id];

    public static SkillEntry FromName(string name)
        => Encyclopedia.SkillCategory.Traversal.FirstObj(e => e._name == name) ?? Encyclopedia.SkillCategory.DefaultEntry();
    
    public static SkillEntry FromNameOrId(string nameOrId)
        => Encyclopedia.SkillCategory.Traversal.FirstObj(e => e._name == nameOrId) ?? Encyclopedia.SkillCategory[nameOrId] ?? Encyclopedia.SkillCategory.DefaultEntry();

    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public void CreateSprite()
    {
        string key = $"Skill{GetName()}";
        if (Encyclopedia.SpriteCategory.ContainsKey(key))
        {
            _spriteEntry = key;
        }
        else
        {
            _spriteEntry = new(key, $"Images/CardIllustrations/{GetName()}");
            Encyclopedia.SpriteCategory.Add(_spriteEntry);
        }
    }

    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public WuXing? GetWuXing() => WuXing;
    public string GetName() => _name;
    public SkillTypeComposite GetSkillTypeComposite() => _skillTypeComposite;
    public string GetCascadeAnnotated() => _cascade.GetCascadeAnnotated();
    public string GetTrivia() => _trivia;

    public JingJie GetJingJie() => LowestJingJie;
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => _costDefinitionsCache[showingJingJie - LowestJingJie].GetLiteralCostDescription();
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
        string description = GetDescription().ToString().ToLower();
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
