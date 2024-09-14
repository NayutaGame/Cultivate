
using UnityEngine;

public class RunNode
{
    protected NodeEntry _entry;
    public NodeEntry Entry => _entry;

    public virtual Sprite GetSprite() => _entry.GetSprite();
    public virtual string GetName() => _entry.GetName();
    public virtual string GetDescription() => _entry.GetDescription();

    public RunNode(NodeEntry entry)
    {
        _entry = entry ?? Encyclopedia.NodeCategory.DefaultEntry();
    }
}
