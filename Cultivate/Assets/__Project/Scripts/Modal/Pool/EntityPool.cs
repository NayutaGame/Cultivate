
using System;
using CLLibrary;

[Serializable]
public class EntityPool : Pool<RunEntity>
{
    public bool TryDrawEntity(out RunEntity template, EntityDescriptor d)
    {
        bool success = TryPopItem(out template, d.CanDraw);
        Shuffle();
        template ??= RunEntity.Default();
        return success;
    }
}
