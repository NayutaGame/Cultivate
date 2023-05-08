
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : Inventory<RunSkill>, IDragDrop
{
    public void RefreshChip()
    {
        Clear();
        foreach (var chip in Encyclopedia.SkillCategory.Traversal)
        {
            RunSkill skill = new RunSkill(chip, chip.JingJieRange.Start);
            skill.SetDragDropDelegate(GetDragDropDelegate());
            Add(skill);
        }
    }

    public RunSkill PickSkill(SkillEntry e, JingJie? jingJie)
    {
        RunSkill picked = new RunSkill(e, jingJie ?? e.JingJieRange.Start);
        Add(picked);
        return picked;
    }

    #region IDragDrop

    private DragDropDelegate _dragDropDelegate;

    public DragDropDelegate GetDragDropDelegate()
        => _dragDropDelegate;

    public void SetDragDropDelegate(DragDropDelegate dragDropDelegate)
        => _dragDropDelegate = dragDropDelegate;

    #endregion
}
