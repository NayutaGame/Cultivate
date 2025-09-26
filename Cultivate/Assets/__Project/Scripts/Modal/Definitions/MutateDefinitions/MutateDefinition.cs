
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class MutateDefinition
{
    private Func<SkillDefinition, bool> _canMutate;
    private Func<SkillDefinition, SkillDefinition> _mutate;

    private MutateDefinition(
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

    public static readonly MutateDefinition ProtectMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => pd is GainArmorProcedureDefinition || pd is HealProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned is GainArmorProcedureDefinition g)
                {
                    g.Value += 8;
                }
                
                if (cloned is HealProcedureDefinition h)
                {
                    h.Value += 8;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition DestroyMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => pd is AttackProcedureDefinition || pd is LoseArmorProcedureDefinition || pd is RemoveArmorProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned is AttackProcedureDefinition a)
                {
                    a.Value += 5;
                }
                
                if (cloned is RemoveArmorProcedureDefinition r)
                {
                    r.Value += 5;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static readonly MutateDefinition RemoveDebuffMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => 
            (pd is GainBuffProcedureDefinition g && !g.BuffEntry.Friendly) ||
            (pd is RemoveArmorProcedureDefinition)),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd =>
                (pd is GainBuffProcedureDefinition g && !g.BuffEntry.Friendly) ||
                (pd is RemoveArmorProcedureDefinition);
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (!cond)
                    newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition CostHealthMutate = new(
        skillDefinition => true,
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                newProcedureDefinitions.Add(cloned);
            }

            CostDefinition oldCostDefinition = skillDefinition.GetCostDefinition();
            HealthCostDefinition newCostDefinition;

            if (oldCostDefinition is ArmorCostDefinition armorCostDefinition)
            {
                newCostDefinition = new HealthCostDefinition(Mathf.Max(1, armorCostDefinition.Value), armorCostDefinition.Closures);
            }
            else if (oldCostDefinition is ChannelCostDefinition channelCostDefinition)
            {
                newCostDefinition = new HealthCostDefinition(Mathf.Max(1, channelCostDefinition.Value * 30), channelCostDefinition.Closures);
            }
            else if (oldCostDefinition is EmptyCostDefinition emptyCostDefinition)
            {
                newCostDefinition = new HealthCostDefinition(1, emptyCostDefinition.Closures);
            }
            else if (oldCostDefinition is HealthCostDefinition healthCostDefinition)
            {
                newCostDefinition = new HealthCostDefinition((healthCostDefinition.Value + 1) / 2, healthCostDefinition.Closures);
            }
            else if (oldCostDefinition is ManaCostDefinition manaCostDefinition)
            {
                newCostDefinition = new HealthCostDefinition(Mathf.Max(1, manaCostDefinition.Value * 6), manaCostDefinition.Closures);
            }
            else
            {
                newCostDefinition = new HealthCostDefinition(1, oldCostDefinition.Closures);
            }

            return SkillDefinition.FromDefinition(newCostDefinition, newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition CycleMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => pd is CycleProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned is CycleProcedureDefinition c)
                {
                    c.WuXing = c.WuXing.Next;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition AccumulateMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd =>
            pd is CycleProcedureDefinition ||
            pd is FollowingCycleProcedureDefinition ||
            (pd is GainBuffProcedureDefinition gainBuff && gainBuff.BuffEntry.BuffStackRule != BuffStackRule.One) ||
            pd is GiveBuffProcedureDefinition giveBuff && giveBuff.BuffEntry.BuffStackRule != BuffStackRule.One),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned is CycleProcedureDefinition c)
                {
                    c.Gain += 1;
                }
                else if (cloned is FollowingCycleProcedureDefinition fc)
                {
                    fc.Gain += 1;
                }
                else if (cloned is GainBuffProcedureDefinition gainBuff && gainBuff.BuffEntry.BuffStackRule != BuffStackRule.One)
                {
                    gainBuff.Stack += 1;
                }
                else if (cloned is GiveBuffProcedureDefinition giveBuff && giveBuff.BuffEntry.BuffStackRule != BuffStackRule.One)
                {
                    giveBuff.Stack += 1;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition ManaMutate = new(
        skillDefinition => skillDefinition.GetCostDefinition() is ManaCostDefinition ||
                           skillDefinition.GetProcedureDefinitions().AnyMatch(pd =>
                               pd is GainBuffProcedureDefinition gainBuff && gainBuff.BuffEntry == Encyclopedia.BuffCategory.FromName("灵气")),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();

                if (cloned is GainBuffProcedureDefinition gainBuff && gainBuff.BuffEntry == Encyclopedia.BuffCategory.FromName("灵气"))
                {
                    gainBuff.Stack += 2;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            if (skillDefinition.GetCostDefinition() is ManaCostDefinition manaCost)
            {
                ManaCostDefinition newManaCost = new ManaCostDefinition(
                    Math.Max(0, manaCost.Value - 2),
                    manaCost.Closures
                );
                return SkillDefinition.FromDefinition(newManaCost, newProcedureDefinitions.ToArray());
            }
            else
            {
                return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition(), newProcedureDefinitions.ToArray());
            }
        });
    
    public static readonly MutateDefinition CritMutate = new(
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
    
    public static readonly MutateDefinition StartStageMutate = new(
        skillDefinition => true,
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                newProcedureDefinitions.Add(cloned);
            }
    
            ProcedureDefinition startStage = new DirectProcedureDefinition(async d =>
                {
                    if (!d.Recursive)
                        return;
                    await d.Caster.CastProcedure(d.Skill, false);
                }).SetPostCondDefinition(PostCondDefinition.StartStage)
                .SetDescription((d, procedureDefinition, costResult, castResult) => d.Join($"开局：使用一次"));
            newProcedureDefinitions.Add(startStage);
    
            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static readonly MutateDefinition PenetrateMutate = new(
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

    public static readonly MutateDefinition ExhaustMutate = new(
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

    public static readonly MutateDefinition SwiftMutate = new(
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

    public static readonly MutateDefinition RemoveCondMutate = new(
        skillDefinition => null != skillDefinition.GetProcedureDefinitions().FirstObj(pd => pd.GetPostCondDefinition() != PostCondDefinition.Default),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned.GetPostCondDefinition() == PostCondDefinition.HasOtherAttack)
                    continue;

                cloned.SetPostCondDefinition(PostCondDefinition.Default);
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    public static readonly MutateDefinition LifeStealMutate = new(
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
    
    // LongevityMutate
    
    public static readonly MutateDefinition CostChannelMutate = new(
        skillDefinition => skillDefinition.GetCostDefinition() is ChannelCostDefinition,
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                newProcedureDefinitions.Add(cloned);
            }

            // 克隆吟唱成本定义并将消耗减少1点，最小为0
            ChannelCostDefinition oldChannelCost = skillDefinition.GetCostDefinition() as ChannelCostDefinition;
            ChannelCostDefinition newChannelCost = new ChannelCostDefinition(
                Math.Max(0, oldChannelCost.Value - 1),  // 吟唱消耗-1，最小为0
                oldChannelCost.Closures
            );

            return SkillDefinition.FromDefinition(newChannelCost, newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition RemoveForbiddenMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => 
            pd is GainBuffProcedureDefinition g && g.BuffEntry.IsForbiddenDebuff),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            Predicate<ProcedureDefinition> pred = pd =>
                pd is GainBuffProcedureDefinition g && g.BuffEntry.IsForbiddenDebuff;
            
            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                bool cond = pred(oldProcedureDefinitions[i]);
                if (!cond)
                    newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });
    
    public static readonly MutateDefinition BerserkMutate = new(
        skillDefinition => skillDefinition.GetProcedureDefinitions().AnyMatch(pd => pd is AttackProcedureDefinition),
        skillDefinition =>
        {
            ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
            List<ProcedureDefinition> newProcedureDefinitions = new();

            for (int i = 0; i < oldProcedureDefinitions.Length; i++)
            {
                ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
                
                if (cloned is AttackProcedureDefinition a)
                {
                    a.Times += 1;
                }
                
                newProcedureDefinitions.Add(cloned);
            }

            return SkillDefinition.FromDefinition(skillDefinition.GetCostDefinition().Clone(), newProcedureDefinitions.ToArray());
        });

    // public static readonly MutateDefinition RemoveCostMutate = new(
    //     skillDefinition => !(skillDefinition.GetCostDefinition() is EmptyCostDefinition),
    //     skillDefinition =>
    //     {
    //         ProcedureDefinition[] oldProcedureDefinitions = skillDefinition.GetProcedureDefinitions();
    //         List<ProcedureDefinition> newProcedureDefinitions = new();
    //         
    //         for (int i = 0; i < oldProcedureDefinitions.Length; i++)
    //         {
    //             ProcedureDefinition cloned = oldProcedureDefinitions[i].Clone();
    //             
    //             newProcedureDefinitions.Add(cloned);
    //         }
    //
    //         return SkillDefinition.FromDefinition(new EmptyCostDefinition(), newProcedureDefinitions.ToArray());
    //     });
}