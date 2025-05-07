
using System;

public class MutateDefinition
{
    private Func<ProcedureDefinition, bool> _canMutate;
    private Func<ProcedureDefinition, ProcedureDefinition> _mutate;

    public MutateDefinition(
        Func<ProcedureDefinition, bool> canMutate,
        Func<ProcedureDefinition, ProcedureDefinition> mutate)
    {
        _canMutate = canMutate;
        _mutate = mutate;
    }

    public bool CanMutate(ProcedureDefinition pd)
        => _canMutate(pd);

    public ProcedureDefinition Mutate(ProcedureDefinition pd)
        => _mutate(pd);

    public static MutateDefinition CritMutate = new MutateDefinition(
        pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.Crit),
        pd => pd.AddClosure(SkillCategory.Crit));
}