
using System.Collections.Generic;

public class ConfirmSkillsSignal : Signal
{
    public List<SkillEntryDescriptor> Selected;

    public ConfirmSkillsSignal(List<SkillEntryDescriptor> selected)
    {
        Selected = selected;
    }
}
