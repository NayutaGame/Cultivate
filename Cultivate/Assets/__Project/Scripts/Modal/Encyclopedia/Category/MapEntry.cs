
using System;

[Serializable]
public class MapEntry : Entry
{
    public string GetName() => GetId();
    
    [NonSerialized] public JingJie _envJingJie;
    [NonSerialized] public int _slotCount;
    [NonSerialized] public int _gold;
    [NonSerialized] public JingJie _skillJingJie;
    [NonSerialized] public int _skillCount;
    [NonSerialized] public Action<RunEnvironment> _onStartRun;
    
    private RoomDefinition[][] _levels;
    public RoomDefinition[][] Levels => _levels;

    public MapEntry(
        string id,
        JingJie envJingJie,
        int slotCount,
        int gold,
        JingJie skillJingJie,
        int skillCount,
        RoomDefinition[][] levels,
        Action<RunEnvironment> onStartRun = null) : base(id)
    {
        _envJingJie = envJingJie;
        _slotCount = slotCount;
        _gold = gold;
        _skillJingJie = skillJingJie;
        _skillCount = skillCount;
        _levels = levels;
        _onStartRun = onStartRun;
    }

    public static implicit operator MapEntry(string id) => Encyclopedia.MapCategory[id];

    public RoomDefinition GetStepDescriptorFromLevelAndStep(int levelIndex, int stepIndex)
        => _levels[levelIndex][stepIndex];

    public void OnStartRun(RunEnvironment env) => _onStartRun?.Invoke(env);
}
