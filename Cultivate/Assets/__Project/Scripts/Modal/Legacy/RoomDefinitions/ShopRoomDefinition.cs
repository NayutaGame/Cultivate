
using System;
using CLLibrary;

[Serializable]
public class ShopRoomDefinition : RoomDefinition
{
    public override LegacyRoomEntry Draw(Map map, LegacyRoom room)
    {
        FinitePool<LegacyRoomEntry> roomPool = new();
        
        roomPool.Populate(Encyclopedia.LegacyRoomCategory.FromName("黑市"));
        roomPool.Populate(Encyclopedia.LegacyRoomCategory.FromName("收藏家"));
        roomPool.Populate(Encyclopedia.LegacyRoomCategory.FromName("以物易物"));
        roomPool.Populate(Encyclopedia.LegacyRoomCategory.FromName("毕业季"));
        roomPool.Populate(Encyclopedia.LegacyRoomCategory.FromName("盲盒"));
        
        roomPool.Depopulate(pred: e => !e.LadderBound.Contains(Ladder));
        
        roomPool.Shuffle();

        roomPool.TryPopItem(out LegacyRoomEntry entry);
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
