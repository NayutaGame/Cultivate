
using System;
using CLLibrary;

[Serializable]
public class ShopRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        FinitePool<RoomEntry> roomPool = new();
        
        roomPool.Populate(Encyclopedia.RoomCategory.FromName("黑市"));
        roomPool.Populate(Encyclopedia.RoomCategory.FromName("收藏家"));
        roomPool.Populate(Encyclopedia.RoomCategory.FromName("以物易物"));
        roomPool.Populate(Encyclopedia.RoomCategory.FromName("毕业季"));
        roomPool.Populate(Encyclopedia.RoomCategory.FromName("盲盒"));
        
        roomPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        roomPool.Shuffle();

        roomPool.TryPopItem(out RoomEntry entry);
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
