
using System;
using System.Collections.Generic;
using CLLibrary;

public class MutateDefinition
{
    private Func<SkillDefinition, bool> _canMutate;
    private Func<SkillDefinition, SkillDefinition> _mutate;

    public MutateDefinition(
        Func<SkillDefinition, bool> canMutate,
        Func<SkillDefinition, SkillDefinition> mutate)
    {
        _canMutate = canMutate;
        _mutate = mutate;
    }

    public bool CanMutate(SkillDefinition skillDefinition)
        => _canMutate(skillDefinition);

    public SkillDefinition Mutate(SkillDefinition skillDefinition)
        => _mutate(skillDefinition);

    public static MutateDefinition CritMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.Crit)),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.Crit);
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (cond)
                    cloned.AddClosure(SkillCategory.Crit);
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
}