
using System.Collections.Generic;

public class ConfirmSkillsSignal : Signal
{
    public List<SkillDescriptor> Selected;

    public ConfirmSkillsSignal(List<SkillDescriptor> selected)
    {
        Selected = selected;
    }
}
