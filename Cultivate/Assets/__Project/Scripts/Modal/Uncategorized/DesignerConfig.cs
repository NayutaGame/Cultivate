
using System;

public class DesignerConfig
{
    public RunEventDescriptor[] _runEventDescriptors;
    public StageEventDescriptor[] _stageEventDescriptors;

    public DesignerConfig(RunEventDescriptor[] runEventDescriptors = null, StageEventDescriptor[] stageEventDescriptors = null)
    {
        _runEventDescriptors = runEventDescriptors ?? Array.Empty<RunEventDescriptor>();
        _stageEventDescriptors = stageEventDescriptors ?? Array.Empty<StageEventDescriptor>();
    }
}
