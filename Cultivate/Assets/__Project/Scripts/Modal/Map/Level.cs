
using System;
using System.Collections.Generic;

public class Level : Addressable
{
    private StepItemListModel _stepItems;

    public StepItem GetStepItem(int stepIndex)
        => _stepItems[stepIndex];
    public int GetStepCount()
        => _stepItems.Count();
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Level(StepDescriptor[] stepDescriptors)
    {
        _accessors = new()
        {
            { "StepItems", () => _stepItems },
        };
        
        _stepItems = new();
        for (int i = 0; i < stepDescriptors.Length; i++)
        {
            _stepItems.Add(new StepItem(stepDescriptors[i]));
        }
    }
}
