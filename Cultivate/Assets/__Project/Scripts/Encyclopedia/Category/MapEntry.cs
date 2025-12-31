
using System;

[Serializable]
public class MapEntry : Entry
{
    [NonSerialized] public JingJie _envJingJie;
    [NonSerialized] public int _slotCount;
    [NonSerialized] private RoomDefinition[][] _levels;

    public MapEntry(
        string id,
        string name,
        JingJie envJingJie,
        int slotCount,
        RoomDefinition[][] levels) : base(id, name)
    {
        _envJingJie = envJingJie;
        _slotCount = slotCount;
        _levels = levels;
    }
    
    public RoomDefinition[][] Levels => _levels;

    public RoomDefinition GetStepDescriptorFromLevelAndStep(int levelIndex, int stepIndex)
        => _levels[levelIndex][stepIndex];
}
