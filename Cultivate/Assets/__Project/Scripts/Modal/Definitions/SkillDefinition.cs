
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
        
        curr.CalcHasStageCast();
        
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
        {
            bool isStartStage = _procedureDefinitions[i].GetPostCondDefinition().Description == PostCondDefinition.StartStage.Description;
            _hasStartStageCast |= isStartStage;
        }
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

        return description;
    }

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