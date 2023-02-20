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

    private List<AcquiredChip> _unequipped;
    public int UnequippedCount => _unequipped.Count;
    public AcquiredChip TryGetUnequipped(int i)
    {
        if (i >= _unequipped.Count)
            return null;
        return _unequipped[i];
    }
    public void AddAcquiredChip(AcquiredChip acquiredChip) => _unequipped.Add(acquiredChip);

    private AcquiredChip[] _equippedNeiGong;
    public int NeiGongCount => RunManager.NeiGongLimit;
    public AcquiredChip GetNeiGong(int i) => _equippedNeiGong[i];

    private AcquiredChip[] _equippedWaiGong;
    public int WaiGongCount => RunManager.WaiGongLimit;
    public AcquiredChip GetWaiGong(int i) => _equippedWaiGong[i];

    public RunHero(int mingYuan = 100, int wuXing = 0, int xiuWei = 0, int health = 40, int mana = 0)
    {
        MingYuan = mingYuan;
        WuXing = wuXing;
        XiuWei = xiuWei;
        Health = health;
        Mana = mana;

        _unequipped = new List<AcquiredChip>();
        _equippedNeiGong = new AcquiredChip[RunManager.NeiGongLimit];
        _equippedWaiGong = new AcquiredChip[RunManager.WaiGongLimit];
    }

    public void SwapUnequipped(int from, int to)
    {
        (_unequipped[from], _unequipped[to]) = (_unequipped[to], _unequipped[from]);
    }

    public void SwapNeiGong(int from, int to)
    {
        (_equippedNeiGong[from], _equippedNeiGong[to]) = (_equippedNeiGong[to], _equippedNeiGong[from]);
    }

    public void SwapWaiGong(int from, int to)
    {
        (_equippedWaiGong[from], _equippedWaiGong[to]) = (_equippedWaiGong[to], _equippedWaiGong[from]);
    }

    public bool TryEquipNeiGong(int from, int to)
    {
        if (!_unequipped[from].IsNeiGong) return false;

        if(_equippedNeiGong[to] != null)
            UnequipNeiGong(to);

        _equippedNeiGong[to] = _unequipped[from];
        _unequipped.RemoveAt(from);
        return true;
    }

    public bool TryEquipWaiGong(int from, int to)
    {
        if (!_unequipped[from].IsWaiGong) return false;

        if(_equippedWaiGong[to] != null)
            UnequipWaiGong(to);

        _equippedWaiGong[to] = _unequipped[from];
        _unequipped.RemoveAt(from);
        return true;
    }

    public void UnequipNeiGong(int neiGong)
    {
        _unequipped.Add(_equippedNeiGong[neiGong]);
        _equippedNeiGong[neiGong] = null;
    }

    public void UnequipWaiGong(int waiGong)
    {
        _unequipped.Add(_equippedWaiGong[waiGong]);
        _equippedWaiGong[waiGong] = null;
    }
}
