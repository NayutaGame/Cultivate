
using CLLibrary;

public class ShopStepDescriptor : StepDescriptor
{
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();

        Pool<NodeEntry> shopPool = new();
        
        shopPool.Populate("黑市");
        shopPool.Populate("收藏家");
        shopPool.Populate("以物易物");
        shopPool.Populate("毕业季");
        shopPool.Populate("盲盒");
        
        shopPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        shopPool.Shuffle();

        NodeEntry entry;
        shopPool.TryPopItem(out entry);
        map.CurrStepItem._nodes.Add(new RunNode(entry, Ladder));
        map.CurrStepItem._nodes.Add(new RunNode("存钱", Ladder));
    }

    public ShopStepDescriptor(int ladder) : base(ladder)
    {
    }
}
