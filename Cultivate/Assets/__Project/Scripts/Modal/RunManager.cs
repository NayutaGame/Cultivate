
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

    public static readonly int[] NeiGongLimitFromJingJie = new[] { 0, 1, 2, 3, 4, 4 };
    public static readonly int[] WaiGongLimitFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };

    private ChipPool _chipPool;
    private DanTian _danTian;
    private ProductInventory _productInventory;
    private ChipInventory _chipInventory;
    private RunHero _hero;
    public RunHero Hero => _hero;
    private RunEnemy _enemy;
    public RunEnemy Enemy => _enemy;

    private JingJie _jingJie;

    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;
            _hero.SetJingJie(_jingJie);
            _danTian.SetJingJie(_jingJie);
            _enemy.SetJingJie(_jingJie);
        }
    }

    private int _mingYuan;

    // 灵根
    // 神识
    // 遁速
    // 心境

    public Modifier Modifier;

    private int _turn;
    public int Turn => _turn;

    private float _xiuWei;
    public float XiuWei => _xiuWei;

    private float _chanNeng;
    public float ChanNeng => _chanNeng;

    public float TurnXiuWei => Modifier.Value.ForceGet("turnXiuWeiAdd") * (1 + Modifier.Value.ForceGet("turnXiuWeiMul"));

    public float TurnChanNeng => Modifier.Value.ForceGet("turnChanNengAdd") * (1 + Modifier.Value.ForceGet("turnChanNengMul"));

    private Dictionary<string, Func<IndexPath, object>> _accessors;

    public override void DidAwake()
    {
        base.DidAwake();

        _chipPool = new ChipPool();
        _danTian = new DanTian();
        _productInventory = new ProductInventory();
        _chipInventory = new ChipInventory();

        _hero = new RunHero();
        _enemy = new RunEnemy();

        _accessors = new()
        {
            { "GetTileXY",            GetTileXY },
            { "TryGetProduct",        TryGetProduct },
            { "TryGetRunChip",        TryGetRunChip },
            { "GetHeroWaiGong",       GetHeroWaiGong },
            { "GetEnemyWaiGong",      GetEnemyWaiGong },
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;
        Modifier.AddChild(_danTian.Modifier);

        JingJie = JingJie.LianQi;
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._accessors[indexPath._str](indexPath);
    private object GetTileXY(IndexPath indexPath) => _danTian.GetTileXY(indexPath._ints[0], indexPath._ints[1]);
    private object TryGetProduct(IndexPath indexPath) => _productInventory[indexPath._ints[0]];
    private object TryGetRunChip(IndexPath indexPath) => _chipInventory.TryGetRunChip(indexPath._ints[0]);
    private object GetHeroWaiGong(IndexPath indexPath) => _hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => _enemy.GetWaiGong(indexPath._ints[0]);

    public int GetRunChipCount() => _chipInventory.GetRunChipCount();
    public int GetProductCount() => _productInventory.Count;
    public string GetStatusString() => $"命元：{_mingYuan}\n气血：{_hero.Health}\n初始灵力：{_hero.Mana}";

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
        _danTian.AutoAssignWorkers();
    }

    public bool TryClickProduct(IndexPath clicked)
    {
        if (!CanClickProduct(clicked))
            return false;
        ClickProduct(clicked);
        return true;
    }

    public bool CanClickProduct(IndexPath clicked)
    {
        Product product = Get<Product>(clicked);
        if (product == null) return false;

        return _productInventory.CanClick(product);
    }

    public void ClickProduct(IndexPath clicked)
    {
        Product product = Get<Product>(clicked);

        _chanNeng -= product.GetCost();
        _productInventory.Click(product);
        _danTian.AutoAssignWorkers();
    }

    public bool CanAfford(Product product)
    {
        if (product.GetCost() > ChanNeng) return false;

        if (product.IsDrag())
            return null != _danTian.Revealed().FirstObj(product.CanDrop);

        if (product.IsClick())
            return product.CanClick();

        return false;
    }

    public bool TryToggleWorkerLock(IndexPath pos)
    {
        Tile tile = Get<Tile>(pos);
        return _danTian.TryToggleWorkerLock(tile);
    }

    public bool TryDrag(IndexPath from, IndexPath to)
    {
        bool fromInventory = from._str == "TryGetRunChip";
        bool toInventory = to._str == "TryGetRunChip";
        bool fromHero = from._str == "GetHeroWaiGong";
        bool toHero = to._str == "GetHeroWaiGong";

        if (fromInventory && toInventory)
        {
            _chipInventory.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromHero && toHero)
        {
            _hero.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromInventory && toHero)
        {
            RunChip toEquip = _chipInventory[from._ints[0]];
            RunChip toUnequip = _hero.GetWaiGong(to._ints[0]);

            if (toUnequip == null)
            {
                _hero.SetWaiGong(to._ints[0], toEquip);
                _chipInventory.RemoveAt(from._ints[0]);
            }
            else
            {
                _hero.SetWaiGong(to._ints[0], toEquip);
                _chipInventory[from._ints[0]] = toUnequip;
            }

            return true;
        }
        else if (fromHero && toInventory)
        {
            RunChip toEquip = _chipInventory[to._ints[0]];
            RunChip toUnequip = _hero.GetWaiGong(from._ints[0]);

            if (toUnequip == null)
                return false;

            _hero.SetWaiGong(from._ints[0], toEquip);
            _chipInventory[to._ints[0]] = toUnequip;
            return true;
        }

        return false;
    }

    public bool Unequip(IndexPath from)
    {
        RunChip toUnequip = _hero.GetWaiGong(from._ints[0]);
        if (toUnequip == null)
            return false;

        _hero.SetWaiGong(from._ints[0], null);
        _chipInventory.Add(toUnequip);
        return true;
    }

    public bool TryWrite(IndexPath from, IndexPath to)
    {
        RunChip toWrite = Get<RunChip>(from);
        _enemy.SetWaiGong(to._ints[0], toWrite?.Clone());
        return true;
    }

    public void DrawChip()
    {
        if(_chipPool.TryPopFirst(_jingJie, out ChipEntry chipEntry))
            _chipInventory.Add(new RunChip(chipEntry));
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

    public void RefreshChip()
    {
        _chipInventory.RefreshChip();
    }

    public void UpgradeFirstChip()
    {
        _chipInventory.UpgradeFirstChip();
    }






















    // public bool TryUpgradeDanTian(IndexPath from, IndexPath to)
    // {
    //     RunChip runChip = (RunChip) Instance.TryGetRunChip(from);
    //     Tile tile = (Tile) Instance.GetTileXY(to);
    //
    //     if (!Instance.CanUpgradeDanTian(runChip, tile)) return false;
    //     Instance.UpgradeDanTian(runChip, tile);
    //     return true;
    // }
    //
    // public bool CanUpgradeDanTian(RunChip runChip, Tile tile)
    // {
    //     return RunChip.CanUpgrade(runChip, tile.Chip);
    // }
    //
    // public void UpgradeDanTian(RunChip runChip, Tile tile)
    // {
    //     tile.Chip.Upgrade();
    //     _chipInventory.Remove(runChip);
    // }
    //
    // public bool TryUpgradeInventory(IndexPath from, IndexPath to)
    // {
    //     int fromIndex = from._ints[0];
    //     int toIndex = to._ints[0];
    //
    //     if (!Instance._chipInventory.CanUpgradeInventory(fromIndex, toIndex)) return false;
    //     Instance._chipInventory.UpgradeInventory(fromIndex, toIndex);
    //     return true;
    // }
}
