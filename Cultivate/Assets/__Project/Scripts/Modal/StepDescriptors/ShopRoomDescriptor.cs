
using CLLibrary;

public class ShopRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        Pool<RoomEntry> shopPool = new();
        
        shopPool.Populate("黑市");
        shopPool.Populate("收藏家");
        shopPool.Populate("以物易物");
        shopPool.Populate("毕业季");
        shopPool.Populate("盲盒");
        
        shopPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        shopPool.Shuffle();

        shopPool.TryPopItem(out RoomEntry entry);
        return entry;
    }

    public override SpriteEntry GetSprite()
        => "ShopRoomIcon";

    public ShopRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
