
using CLLibrary;

public class ShopStepDescriptor : StepDescriptor
{
    public override RunNode Draw(Map map)
    {
        Pool<NodeEntry> shopPool = new();
        
        shopPool.Populate("黑市");
        shopPool.Populate("收藏家");
        shopPool.Populate("以物易物");
        shopPool.Populate("毕业季");
        shopPool.Populate("盲盒");
        
        shopPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        shopPool.Shuffle();

        shopPool.TryPopItem(out var entry);
        return new RunNode(entry, Ladder);
    }

    public ShopStepDescriptor(int ladder) : base(ladder)
    {
    }
}
