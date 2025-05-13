
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

    public static MutateDefinition LifeStealMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.LifeSteal)),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.LifeSteal);
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (cond)
                    cloned.AddClosure(SkillCategory.LifeSteal);
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition PenetrateMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.Penetrate)),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd => pd is AttackProcedureDefinition && !pd.ContainsClosure(SkillCategory.Penetrate);
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (cond)
                    cloned.AddClosure(SkillCategory.Penetrate);
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition SwiftMutate = new(
        skillDefinition => null == skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd is SetActionPointProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                newProcedureDefinitions.Add(cloned);
            }
            
            newProcedureDefinitions.Add(new SetActionPointProcedureDefinition(2));

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition RemoveCondMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd.GetPostCondDefinition() != PostCondDefinition.Default),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();

                cloned.SetPostCondDefinition(PostCondDefinition.Default);
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition RemoveCostMutate = new(
        skillDefinition => !(skillDefinition.GetCostDefinition() is EmptyCostDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(new EmptyCostDefinition(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition ExhaustMutate = new(
        skillDefinition => null == skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd is ExhaustProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                newProcedureDefinitions.Add(cloned);
            }
            
            newProcedureDefinitions.Add(new ExhaustProcedureDefinition());

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static MutateDefinition RemoveDebuffMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd =>
        {
            GainBuffProcedureDefinition gainBuffProcedureDefinition = pd as GainBuffProcedureDefinition;
            if (gainBuffProcedureDefinition == null)
                return false;
            return !gainBuffProcedureDefinition.BuffEntry.Friendly;
        }),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd =>
            {
                GainBuffProcedureDefinition gainBuffProcedureDefinition = pd as GainBuffProcedureDefinition;
                if (gainBuffProcedureDefinition == null)
                    return false;
                return !gainBuffProcedureDefinition.BuffEntry.Friendly;
            };
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (!cond)
                    newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    // has bug
    // public static MutateDefinition StartStageMutate = new(
    //     skillDefinition => true,
    //     skillDefinition =>
    //     {
    //         ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
    //         List<ProcedureDefinition> newProcedureDefinitions = new();
    //         
    //         for (int i = 0; i < oldProcedureDefinitions.Length; i++)
    //         {
    //             ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
    //             newProcedureDefinitions.Add(cloned);
    //         }
    //
    //         ProcedureDefinition startStage = new DirectProcedureDefinition(async d =>
    //             {
    //                 if (!d.Recursive)
    //                     return;
    //                 await d.Caster.CastProcedure(d.Skill, false);
    //             }).SetPostCondDefinition(PostCondDefinition.StartStage)
    //             .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"开局：使用一次"));
    //         newProcedureDefinitions.Add(startStage);
    //
    //         return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
    //     });
}