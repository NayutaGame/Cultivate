
using System;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public static readonly int NeiGongLimit = 4;
    public static readonly int WaiGongLimit = 12;

    private Status _status;
    private DanTian _danTian;
    private Inventory _inventory;
    private Hero _hero;
    private Enemy _enemy;

    private Dictionary<string, Func<IndexPath, object>> _funcList;

    public override void DidAwake()
    {
        base.DidAwake();

        _status = new Status();
        _danTian = new DanTian();
        _inventory = new Inventory();
        _inventory.Add(new RunChip("冰心决"));
        _inventory.Add(new RunChip("飞云劲", 2));
        _inventory.Add(new RunChip("紫微印", 3));
        _inventory.Add(new RunChip("善水印", 4));
        _inventory.Add(new RunChip("上清印", 5));
        _inventory.Add(new RunChip("火铃印", 6));
        _inventory.Add(new RunChip("三山印", 7));
        _inventory.Add(new RunChip("上清印", 5));

        _hero = new Hero();
        _enemy = new Enemy(new RunChip[]
        {
            new ("冰心决"),
            null,
            null,
            null,
        },
        new RunChip[]
        {
            new ("紫微印", 3),
            new ("紫微印", 3),
            new ("善水印", 4),
            new ("上清印", 5),
            new ("火铃印", 6),
            new ("三山印", 7),
            new ("上清印", 3),
            new ("上清印", 2),
            new ("上清印", 2),
            new ("上清印", 3),
            null,
            null,
        });

        _funcList = new()
        {
            { "GetTileXY",            GetTileXY       },
            { "TryGetRunChip",        TryGetRunChip   },
            { "GetRunChipCount",      GetRunChipCount },
            { "GetStatusString",      GetStatusString },
            { "GetAcquiredChipCount", GetAcquiredChipCount },
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
    private object GetRunChipCount(IndexPath indexPath) => _inventory.GetRunChipCount();
    private object GetStatusString(IndexPath indexPath) => $"命元：{_status.MingYuan}\n悟性：{_status.WuXing}\n修为：{_status.XiuWei}\n气血：{_status.Health}\n初始灵力：{_status.Mana}";
    private object GetAcquiredChipCount(IndexPath indexPath) => _danTian.GetAcquiredChipCount();
    private object TryGetAcquiredChip(IndexPath indexPath) => _danTian.TryGetAcquiredChip(indexPath._ints[0]);
    private object GetHeroNeiGong(IndexPath indexPath) => _hero.GetNeiGong(indexPath._ints[0]);
    private object GetHeroWaiGong(IndexPath indexPath) => _hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyNeiGong(IndexPath indexPath) => _enemy.GetNeiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => _enemy.GetWaiGong(indexPath._ints[0]);

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
}
