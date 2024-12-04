
using System;
using System.Collections.Generic;

public class PuzzlePanelDescriptor : PanelDescriptor
{
    private Puzzle _puzzle;

    public string GetDescription() => _puzzle.Description;
    public string GetCondition() => _puzzle.Condition;
    public StageResult GetResult() => _puzzle?.GetResult();

    public PuzzlePanelDescriptor(Puzzle puzzle)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Home",                     () => _puzzle.Home },
            { "Away",                     () => _puzzle.Away },
        };

        _puzzle = puzzle;

        foreach (SkillSlot slot in _puzzle.Home.TraversalCurrentSlots())
        {
            if (slot.Skill != null)
                slot.Skill.Borrowed = true;
        }
    }

    private void RemoveAllBorrowed()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        RunEntity puzzleHome = _puzzle.Home;
        foreach (SkillSlot skillSlot in puzzleHome.TraversalCurrentSlots())
        {
            if (skillSlot.Skill is { Borrowed: true })
                skillSlot.Skill = null;
        }

        foreach (SkillSlot skillSlot in env.Home.TraversalCurrentSlots())
        {
            if (skillSlot.Skill is { Borrowed: true })
                skillSlot.Skill = null;
        }

        List<RunSkill> toRemove = new();
        foreach (RunSkill skill in env.Hand.Traversal())
        {
            if (skill is { Borrowed: true })
                toRemove.Add(skill);
        }

        foreach (RunSkill skill in toRemove)
        {
            env.Hand.Remove(skill);
        }
    }

    private void UnequipAll()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        RunEntity puzzleHome = _puzzle.Home;
        foreach (SkillSlot skillSlot in puzzleHome.TraversalCurrentSlots())
        {
            if (skillSlot.Skill != null)
                env.LegacyUnequipProcedure(skillSlot, null);
        }
    }
    
    private Func<PuzzleResultSignal, PanelDescriptor> _operation;
    public PuzzlePanelDescriptor SetOperation(Func<PuzzleResultSignal, PanelDescriptor> operation)
    {
        _operation = operation;
        return this;
    }
    
    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is PuzzleResultSignal puzzleResultSignal)
        {
            RemoveAllBorrowed();
            UnequipAll();
            // make sure puzzle is disposed
            // refresh hand and field
            return _operation(puzzleResultSignal);
        }
    
        return this;
    }

    public static PuzzlePanelDescriptor GetTemplate()
    {
        Puzzle puzzle = new(
            description: "尝试帮助少年击中目标",
            condition: "目标受到伤害",
            home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
            away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
            {
                RunSkill.FromEntry("0609"),
                RunSkill.FromEntry("0609"),
                RunSkill.FromEntry("0609"),
            }),
            kernel: new StageKernel(async (env, turn, whosTurn, forced) =>
            {
                CommitDetails d = new CommitDetails(env.Entities[whosTurn]);

                await env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                if (forced)
                {
                    d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                }
                else
                {
                    if (d.Cancel)
                        return 0;

                    if (turn < 6)
                        return 0;

                    d.Flag = env.Entities[0].Hp > 0 ? 1 : 2;
                }

                await env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                if (d.Flag == 0)
                    return d.Flag;

                env.Result.Flag = d.Flag;
                env.Result.HomeLeftHp = env.Entities[0].Hp;
                env.Result.AwayLeftHp = env.Entities[1].Hp;
                env.Result.TryAppend(env.Result.Flag == 1 ? $"主场胜利\n" : $"客场胜利\n");
                return d.Flag;
            })
        );
        
        PuzzlePanelDescriptor template = new(puzzle);
        DialogPanelDescriptor pass = new DialogPanelDescriptor("通过", "通过对话");
        DialogPanelDescriptor noPass = new DialogPanelDescriptor("未通过", "未通过对话");
        template.SetOperation(s =>
        {
            if (s.Flag == 1)
            {
                return pass;
            }

            return noPass;
        });
        
        return template;
    }
}
