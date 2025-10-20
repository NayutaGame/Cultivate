
public class RoomGraph
{
    public RoomGraph()
    {
    }

    public static Cell CreateRoomGraph(MapNode mapNode, JingJie jingJie, int ladder, RunEntity home)
    {
        int baseGoldReward = RoomDefinition.GetGoldRewardFromLadder(ladder);
        DialogCell A = new DialogCell(
                titleText: "存钱",
                detailedText: $"获得了{baseGoldReward}金钱")
            .SetReward(Reward.FromGold(baseGoldReward));
        return A;
    }
}