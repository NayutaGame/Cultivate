
using System;
using UnityEngine;

[Serializable]
public abstract class RoomDescriptor
{
    public static readonly int[] GoldRewardTable = new int[]
    {
        1, /*1,*/ 2,
        2, 2, 4,
        4, 4, 8,
        8, 8, 16,
        16, 16, 16,
    };
    
    public abstract RoomEntry Draw(Map map, Room room);

    public abstract SpriteEntry GetSprite();
    public abstract string GetDescription();

    [SerializeField] private int _ladder;
    public int Ladder => _ladder;
    
    public RoomDescriptor(int ladder)
    {
        _ladder = ladder;
    }
}
