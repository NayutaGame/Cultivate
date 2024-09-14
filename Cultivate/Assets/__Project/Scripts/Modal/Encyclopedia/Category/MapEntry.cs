
using System;

[Serializable]
public class MapEntry : Entry
{
    public string GetName() => GetId();
    public JingJie _envJingJie;
    public int _slotCount;
    public int _gold;
    public JingJie _skillJingJie;
    public int _skillCount;
    private RoomDescriptor[][] _levels;
    public RoomDescriptor[][] Levels => _levels;

    public MapEntry(
        string id,
        JingJie envJingJie,
        int slotCount,
        int gold,
        JingJie skillJingJie,
        int skillCount,
        RoomDescriptor[][] levels) : base(id)
    {
        _envJingJie = envJingJie;
        _slotCount = slotCount;
        _gold = gold;
        _skillJingJie = skillJingJie;
        _skillCount = skillCount;
        _levels = levels;
    }

    public static implicit operator MapEntry(string id) => Encyclopedia.MapCategory[id];

    public RoomDescriptor GetStepDescriptorFromLevelAndStep(int levelIndex, int stepIndex)
        => _levels[levelIndex][stepIndex];
}
