
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillEntry : Entry, AnnotatableSkill
{
    [NonSerialized] private WuXing _wuXing;
    [NonSerialized] private Bound _jingJieBound;
    [NonSerialized] private TagComposite _tagComposite;
    [NonSerialized] private StageClosure[] _closures;
    [NonSerialized] private string _trivia;
    [NonSerialized] private MergeRule _overridingMergeRule;
    [NonSerialized] private bool _hasStartStageCast;
    [NonSerialized] private MutateDefinition[] _mutate;
    [NonSerialized] private SpriteEntry _spriteEntry;
    [NonSerialized] private SkillDefinition[] _skillDefinitions;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public SkillEntry(string id,
        string name,
        CLLibrary.Bound jingJieBound,
        WuXing wuXing,
        TagComposite tagComposite = null,
        
        StageClosure[] closures = null,
        
        Func<CastDetails, UniTask> castGenerator = null,
        Func<JingJie, int, ResultDict, ResultDict, Description> descriptionGenerator = null,
        
        string trivia = null,
        MergeRule overridingMergeRule = null,
        
        Func<int, int, CostDefinition> cost = null,
        Func<int, int, ProcedureDefinition[]> cast = null,
        Func<int, int, MutateDefinition[]> mutate = null
        ) : base(id, name)
    {
        _jingJieBound = jingJieBound;
        _wuXing = wuXing;
        
        _tagComposite = (tagComposite ?? 0) | wuXing.GetTag();

        _closures = closures ?? Array.Empty<StageClosure>();
        
        _trivia = trivia;

        _overridingMergeRule = overridingMergeRule ?? MergeRule.Trivial;

        _mutate = mutate?.Invoke(LowestJingJie, 0);

        BuildSkillDefinitions(cost, cast);

        for (int i = 0; i < _skillDefinitions.Length; i++)
            _hasStartStageCast |= _skillDefinitions[i].HasStartStageCast;
    }

    public override void Init()
    {
        CreateSprite();
    }

    public void CreateSprite()
    {
        string name = $"Skill{GetName()}";
        if (Encyclopedia.SpriteCategory.ContainsName(name))
        {
            _spriteEntry = Encyclopedia.SpriteCategory.FromName(name);
        }
        else
        {
            string id = $"SpriteAuto{Encyclopedia.SpriteCategory.Count():0000}";
            _spriteEntry = new(id, name, $"Images/CardIllustrations/{GetName()}");
            Encyclopedia.SpriteCategory.Add(_spriteEntry);
        }
    }
    
    public WuXing WuXing => _wuXing;
    public bool JingJieContains(JingJie jingJie) => _jingJieBound.Contains(jingJie);
    public JingJie LowestJingJie => _jingJieBound.Start;
    public JingJie HighestJingJie => _jingJieBound.End - 1;
    public StageClosure[] Closures => _closures;
    public MergeRule OverridingMergeRule => _overridingMergeRule;
    public bool HasStartStageCast() => _hasStartStageCast;
    public bool IsMutator => _mutate != null;
    public MutateDefinition[] GetMutateDefinitions() => _mutate;
    
    public SkillDefinition GetSkillDefinitionFromDj(int dj)
        => _skillDefinitions[dj.Clamp(0, HighestJingJie - LowestJingJie)];
    
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => GetSkillDefinitionFromDj(showingJingJie - LowestJingJie).GetLiteralCostDescription();
    
    public Description GetDescription(JingJie showingJingJie)
        => GetSkillDefinitionFromDj(showingJingJie - LowestJingJie).GetLiteralDescription();
    public Description GetDescription()
        => GetSkillDefinitionFromDj(0).GetLiteralDescription();

    private CostDefinition DefaultCost(int j, int dj)
        => new EmptyCostDefinition();

    public ProcedureDefinition[] DefaultCast(int j, int dj)
        => Array.Empty<ProcedureDefinition>();

    private void BuildSkillDefinitions(Func<int, int, CostDefinition> cost, Func<int, int, ProcedureDefinition[]> cast)
    {
        _skillDefinitions = new SkillDefinition[_jingJieBound.Length];
        Func<int, int, CostDefinition> costGenerator = cost ?? DefaultCost;
        Func<int, int, ProcedureDefinition[]> castGenerator = cast ?? DefaultCast;
        for (int i = 0; i < _jingJieBound.Length; i++)
        {
            CostDefinition costDefinition = costGenerator(LowestJingJie + i, i);
            
            List<ProcedureDefinition> procedureDefinitionsList = castGenerator(LowestJingJie + i, i).ToList();
            ProcedureDefinition[] procedureDefinitions = procedureDefinitionsList.FilterObj(pd => pd.GetPreCondDefinition().Cond(LowestJingJie + i, i)).ToArray();

            _skillDefinitions[i] = SkillDefinition.FromDefinition(costDefinition, procedureDefinitions);
        }
    }

    public Sprite GetSprite() => _spriteEntry?.Sprite ? _spriteEntry?.Sprite : Encyclopedia.SpriteCategory.MissingSkillIllustration().Sprite;
    public WuXing GetWuXing() => WuXing;
    public TagComposite GetTagComposite() => _tagComposite;
    public string GetTrivia() => _trivia;

    public JingJie GetJingJie() => LowestJingJie;
    public JingJie GetLowestJingJie() => LowestJingJie;
    public JingJie GetHighestJingJie() => HighestJingJie;
    public Sprite GetJingJieSprite(JingJie showingJingJie) => CanvasManager.Instance.JingJieSprites[showingJingJie];
    public JingJie NextJingJie(JingJie showingJingJie)
    {
        int next = showingJingJie + 1;
        if (JingJieContains(next))
            return next;

        return LowestJingJie;
    }

    public bool CanShowAnnotation()
        => true;
    
    public bool MatchSearchText(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return true;
            
        searchText = searchText.ToLower().Trim();
        
        // 1. 直接匹配
        if (GetName().ToLower().Contains(searchText) || 
            GetId().ToLower().Contains(searchText))
            return true;
            
        // 2. 五行匹配
        if (_wuXing != null)
        {
            string wuXingName = _wuXing.ToString().ToLower();
            if (wuXingName.Contains(searchText))
                return true;
        }
        
        // 3. 境界匹配
        string jingJieRange = $"{LowestJingJie}到{HighestJingJie}".ToLower();
        if (jingJieRange.Contains(searchText))
            return true;
            
        // 4. 技能类型匹配
        // if (_tagComposite.ToString().ToLower().Contains(searchText))
        //     return true;
            
        // 5. 描述文本匹配
        string description = GetDescription().GetRawString().ToLower();
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
