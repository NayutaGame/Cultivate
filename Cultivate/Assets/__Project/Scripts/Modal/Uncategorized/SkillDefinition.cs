
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SkillDefinition
{
    private CostDefinition _costDefinition;
    public CostDefinition GetCostDefinition() => _costDefinition;
    
    private ProcedureDefinition[] _procedureDefinitions;
    public ProcedureDefinition[] GetProcedureDefinitions() => _procedureDefinitions;

    private bool _hasStartStageCast;
    public bool HasStartStageCast => _hasStartStageCast;

    private AnnotationArray _cascade;
    public AnnotationArray Cascade
    {
        get
        {
            if (_cascade != null)
                return _cascade;
            
            CostDescription costDescription = GetLiteralCostDescription();
            _cascade = AnnotationArray.FromDescriptionAndCostType(GetLiteralDescription(), costDescription.Type);
            
            return _cascade;
        }
    }

    private SkillDefinition(CostDefinition costDefinition, ProcedureDefinition[] procedureDefinitions)
    {
        _costDefinition = costDefinition;
        _procedureDefinitions = procedureDefinitions;

        CalcHasStageCast();
    }

    public static SkillDefinition FromDefinition(CostDefinition costDefinition, ProcedureDefinition[] procedureDefinitions)
        => new(costDefinition, procedureDefinitions);

    public static SkillDefinition FromMutate(SkillDefinition skillDefinition, List<SkillEntry> mutators)
    {
        SkillDefinition curr = skillDefinition.Clone();
        
        for (int i = 0; i < mutators.Count; i++)
        {
            MutateDefinition[] mutateDefinitions = mutators[i].GetMutateDefinitions();
            for (int j = 0; j < mutateDefinitions.Length; j++)
                curr = mutateDefinitions[j].Mutate(curr);
        }
        
        return curr;
    }

    public SkillDefinition Clone()
    {
        ProcedureDefinition[] cloned = new ProcedureDefinition[_procedureDefinitions.Length];
        for (int i = 0; i < _procedureDefinitions.Length; i++)
            cloned[i] = _procedureDefinitions[i].Clone();
        return FromDefinition(_costDefinition.Clone(), cloned);
    }

    private void CalcHasStageCast()
    {
        _hasStartStageCast = false;
        for (int i = 0; i < _procedureDefinitions.Length; i++)
            _hasStartStageCast |= _procedureDefinitions[i].GetPostCondDefinition() == PostCondDefinition.StartStage;
    }

    public CostDescription GetLiteralCostDescription()
        => _costDefinition.GetLiteralCostDescription();

    public async UniTask Cast(CastDetails d)
    {
        for (int i = 0; i < _procedureDefinitions.Length; i++)
            await _procedureDefinitions[i].TryCast(d);
    }

    public Description GetLiteralDescription()
        => GetActualDescription(null, null);
    
    public string GetLiteralDescriptionHighlighted()
        => GetActualDescription(null, null).GetHighlight(Cascade);

    public Description GetActualDescription(ResultDict costResult, ResultDict castResult)
    {
        Description description = new();
        ResultDict tempCastResult = castResult ?? new();
            
        for (int i = 0; i < _procedureDefinitions.Length; i++)
        {
            if (i != 0)
                description.AppendReturn();
            Description subDescription = new();
            _procedureDefinitions[i].GetDescription(subDescription, costResult, tempCastResult);
            description.Join(subDescription);
        }

        Description descriptionFromCost = _costDefinition.DefaultGetDescription(costResult);
        if (descriptionFromCost != null)
        {
            description.AppendReturn();
            description.Join(descriptionFromCost);
        }

        ProcessExtraReturn(description);
        ProcessSoftReturn(description);

        return description;
    }

    private static readonly Regex SplitterRegex = new Regex(@"(?:\n|\|\|)+", RegexOptions.Compiled);
    private static readonly Regex TrimReturnRegex = new Regex(@"^(?:\n|\|\|)+|(?:\n|\|\|)+$", RegexOptions.Compiled);

    private void ProcessExtraReturn(Description description)
    {
        string content = description.ToString();

        content = TrimReturnRegex.Replace(content, "");

        string[] groups = SplitterRegex.Split(content);
        MatchCollection matchCollection = SplitterRegex.Matches(content);
        
        description.Sb.Clear();

        for (int i = 0; i < groups.Length - 1; i++)
        {
            description.Sb.Append(groups[i]);
            string split = matchCollection[i].Value;
            description.Sb.Append(split.Contains("\n") ? "\n" : "||");
        }

        description.Sb.Append(groups[^1]);
    }

    private void ProcessSoftReturn(Description description, int maxConvert = 2)
    {
        const string SOFT_RETURN = "||";
        const string HARD_RETURN = "\n";
        const string SPACE = "  ";

        string content = description.ToString();
        int hardReturnCount = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] == '\n')
                hardReturnCount++;
        }

        int convert = Mathf.Max(0, maxConvert - hardReturnCount);
        int replaced = 0;
        int idx = 0;
        StringBuilder sb = new StringBuilder();
        while (idx < content.Length)
        {
            int softIdx = content.IndexOf(SOFT_RETURN, idx);
            if (softIdx == -1)
            {
                sb.Append(content.Substring(idx));
                break;
            }
            sb.Append(content.Substring(idx, softIdx - idx));
            if (replaced < convert)
            {
                sb.Append(HARD_RETURN);
                replaced++;
            }
            else
            {
                sb.Append(SPACE);
            }
            idx = softIdx + SOFT_RETURN.Length;
        }
        // 更新description内容
        description.Sb.Clear();
        description.Sb.Append(sb.ToString());
    }

    public string GetActualDescriptionHighlighted(ResultDict costResult, ResultDict castResult)
        => GetActualDescription(costResult, castResult).GetHighlight(Cascade);

    public bool CanMutate(MutateDefinition[] mutateDefinitions)
    {
        for (int i = 0; i < mutateDefinitions.Length; i++)
        {
            MutateDefinition mutateDefinition = mutateDefinitions[i];
            if (mutateDefinition.CanMutate(this))
                return true;
        }

        return false;
    }
}