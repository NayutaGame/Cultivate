
using System;
using CLLibrary;

[Serializable]
public class EntityPool : FinitePool<RunEntity>
{
    public bool TryDrawEntity(out RunEntity template, EntityQuery d)
    {
        bool success = TryPopItem(out template, d.Matches);
        Shuffle();
        template ??= RunEntity.Default();
        return success;
    }
}
