
using System.Collections.Generic;

public class ConfirmSkillsSignal : Signal
{
    public List<SkillGhost> Selected;

    public ConfirmSkillsSignal(List<SkillGhost> selected)
    {
        Selected = new();
        foreach (SkillGhost skillReference in selected)
            Selected.Add(skillReference.Clone());
    }
}
