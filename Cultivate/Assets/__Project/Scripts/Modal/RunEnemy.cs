using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnemy
{
    public int Health { get; private set; }

    private RunChip[] _equippedNeiGong;
    public int NeiGongCount => RunManager.NeiGongLimit;
    private RunChip[] _equippedWaiGong;
    public int WaiGongCount => RunManager.WaiGongLimit;

    public RunEnemy(int health, RunChip[] equippedNeiGong, RunChip[] equippedWaiGong)
    {
        Health = health;
        _equippedNeiGong = equippedNeiGong;
        _equippedWaiGong = equippedWaiGong;
    }

    public RunChip GetNeiGong(int i) => _equippedNeiGong[i];
    public RunChip GetWaiGong(int i) => _equippedWaiGong[i];
}
