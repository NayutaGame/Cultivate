
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
            { "TryGetUnequipped",     TryGetUnequipped },
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
    private object TryGetUnequipped(IndexPath indexPath) => _hero.TryGetUnequipped(indexPath._ints[0]);
    private object GetHeroNeiGong(IndexPath indexPath) => _hero.GetNeiGong(indexPath._ints[0]);
    private object GetHeroWaiGong(IndexPath indexPath) => _hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyNeiGong(IndexPath indexPath) => _enemy.GetNeiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => _enemy.GetWaiGong(indexPath._ints[0]);

    public int GetRunChipCount() => _inventory.GetRunChipCount();
    public int GetUnequippedCount() => _hero.UnequippedCount;

    public bool Swap(IndexPath from, IndexPath to)
    {
        _inventory.Swap(from._ints[0], to._ints[0]);
        return true;
    }

    public bool TryApply(IndexPath from, IndexPath to)
    {
        RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
        Tile tile = (Tile) Instance.GetTileXY(to);

        if (!Instance.CanApply(runChip, tile)) return false;
        Instance.Apply(runChip, tile);
        return true;
    }

    public bool CanApply(RunChip runChip, Tile tile)
    {
        if (tile.Chip != null) return false;
        // Mana Constraint
        return true;
    }

    public void Apply(RunChip runChip, Tile tile)
    {
        // 修炼case

        AcquiredChip newAcquiredChip = new AcquiredChip(runChip, tile);
        tile.Chip = newAcquiredChip;
        _hero.AddAcquiredChip(newAcquiredChip);

        _inventory.Remove(runChip);
    }

    public bool TryUpgradeDanTian(IndexPath from, IndexPath to)
    {
        RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
        Tile tile = (Tile) Instance.GetTileXY(to);

        if (!Instance.CanUpgradeDanTian(runChip, tile)) return false;
        Instance.UpgradeDanTian(runChip, tile);
        return true;
    }

    public bool CanUpgradeDanTian(RunChip runChip, Tile tile)
    {
        return RunChip.CanUpgrade(runChip, tile.Chip);
    }

    public void UpgradeDanTian(RunChip runChip, Tile tile)
    {
        tile.Chip.Level += 1;
        _inventory.Remove(runChip);
    }

    public bool TryUpgradeInventory(IndexPath from, IndexPath to)
    {
        int fromIndex = from._ints[0];
        int toIndex = to._ints[0];

        if (!Instance._inventory.CanUpgradeInventory(fromIndex, toIndex)) return false;
        Instance._inventory.UpgradeInventory(fromIndex, toIndex);
        return true;
    }







    public bool SwapUnequipped(IndexPath from, IndexPath to)
    {
        _hero.SwapUnequipped(from._ints[0], to._ints[0]);
        return true;
    }

    public bool TryEquipNeiGong(IndexPath unequipped, IndexPath neiGong)
    {
        _hero.TryEquipNeiGong(unequipped._ints[0], neiGong._ints[0]);
        return true;
    }

    public bool TryEquipWaiGong(IndexPath unequipped, IndexPath waiGong)
    {
        _hero.TryEquipWaiGong(unequipped._ints[0], waiGong._ints[0]);
        return true;
    }

    public void SwapNeiGong(IndexPath from, IndexPath to)
    {
        _hero.SwapNeiGong(from._ints[0], to._ints[0]);
    }

    public void SwapWaiGong(IndexPath from, IndexPath to)
    {
        _hero.SwapWaiGong(from._ints[0], to._ints[0]);
    }

    public void UnequipNeiGong(IndexPath neigong)
    {
        _hero.UnequipNeiGong(neigong._ints[0]);
    }

    public void UnequipWaiGong(IndexPath waiGong)
    {
        _hero.UnequipWaiGong(waiGong._ints[0]);
    }




    public bool TryCopy(IndexPath from, IndexPath to)
    {
        if (to._str == "GetEnemyNeiGong")
        {
            return TryCopyNeiGong(from, to);
        }
        else if (to._str == "GetEnemyWaiGong")
        {
            return TryCopyWaiGong(from, to);
        }

        return false;
    }

    public bool TryCopyNeiGong(IndexPath from, IndexPath to)
    {
        var c = Get<AcquiredChip>(from);
        if (c == null)
        {
            _enemy.SetNeiGong(to._ints[0], null);
            return true;
        }
        else if (c.IsNeiGong)
        {
            _enemy.SetNeiGong(to._ints[0], new RunChip(c._entry, c.Level));
            return true;
        }

        return false;
    }

    public bool TryCopyWaiGong(IndexPath from, IndexPath to)
    {
        var c = Get<AcquiredChip>(from);
        if (c == null)
        {
            _enemy.SetWaiGong(to._ints[0], null);
            return true;
        }
        else if (c.IsWaiGong)
        {
            _enemy.SetWaiGong(to._ints[0], new RunChip(c._entry, c.Level));
            return true;
        }

        return false;
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
