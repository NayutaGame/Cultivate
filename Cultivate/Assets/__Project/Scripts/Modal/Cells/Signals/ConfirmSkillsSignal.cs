
using System.Collections.Generic;

public class ConfirmSkillsSignal : Signal
{
    public List<int> PickedIndices;

    public ConfirmSkillsSignal(List<int> pickedIndices)
    {
        PickedIndices = new();
        foreach (int i in pickedIndices)
            PickedIndices.Add(i);
    }
}
