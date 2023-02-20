
using System;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public static readonly int NeiGongLimit = 4;
    public static readonly int WaiGongLimit = 12;

    private DanTian _danTian;
    private Inventory _inventory;
    private RunHero _hero;
    public RunHero Hero => _hero;
    private RunEnemy _enemy;
    public RunEnemy Enemy => _enemy;

    private Dictionary<string, Func<IndexPath, object>> _funcList;

    public override void DidAwake()
    {
        base.DidAwake();

        _danTian = new DanTian();
        _inventory = new Inventory();

        _hero = new RunHero();
        _enemy = new RunEnemy(40, new RunChip[]
        {
            null,
            null,
            null,
            null,
        },
        new RunChip[]
        {
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
        });

        _funcList = new()
        {
            { "GetTileXY",            GetTileXY       },
            { "TryGetRunChip",        TryGetRunChip   },
            { "GetStatusString",      GetStatusString },
            { "TryGetAcquiredChip",   TryGetAcquiredChip },
            { "GetHeroNeiGong",       GetHeroNeiGong },
            { "GetHeroWaiGong",       GetHeroWaiGong },
            { "GetEnemyNeiGong",      GetEnemyNeiGong },
            { "GetEnemyWaiGong",      GetEnemyWaiGong },
        };
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._funcList[indexPath._str](indexPath);
    public static T Get<T>(string funcName) => Get<T>(new IndexPath(funcName));
    private object GetTileXY(IndexPath indexPath) => _danTian.GetTileXY(indexPath._ints[0], indexPath._ints[1]);
    private object TryGetRunChip(IndexPath indexPath) => _inventory.TryGetRunChip(indexPath._ints[0]);
    private object GetStatusString(IndexPath indexPath) => $"命元：{_hero.MingYuan}\n悟性：{_hero.WuXing}\n修为：{_hero.XiuWei}\n气血：{_hero.Health}\n初始灵力：{_hero.Mana}";
    private object TryGetAcquiredChip(IndexPath indexPath) => _danTian.TryGetAcquiredChip(indexPath._ints[0]);
    private object GetHeroNeiGong(IndexPath indexPath) => _hero.GetNeiGong(indexPath._ints[0]);
    private object GetHeroWaiGong(IndexPath indexPath) => _hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyNeiGong(IndexPath indexPath) => _enemy.GetNeiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => _enemy.GetWaiGong(indexPath._ints[0]);

    public int GetRunChipCount() => _inventory.GetRunChipCount();
    public int GetAcquiredChipCount() => _danTian.GetAcquiredChipCount();

    public static bool Swap(IndexPath from, IndexPath to)
    {
        Instance._inventory.Swap(from._ints[0], to._ints[0]);
        return true;
    }

    public static bool TryApply(IndexPath from, IndexPath to)
    {
        RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
        Tile tile = (Tile) Instance.GetTileXY(to);

        if (!Instance.CanApply(runChip, tile)) return false;
        Instance.Apply(runChip, tile);
        return true;
    }

    public bool CanApply(RunChip runChip, Tile tile)
    {
        if (tile.RunChip != null) return false;
        // Mana Constraint
        return true;
    }

    public void Apply(RunChip runChip, Tile tile)
    {
        // 修炼case

        _danTian.Acquire(runChip, tile);
        _inventory.Remove(runChip);
    }

    public static bool TryUpgradeDanTian(IndexPath from, IndexPath to)
    {
        RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
        Tile tile = (Tile) Instance.GetTileXY(to);

        if (!Instance.CanUpgradeDanTian(runChip, tile)) return false;
        Instance.UpgradeDanTian(runChip, tile);
        return true;
    }

    public bool CanUpgradeDanTian(RunChip runChip, Tile tile)
    {
        return RunChip.CanUpgrade(runChip, tile.RunChip);
    }

    public void UpgradeDanTian(RunChip runChip, Tile tile)
    {
        tile.RunChip.Level += 1;
        _inventory.Remove(runChip);
    }

    public static bool TryUpgradeInventory(IndexPath from, IndexPath to)
    {
        int fromIndex = from._ints[0];
        int toIndex = to._ints[0];

        if (!Instance._inventory.CanUpgradeInventory(fromIndex, toIndex)) return false;
        Instance._inventory.UpgradeInventory(fromIndex, toIndex);
        return true;
    }

    public static bool SwapAcquired(IndexPath from, IndexPath to)
    {
        Instance._danTian.SwapAcquired(from._ints[0], to._ints[0]);
        return true;
    }

    public static bool TryEquipNeiGong(IndexPath acquired, IndexPath neiGong)
    {
        int acquiredIndex = acquired._ints[0];
        int neiGongIndex = neiGong._ints[0];
        if(!Instance.CanEquipNeiGong(acquiredIndex, neiGongIndex)) return false;
        Instance.EquipNeiGong(acquiredIndex, neiGongIndex);
        return true;
    }

    private bool CanEquipNeiGong(int acquiredIndex, int neiGongIndex)
    {
        RunChip runChip = _danTian.TryGetAcquiredChip(acquiredIndex);
        return runChip._entry.GetType() == typeof(NeigongEntry);
    }

    private void EquipNeiGong(int acquiredIndex, int neiGongIndex)
    {
        Tile t = _danTian.TryGetAcquiredTile(acquiredIndex);
        _hero.TryRemoveNeiGongTile(t);
        _hero.EquipNeiGong(t, neiGongIndex);
    }

    public static bool TryEquipWaiGong(IndexPath acquired, IndexPath waiGong)
    {
        int acquiredIndex = acquired._ints[0];
        int waiGongIndex = waiGong._ints[0];
        if(!Instance.CanEquipWaiGong(acquiredIndex, waiGongIndex)) return false;
        Instance.EquipWaiGong(acquiredIndex, waiGongIndex);
        return true;
    }

    private bool CanEquipWaiGong(int acquiredIndex, int waiGongIndex)
    {
        RunChip runChip = _danTian.TryGetAcquiredChip(acquiredIndex);
        return runChip._entry.GetType() == typeof(WaigongEntry);
    }

    private void EquipWaiGong(int acquiredIndex, int waiGongIndex)
    {
        Tile t = _danTian.TryGetAcquiredTile(acquiredIndex);
        _hero.TryRemoveWaiGongTile(t);
        _hero.EquipWaiGong(t, waiGongIndex);
    }

    public static void SwapNeiGong(IndexPath from, IndexPath to)
    {
        Instance._hero.SwapNeiGong(from._ints[0], to._ints[0]);
    }

    public static void SwapWaiGong(IndexPath from, IndexPath to)
    {
        Instance._hero.SwapWaiGong(from._ints[0], to._ints[0]);
    }

    public void RefreshChip()
    {
        _inventory.RefreshChip();
    }

    public void UpgradeFirstChip()
    {
        _inventory.UpgradeFirstChip();
    }
}
