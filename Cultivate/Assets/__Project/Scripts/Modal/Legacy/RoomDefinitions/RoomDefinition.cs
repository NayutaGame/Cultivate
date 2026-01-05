
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public abstract class RoomDefinition
{
    private static readonly int[] GoldRewardFromLadder = new int[]
    {
        1, /*1,*/ 2,
        2, 2, 4,
        4, 4, 8,
        8, 8, 16,
        16, 16, 16,
    };
    
    private static readonly int[] HealthRewardFromLadder = new int[]
    {
        1, /*1,*/ 2,
        2, 2, 4,
        4, 4, 8,
        8, 8, 16,
        16, 16, 16,
    };
    
    public static int GetGoldRewardFromLadder(int ladder)
        => GoldRewardFromLadder[ladder.Clamp(0, GoldRewardFromLadder.Length - 1)];
    
    public static int GetHealthRewardFromLadder(int ladder)
        => HealthRewardFromLadder[ladder.Clamp(0, HealthRewardFromLadder.Length - 1)];

    public static int GetCardBasePriceFromLadder(int ladder)
        => GetCardBasePriceFromJingJie(GetJingJieFromLadder(ladder));

    private static readonly JingJie[] JingJieFromLadder = new JingJie[]
    {
        JingJie.LianQi, /*JingJie.LianQi,*/ JingJie.LianQi,
        JingJie.ZhuJi, JingJie.ZhuJi, JingJie.ZhuJi,
        JingJie.JinDan, JingJie.JinDan, JingJie.JinDan,
        JingJie.YuanYing, JingJie.YuanYing, JingJie.YuanYing,
        JingJie.HuaShen, JingJie.HuaShen, JingJie.HuaShen,
    };

    public static JingJie GetJingJieFromLadder(int ladder)
        => JingJieFromLadder[ladder.Clamp(0, JingJieFromLadder.Length - 1)];

    private static readonly int[] CardBasePriceFromJingJie = new int[] { 1, 2, 4, 8, 16, 16 };

    public static int GetCardBasePriceFromJingJie(JingJie jingJie)
        => CardBasePriceFromJingJie[((int)jingJie).Clamp(0, CardBasePriceFromJingJie.Length - 1)];
    
    public abstract LegacyRoomEntry Draw(Map map, LegacyRoom room);

    public abstract string GetTitle();
    public abstract SpriteEntry GetSprite();
    public abstract Description GetDescription();

    [SerializeField] private int _ladder;
    public int Ladder => _ladder;

    [NonSerialized] public Func<Profile, RunEnvironment, bool> Pred;
    
    public RoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred)
    {
        _ladder = ladder;
        Pred = pred;
    }
}
