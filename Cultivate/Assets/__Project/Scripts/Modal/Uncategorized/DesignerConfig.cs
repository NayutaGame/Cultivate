
using System;
using System.Collections.Generic;

public class DesignerConfig
{
    private Action<Map> _initMapPools;

    public Dictionary<int, CLEventDescriptor> _eventDescriptorDict;

    public DesignerConfig(Action<Map> initMapPools = null,
        params CLEventDescriptor[] eventDescriptors)
    {
        _initMapPools = initMapPools;

        _eventDescriptorDict = new Dictionary<int, CLEventDescriptor>();
        foreach (var eventDescriptor in eventDescriptors)
            _eventDescriptorDict[eventDescriptor.EventId] = eventDescriptor;
    }

    public void InitMapPools(Map map)
    {
        _initMapPools?.Invoke(map);
    }
}
