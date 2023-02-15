using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageEntity
{
    public int Health;
    public int Armor;

    public StageNeiGong[] _neiGongList;
    public StageWaiGong[] _waiGongList;
    public int _p;

    public void Execute()
    {
        StageWaiGong chip = _waiGongList[_p];
        Debug.Log($"p = {_p}");
        chip.Execute();
        _p = (_p + 1) % _waiGongList.Length;
    }
}
