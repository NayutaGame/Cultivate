
using System.Collections.Generic;

public class ConfirmSkillsSignal : Signal
{
    public List<SkillReference> Selected;

    public ConfirmSkillsSignal(List<SkillReference> selected)
    {
        Selected = selected;
    }
}
