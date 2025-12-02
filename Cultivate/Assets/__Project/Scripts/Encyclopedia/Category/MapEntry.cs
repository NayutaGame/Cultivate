
using System;

[Serializable]
public class MapEntry : Entry
{
    [NonSerialized] public JingJie _envJingJie;
    [NonSerialized] public int _slotCount;
    [NonSerialized] public int _gold;
    [NonSerialized] private RoomDefinition[][] _levels;

    public MapEntry(
        string id,
        string name,
        JingJie envJingJie,
        int slotCount,
        int gold,
        RoomDefinition[][] levels) : base(id, name)
    {
        _envJingJie = envJingJie;
        _slotCount = slotCount;
        _gold = gold;
        _levels = levels;
    }
    
    public RoomDefinition[][] Levels => _levels;

    public RoomDefinition GetStepDescriptorFromLevelAndStep(int levelIndex, int stepIndex)
        => _levels[levelIndex][stepIndex];
}
