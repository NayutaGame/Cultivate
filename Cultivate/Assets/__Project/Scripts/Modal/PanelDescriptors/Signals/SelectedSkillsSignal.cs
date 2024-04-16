
using System.Collections.Generic;

public class SelectedSkillsSignal : Signal
{
    public List<SkillDescriptor> Selected;

    public SelectedSkillsSignal(List<SkillDescriptor> selected)
    {
        Selected = selected;
    }
}
