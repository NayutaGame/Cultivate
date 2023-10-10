
using CLLibrary;

public class EntityPool : Pool<RunEntity>
{
    public EntityPool()
    {
        Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
    }

    public bool TryDrawEntity(out RunEntity template, DrawEntityDetails d)
    {
        Shuffle();
        bool success = TryPopItem(out template, d.CanDraw);
        if (success)
            return true;

        template = RunEntity.Default();
        return false;
    }
}
