
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public abstract class RoomDescriptor
{
    private static readonly int[] GoldRewardFromLadder = new int[]
    {
        1, /*1,*/ 2,
        2, 2, 4,
        4, 4, 8,
        8, 8, 16,
        16, 16, 16,
    };
    
    public static int GetGoldRewardFromLadder(int ladder)
        => GoldRewardFromLadder[ladder.Clamp(0, GoldRewardFromLadder.Length - 1)];

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
    
    public abstract RoomEntry Draw(Map map, Room room);

    public abstract SpriteEntry GetSprite();
    public abstract string GetDescription();

    [SerializeField] private readonly int _ladder;
    public int Ladder => _ladder;
    
    public RoomDescriptor(int ladder)
    {
        _ladder = ladder;
    }
}
