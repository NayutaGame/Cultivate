
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
    public DanTian DanTian { get; private set; }
    private ProductInventory _productInventory;
    public ChipInventory ChipInventory { get; private set; }
    public AcquiredWaiGongInventory AcquiredWaiGongInventory { get; private set; }
    public AcquiredSlotInventory AcquiredSlotInventory { get; private set; }
    public RunHero Hero { get; private set; }
    public RunEnemy Enemy { get; private set; }

    private JingJie _jingJie;

    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;
            Hero.SetJingJie(_jingJie);
            DanTian.SetJingJie(_jingJie);
            Enemy.SetJingJie(_jingJie);
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
        DanTian = new DanTian();
        _productInventory = new ProductInventory();
        ChipInventory = new ChipInventory();
        AcquiredWaiGongInventory = new();
        AcquiredSlotInventory = new();

        Hero = new RunHero();
        Enemy = new RunEnemy();

        _accessors = new()
        {
            { "GetTileXY",             GetTileXY },
            // { "TryGetProduct",         TryGetProduct },
            { "TryGetRunChip",         TryGetRunChip },
            { "TryGetAcquiredWaiGong", TryGetAcquiredWaiGong },
            { "GetAcquiredSlot",       GetAcquiredSlot },
            { "GetHeroWaiGong",        GetHeroWaiGong },
            { "GetEnemyWaiGong",       GetEnemyWaiGong },
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;
        Modifier.AddChild(DanTian.Modifier);

        JingJie = JingJie.LianQi;
    }

    public static T Get<T>(IndexPath indexPath) => (T) Instance._accessors[indexPath._str](indexPath);
    private object GetTileXY(IndexPath indexPath) => DanTian.GetTileXY(indexPath._ints[0], indexPath._ints[1]);
    // private object TryGetProduct(IndexPath indexPath) => _productInventory[indexPath._ints[0]];
    private object TryGetRunChip(IndexPath indexPath) => ChipInventory.TryGet(indexPath._ints[0]);
    private object TryGetAcquiredWaiGong(IndexPath indexPath) => AcquiredWaiGongInventory.TryGet(indexPath._ints[0]);
    private object GetAcquiredSlot(IndexPath indexPath) => AcquiredSlotInventory[indexPath._ints[0]];
    private object GetHeroWaiGong(IndexPath indexPath) => Hero.GetWaiGong(indexPath._ints[0]);
    private object GetEnemyWaiGong(IndexPath indexPath) => Enemy.GetWaiGong(indexPath._ints[0]);

    public int GetRunChipCount() => ChipInventory.Count;
    public int GetProductCount() => _productInventory.Count;
    public string GetStatusString() => $"命元：{_mingYuan}\n气血：{Hero.Health}\n初始灵力：{Hero.Mana}";

    // public bool TryDropProduct(IndexPath from, IndexPath to)
    // {
    //     if (!CanDropProduct(from, to))
    //         return false;
    //     DropProduct(from, to);
    //     return true;
    // }
    //
    // public bool CanDropProduct(IndexPath from, IndexPath to)
    // {
    //     Product product = Get<Product>(from);
    //     Tile tile = Get<Tile>(to);
    //     if (product == null || tile == null) return false;
    //
    //     return _productInventory.CanDrop(product, tile);
    // }
    //
    // public void DropProduct(IndexPath from, IndexPath to)
    // {
    //     Product product = Get<Product>(from);
    //     Tile tile = Get<Tile>(to);
    //
    //     _chanNeng -= product.GetCost();
    //     _productInventory.Drop(product, tile);
    //     // _danTian.AutoAssignWorkers();
    // }
    //
    // public bool TryClickProduct(IndexPath clicked)
    // {
    //     if (!CanClickProduct(clicked))
    //         return false;
    //     ClickProduct(clicked);
    //     return true;
    // }
    //
    // public bool CanClickProduct(IndexPath clicked)
    // {
    //     Product product = Get<Product>(clicked);
    //     if (product == null) return false;
    //
    //     return _productInventory.CanClick(product);
    // }
    //
    // public void ClickProduct(IndexPath clicked)
    // {
    //     Product product = Get<Product>(clicked);
    //
    //     _chanNeng -= product.GetCost();
    //     _productInventory.Click(product);
    //     // _danTian.AutoAssignWorkers();
    // }
    //
    // public bool CanAfford(Product product)
    // {
    //     if (product.GetCost() > ChanNeng) return false;
    //
    //     if (product.IsDrag())
    //         return null != _danTian.Revealed().FirstObj(product.CanDrop);
    //
    //     if (product.IsClick())
    //         return product.CanClick();
    //
    //     return false;
    // }
    //
    // public bool TryToggleWorkerLock(IndexPath pos)
    // {
    //     Tile tile = Get<Tile>(pos);
    //     return _danTian.TryToggleWorkerLock(tile);
    // }
    //
    // public bool TryDrag(IndexPath from, IndexPath to)
    // {
    //     bool fromInventory = from._str == "TryGetRunChip";
    //     bool toInventory = to._str == "TryGetRunChip";
    //     bool fromHero = from._str == "GetHeroWaiGong";
    //     bool toHero = to._str == "GetHeroWaiGong";
    //
    //     if (fromInventory && toInventory)
    //     {
    //         _chipInventory.Swap(from._ints[0], to._ints[0]);
    //         return true;
    //     }
    //     else if (fromHero && toHero)
    //     {
    //         _hero.Swap(from._ints[0], to._ints[0]);
    //         return true;
    //     }
    //     else if (fromInventory && toHero)
    //     {
    //         RunChip toEquip = _chipInventory[from._ints[0]];
    //         RunChip toUnequip = _hero.GetWaiGong(to._ints[0]);
    //
    //         if (toUnequip == null)
    //         {
    //             _hero.SetWaiGong(to._ints[0], toEquip);
    //             _chipInventory.RemoveAt(from._ints[0]);
    //         }
    //         else
    //         {
    //             _hero.SetWaiGong(to._ints[0], toEquip);
    //             _chipInventory[from._ints[0]] = toUnequip;
    //         }
    //
    //         return true;
    //     }
    //     else if (fromHero && toInventory)
    //     {
    //         RunChip toEquip = _chipInventory[to._ints[0]];
    //         RunChip toUnequip = _hero.GetWaiGong(from._ints[0]);
    //
    //         if (toUnequip == null)
    //             return false;
    //
    //         _hero.SetWaiGong(from._ints[0], toEquip);
    //         _chipInventory[to._ints[0]] = toUnequip;
    //         return true;
    //     }
    //
    //     return false;
    // }

    public bool InventorySwap(IndexPath from, IndexPath to)
    {
        ChipInventory.Swap(from._ints[0], to._ints[0]);
        return true;
    }

    public bool InventoryMoveToEnd(IndexPath indexPath)
    {
        ChipInventory.MoveToEnd(indexPath._ints[0]);
        return true;
    }

    public bool TryDrag(IndexPath from, IndexPath to)
    {
        bool fromAcquired = from._str == "TryGetAcquiredWaiGong";
        bool toAcquired = to._str == "TryGetAcquiredWaiGong";
        bool fromHero = from._str == "GetHeroWaiGong";
        bool toHero = to._str == "GetHeroWaiGong";

        if (fromAcquired && toAcquired)
        {
            AcquiredWaiGongInventory.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromHero && toHero)
        {
            Hero.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromAcquired && toHero)
        {
            AcquiredRunChip acquiredRunChip = AcquiredWaiGongInventory.TryGet(from._ints[0]);
            HeroRunChip toEquip = new HeroRunChip(to._ints[0], acquiredRunChip);
            HeroRunChip toUnequip = Hero.GetWaiGong(to._ints[0]);

            if (toUnequip == null)
            {
                Hero.SetWaiGong(to._ints[0], toEquip);
                AcquiredWaiGongInventory.RemoveAt(from._ints[0]);
            }
            else
            {
                Hero.SetWaiGong(to._ints[0], toEquip);
                AcquiredWaiGongInventory[from._ints[0]] = toUnequip.AcquiredRunChip;
            }

            return true;
        }
        else if (fromHero && toAcquired)
        {
            AcquiredRunChip acquiredRunChip = AcquiredWaiGongInventory.TryGet(to._ints[0]);
            HeroRunChip toEquip = new HeroRunChip(from._ints[0], acquiredRunChip);
            HeroRunChip toUnequip = Hero.GetWaiGong(from._ints[0]);

            if (toUnequip == null)
                return false;

            Hero.SetWaiGong(from._ints[0], toEquip);
            AcquiredWaiGongInventory[to._ints[0]] = toUnequip.AcquiredRunChip;
            return true;
        }

        return false;
    }

    public bool AcquiredWaiGongMoveToEnd(IndexPath indexPath)
    {
        AcquiredWaiGongInventory.MoveToEnd(indexPath._ints[0]);
        return true;
    }

    public bool TryWrite(IndexPath from, IndexPath to)
    {
        AcquiredRunChip toWrite = Get<AcquiredRunChip>(from);
        Enemy.SetWaiGong(to._ints[0], toWrite?.Chip.Clone());
        return true;
    }

    public bool Unequip(IndexPath indexPath)
    {
        HeroRunChip toUnequip = Hero.GetWaiGong(indexPath._ints[0]);

        if (toUnequip == null)
            return false;

        Hero.SetWaiGong(indexPath._ints[0], null);
        AcquiredWaiGongInventory.Add(toUnequip.AcquiredRunChip);
        return true;
    }

    public void DrawChip()
    {
        if(_chipPool.TryPopFirst(_jingJie, out ChipEntry chipEntry))
            ChipInventory.Add(new RunChip(chipEntry));
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
        ChipInventory.RefreshChip();
    }

    public void UpgradeFirstChip()
    {
        ChipInventory.UpgradeFirstChip();
    }

    public void ClearChip()
    {
        ChipInventory.Clear();
    }






















    public bool TryUpgradeDanTian(IndexPath from, IndexPath to)
    {
        RunChip runChip = Get<RunChip>(from);
        Tile tile = Get<Tile>(to);

        if (!CanUpgradeDanTian(runChip, tile)) return false;
        UpgradeDanTian(runChip, tile);
        return true;
    }

    public bool CanUpgradeDanTian(RunChip runChip, Tile tile)
    {
        return RunChip.CanUpgrade(runChip, tile.AcquiredRunChip?.Chip);
    }

    public void UpgradeDanTian(RunChip runChip, Tile tile)
    {
        tile.AcquiredRunChip.Upgrade();
        ChipInventory.Remove(runChip);
    }

    public bool TryPlug(IndexPath from, IndexPath to)
    {
        RunChip runChip = Get<RunChip>(from);
        Tile tile = Get<Tile>(to);

        if (!runChip._entry.CanPlug(tile, runChip)) return false;
        runChip._entry.Plug(tile, runChip);
        return true;
    }

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
