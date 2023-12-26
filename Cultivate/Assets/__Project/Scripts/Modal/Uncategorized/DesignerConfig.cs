
using System.Collections.Generic;

public class DesignerConfig
{
    public Dictionary<int, RunEventDescriptor> _runEventDescriptorDict;
    public Dictionary<int, StageEventDescriptor> _stageEventDescriptorDict;

    public DesignerConfig(RunEventDescriptor[] runEventDescriptors = null, StageEventDescriptor[] stageEventDescriptors = null)
    {
        _runEventDescriptorDict = new();
        if (runEventDescriptors != null)
            foreach (var d in runEventDescriptors)
                _runEventDescriptorDict[d.EventId] = d;

        _stageEventDescriptorDict = new();
        if (stageEventDescriptors != null)
            foreach (var d in stageEventDescriptors)
                _stageEventDescriptorDict[d.EventId] = d;
    }
}
