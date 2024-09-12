
using System;

public class AdventureStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        Predicate<NodeEntry> pred = e => e.CanCreate(map, Ladder);
        NodeEntry entry;
        
        if (map.InsertedAdventurePool.TryPopItem(out entry, pred: pred))
        {
            
        }
        else if (map.AdventurePool.TryPopItem(out entry, pred: pred))
        {
            
        }
        else
        {
            entry = "不存在的事件";
        }
        
        return new RunNode(entry, Ladder);
    }

    public AdventureStepDescriptor(int ladder) : base(ladder)
    {
    }
}
