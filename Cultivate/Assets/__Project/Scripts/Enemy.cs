using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private RunChip[] _equippedNeiGong;
    private RunChip[] _equippedWaiGong;

    public Enemy(RunChip[] equippedNeiGong, RunChip[] equippedWaiGong)
    {
        _equippedNeiGong = equippedNeiGong;
        _equippedWaiGong = equippedWaiGong;
    }

    public RunChip GetNeiGong(int i) => _equippedNeiGong[i];
    public RunChip GetWaiGong(int i) => _equippedWaiGong[i];
}
