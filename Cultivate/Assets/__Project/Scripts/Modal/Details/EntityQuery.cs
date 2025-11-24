using CLLibrary;

public sealed class EntityQuery
{
    private EntityEntry _entry;
    private int? _ladder;
    private int? _targetDifficulty;
    private bool _limitToPool;

    public bool LimitToPool => _limitToPool;
    public EntityEntry Entry => _entry;

    private EntityQuery(
        EntityEntry entry = null,
        int? ladder = null,
        int? targetDifficulty = null,
        bool limitToPool = true)
    {
        _entry = entry;
        _ladder = ladder;
        _targetDifficulty = targetDifficulty;
        _limitToPool = limitToPool;
    }

    public static EntityQuery AnyEntity()
        => new();

    public static EntityQuery FromEntry(EntityEntry entry)
        => new(entry: entry);

    public static EntityQuery FromName(string name)
        => new(entry: Encyclopedia.EntityCategory.FromName(name), limitToPool: false);

    public static EntityQuery FromLadder(int ladder)
        => new(ladder: ladder);

    public static EntityQuery FromEditorQuery(EditorEntityQuery editorQuery)
        => new(
            entry: string.IsNullOrEmpty(editorQuery.EntryName) ? null : Encyclopedia.EntityCategory.FromName(editorQuery.EntryName),
            ladder: editorQuery.Ladder >= 0 ? editorQuery.Ladder : null,
            targetDifficulty: editorQuery.TargetDifficulty >= 0 ? editorQuery.TargetDifficulty : null,
            limitToPool: editorQuery.LimitToPool);

    public bool Matches(RunEntity entity)
    {
        if (_entry != null && entity.GetModel() != _entry)
            return false;

        if (_ladder.HasValue && entity.GetLadder() != _ladder.Value)
            return false;

        if (_targetDifficulty.HasValue)
        {
            Bound entityAllowedDifficulty = entity.GetAllowedDifficulty();
            if (!entityAllowedDifficulty.Contains(_targetDifficulty.Value))
                return false;
        }

        if (_limitToPool && !entity.IsInPool())
            return false;

        return true;
    }
}
