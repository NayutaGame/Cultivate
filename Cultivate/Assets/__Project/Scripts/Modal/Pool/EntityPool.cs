
using CLLibrary;

public class EntityPool : Pool<RunEntity>
{
    public bool TryDrawEntity(out RunEntity template, EntityDescriptor d)
    {
        Shuffle();
        bool success = TryPopItem(out template, d.CanDraw);
        template ??= RunEntity.Default();
        return success;
    }
}
