
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
                env.UnequipProcedure(skillSlot, null);
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
}
