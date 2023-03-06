
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>
{
    public static readonly int NeiGongLimit = 4;
    public static readonly int WaiGongLimit = 12;

    private DanTian _danTian;
    public DanTian DanTian => _danTian;
    private ProductInventory _productInventory;
    public ProductInventory ProductInventory => _productInventory;
    private Inventory _inventory;
    private RunHero _hero;
    public RunHero Hero => _hero;
    private RunEnemy _enemy;
    public RunEnemy Enemy => _enemy;

    private int _mingYuan;
    public int MingYuan => _mingYuan;

    private int _turn;
    public int Turn => _turn;

    private float _xiuWei;
    public float XiuWei => _xiuWei;

    private float _chanNeng;
    public float ChanNeng => _chanNeng;

    public Modifier Modifier;

    public float TurnXiuWei => Modifier.Value.ForceGet("turnXiuWeiAdd") * (1 + Modifier.Value.ForceGet("turnXiuWeiMul"));
    public float TurnChanNeng => Modifier.Value.ForceGet("turnChanNengAdd") * (1 + Modifier.Value.ForceGet("turnChanNengMul"));

    private Dictionary<string, Func<IndexPath, object>> _funcList;

    public override void DidAwake()
    {
        base.DidAwake();

        _danTian = new DanTian();
        _productInventory = new ProductInventory();
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
            { "GetTileXY",            GetTileXY },
            { "TryGetRunChip",        TryGetRunChip },
            { "TryGetUnequipped",     TryGetUnequipped },
            { "GetHeroNeiGong",       GetHeroNeiGong },
            { "GetHeroWaiGong",       GetHeroWaiGong },
            { "GetEnemyNeiGong",      GetEnemyNeiGong },
            { "GetEnemyWaiGong",      GetEnemyWaiGong },
            { "TryGetProduct",        TryGetProduct },
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;
        Modifier.AddChild(_danTian.Modifier);

        SetJingJie(JingJie.LianQi);
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._funcList[indexPath._str](indexPath);
    public static T Get<T>(string funcName) => Get<T>(new IndexPath(funcName));
    private object GetTileXY(IndexPath indexPath) => _danTian.GetTileXY(indexPath._ints[0], indexPath._ints[1]);
    private object TryGetRunChip(IndexPath indexPath) => _inventory.TryGetRunChip(indexPath._ints[0]);
    private object TryGetUnequipped(IndexPath indexPath) => _hero.TryGetUnequipped(indexPath._ints[0]);
    private object GetHeroNeiGong(IndexPath indexPath) => _hero.GetNeiGong(indexPath._ints[0]);
    private object GetHeroWaiGong(IndexPath indexPath) => _hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyNeiGong(IndexPath indexPath) => _enemy.GetNeiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => _enemy.GetWaiGong(indexPath._ints[0]);
    private object TryGetProduct(IndexPath indexPath) => _productInventory[indexPath._ints[0]];

    public int GetRunChipCount() => _inventory.GetRunChipCount();
    public int GetUnequippedCount() => _hero.UnequippedCount;
    public int GetProductCount() => _productInventory.Count;
    public string GetStatusString() => $"命元：{MingYuan}\n气血：{_hero.Health}\n初始灵力：{_hero.Mana}";

    public bool TryDropProduct(IndexPath from, IndexPath to)
    {
        if (!CanDropProduct(from, to))
            return false;
        DropProduct(from, to);
        return true;
    }

    public bool CanDropProduct(IndexPath from, IndexPath to)
    {
        Product product = Get<Product>(from);
        Tile tile = Get<Tile>(to);
        if (product == null || tile == null) return false;

        return _productInventory.CanDrop(product, tile);
    }

    public void DropProduct(IndexPath from, IndexPath to)
    {
        Product product = Get<Product>(from);
        Tile tile = Get<Tile>(to);

        _chanNeng -= product.GetCost();
        _productInventory.Drop(product, tile);
    }

    public bool CanAfford(Product product)
    {
        if (product.GetCost() > ChanNeng) return false;
        return null != _danTian.Revealed().FirstObj(product.CanDrop);
    }

    public bool TryToggleWorkerLock(IndexPath pos)
    {
        Tile tile = Get<Tile>(pos);
        return _danTian.TryToggleWorkerLock(tile);
    }

    public void AddTurn()
    {
        _turn += 1;
        _xiuWei += TurnXiuWei;
        _chanNeng += TurnChanNeng;
    }

    public void AddXiuWei()
    {
        _xiuWei += 10;
    }

    public void AddChanNeng()
    {
        _chanNeng += 10;
    }



















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
        if (!runChip.IsNeiGong && !runChip.IsWaiGong) return false;
        return true;
    }

    public void Apply(RunChip runChip, Tile tile)
    {
        AcquiredChip newAcquiredChip = new AcquiredChip(runChip, tile);
        tile.Chip = newAcquiredChip;
        _hero.AddAcquiredChip(newAcquiredChip);

        _inventory.Remove(runChip);
    }

    public bool TryXiuLian(IndexPath from, IndexPath to)
    {
        RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
        Tile tile = (Tile) Instance.GetTileXY(to);

        if (!Instance.CanXiuLian(runChip, tile)) return false;
        Instance.XiuLian(runChip, tile);
        return true;
    }

    public bool CanXiuLian(RunChip runChip, Tile tile)
    {
        if (tile.Chip != null) return false;

        if(runChip._entry is XiulianEntry xiuLian)
        {
            return xiuLian.CanApply(tile);
        }
        else
        {
            return false;
        }
    }

    public void XiuLian(RunChip runChip, Tile tile)
    {
        XiulianEntry xiuLian = runChip._entry as XiulianEntry;
        xiuLian.Apply(tile);

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
        tile.Chip.Upgrade();
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

    public void SetJingJie(JingJie jingJie)
    {
        _hero.JingJie = jingJie;
        _danTian.SetJingJie(_hero.JingJie);
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
