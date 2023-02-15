using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHero
{
    public int MingYuan { get; private set; }
    public int WuXing { get; private set; }
    public int XiuWei { get; private set; }
    public int Health { get; private set; }
    public int Mana { get; private set; }

    // 灵根
    // 神识
    // 遁速
    // 心境

    private Tile[] _equippedNeiGong;
    public int NeiGongCount => RunManager.NeiGongLimit;
    private Tile[] _equippedWaiGong;
    public int WaiGongCount => RunManager.WaiGongLimit;

    public RunHero(int mingYuan = 100, int wuXing = 0, int xiuWei = 0, int health = 40, int mana = 0)
    {
        MingYuan = mingYuan;
        WuXing = wuXing;
        XiuWei = xiuWei;
        Health = health;
        Mana = mana;

        _equippedNeiGong = new Tile[RunManager.NeiGongLimit];
        _equippedWaiGong = new Tile[RunManager.WaiGongLimit];
    }

    public RunChip GetNeiGong(int i)
    {
        if(_equippedNeiGong[i] == null)
            return null;
        return _equippedNeiGong[i].RunChip;
    }

    public RunChip GetWaiGong(int i)
    {
        if(_equippedWaiGong[i] == null)
            return null;
        return _equippedWaiGong[i].RunChip;
    }

    public bool TryRemoveNeiGongTile(Tile tile)
    {
        for (int i = 0; i < _equippedNeiGong.Length; i++)
        {
            if (tile == _equippedNeiGong[i])
            {
                _equippedNeiGong[i] = null;
                return true;
            }
        }

        return false;
    }

    public void EquipNeiGong(Tile tile, int atIndex)
    {
        _equippedNeiGong[atIndex] = tile;
    }

    public bool TryRemoveWaiGongTile(Tile tile)
    {
        for (int i = 0; i < _equippedWaiGong.Length; i++)
        {
            if (tile == _equippedWaiGong[i])
            {
                _equippedWaiGong[i] = null;
                return true;
            }
        }

        return false;
    }

    public void EquipWaiGong(Tile tile, int atIndex)
    {
        _equippedWaiGong[atIndex] = tile;
    }

    public void SwapNeiGong(int from, int to)
    {
        (_equippedNeiGong[from], _equippedNeiGong[to]) = (_equippedNeiGong[to], _equippedNeiGong[from]);
    }

    public void SwapWaiGong(int from, int to)
    {
        (_equippedWaiGong[from], _equippedWaiGong[to]) = (_equippedWaiGong[to], _equippedWaiGong[from]);
    }
}
