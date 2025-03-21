
using System;
using System.Linq;

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
    
    private RoomDescriptor[][] _levels;
    public RoomDescriptor[][] Levels => _levels;

    public MapEntry(
        string id,
        JingJie envJingJie,
        int slotCount,
        int gold,
        JingJie skillJingJie,
        int skillCount,
        RoomDescriptor[][] levelsLayout,
        Action<RunEnvironment> onStartRun = null) : base(id)
    {
        _envJingJie = envJingJie;
        _slotCount = slotCount;
        _gold = gold;
        _skillJingJie = skillJingJie;
        _skillCount = skillCount;
        _levels = CompileLevels(levelsLayout);
        _onStartRun = onStartRun;
    }

    private RoomDescriptor[][] CompileLevels(RoomDescriptor[][] levelsLayout)
    {
        return levelsLayout
            .Select(level => level.Where(room => room != null).ToArray())
            .ToArray();
    }

    public static implicit operator MapEntry(string id) => Encyclopedia.MapCategory[id];

    public RoomDescriptor GetStepDescriptorFromLevelAndStep(int levelIndex, int stepIndex)
        => _levels[levelIndex][stepIndex];

    public void OnStartRun(RunEnvironment env) => _onStartRun?.Invoke(env);
}
