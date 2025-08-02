
using System;
using CLLibrary;

[Serializable]
public class ShopRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        Pool<RoomEntry> shopPool = new();
        
        shopPool.Populate(Encyclopedia.RoomCategory.FromName("黑市"));
        shopPool.Populate(Encyclopedia.RoomCategory.FromName("收藏家"));
        shopPool.Populate(Encyclopedia.RoomCategory.FromName("以物易物"));
        shopPool.Populate(Encyclopedia.RoomCategory.FromName("毕业季"));
        shopPool.Populate(Encyclopedia.RoomCategory.FromName("盲盒"));
        
        shopPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        shopPool.Shuffle();

        shopPool.TryPopItem(out RoomEntry entry);
        return entry;
    }

    public override string GetTitle()
        => "商店房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("ShopRoomIcon");

    public override Description GetDescription()
        => new("可以购买东西");

    public ShopRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }
}
