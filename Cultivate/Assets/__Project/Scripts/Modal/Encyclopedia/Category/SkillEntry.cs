
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

    public Description GetLiteralDescription() => GetSkillDefinitionFromDj(0).GetLiteralDescription();

    public string GetHighlight()
        => GetSkillDefinitionFromDj(0).GetLiteralDescriptionHighlighted();
    public string GetHighlight(JingJie jingJie, ResultDict costResult, ResultDict castResult)
        => GetSkillDefinitionFromDj(jingJie - LowestJingJie).GetActualDescriptionHighlighted(costResult, castResult);
    
    private string _trivia;

    private MergeRule _overridingMergeRule;
    public MergeRule OverridingMergeRule => _overridingMergeRule;

    private bool _hasStartStageCast;
    public bool HasStartStageCast() => _hasStartStageCast;
    public bool IsMutator => _mutate != null;
    private MutateDefinition[] _mutate;
    public MutateDefinition[] GetMutateDefinitions() => _mutate;

    private SpriteEntry _spriteEntry;
    private SkillDefinition[] _skillDefinitions;
    public SkillDefinition GetSkillDefinitionFromDj(int dj)
        => _skillDefinitions[dj.Clamp(0, HighestJingJie - LowestJingJie)];

    public SkillEntry(string id,
        string name,
        CLLibrary.Bound jingJieBound,
        WuXing? wuXing = null,
        SkillTypeComposite skillTypeComposite = null,
        
        StageClosure[] closures = null,
        
        Func<CastDetails, UniTask> castGenerator = null,
        Func<JingJie, int, ResultDict, ResultDict, Description> descriptionGenerator = null,
        
        string trivia = null,
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
        
        _trivia = trivia;

        _overridingMergeRule = overridingMergeRule ?? MergeRule.Trivial;

        _mutate = mutate?.Invoke(LowestJingJie, 0);

        BuildSkillDefinitions(cost, cast);

        for (int i = 0; i < _skillDefinitions.Length; i++)
            _hasStartStageCast |= _skillDefinitions[i].HasStartStageCast;
    }

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
    public string GetCascadeAnnotated() => GetSkillDefinitionFromDj(0).Cascade.GetCascadeAnnotated();
    public string GetTrivia() => _trivia;

    public JingJie GetJingJie() => LowestJingJie;
    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => GetSkillDefinitionFromDj(showingJingJie - LowestJingJie).GetLiteralCostDescription();
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
        string description = GetLiteralDescription().ToString().ToLower();
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
