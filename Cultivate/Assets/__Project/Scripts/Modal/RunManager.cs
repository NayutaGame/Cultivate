
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

    public static readonly int[] WaiGongStartFromJingJie = new[] { 9, 6, 4, 2, 0, 0 };

    private ChipPool _chipPool;
    public DanTian DanTian { get; private set; }
    private ProductInventory _productInventory;
    public ChipInventory ChipInventory { get; private set; }
    public AcquiredWaiGongInventory AcquiredInventory { get; private set; }
    public RunHero Hero { get; private set; }
    public EnemyPool EnemyPool { get; private set; }
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
        AcquiredInventory = new();

        Hero = new RunHero();
        EnemyPool = new ();
        NextEnemyFromPool();

        _accessors = new()
        {
            { "GetTileXY",             GetTileXY },
            // { "TryGetProduct",         TryGetProduct },
            { "TryGetRunChip",         TryGetRunChip },
            { "TryGetAcquired",        TryGetAcquired },
            { "GetHeroSlot",           GetHeroSlot },
            { "GetEnemySlot",          GetEnemySlot },
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
    private object TryGetAcquired(IndexPath indexPath) => AcquiredInventory.TryGet(indexPath._ints[0]);
    private object GetHeroSlot(IndexPath indexPath) => Hero.HeroSlotInventory[indexPath._ints[0]];
    private object GetEnemySlot(IndexPath indexPath) => Enemy.GetSlot(indexPath._ints[0]);

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
        bool fromAcquired = from._str == "TryGetAcquired";
        bool toAcquired = to._str == "TryGetAcquired";
        bool fromHero = from._str == "GetHeroSlot";
        bool toHero = to._str == "GetHeroSlot";

        if (fromAcquired && toAcquired)
        {
            AcquiredInventory.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromHero && toHero)
        {
            Hero.HeroSlotInventory.Swap(from._ints[0], to._ints[0]);
            return true;
        }
        else if (fromAcquired && toHero)
        {
            AcquiredRunChip toEquip = AcquiredInventory[from._ints[0]];
            HeroChipSlot slot = Hero.HeroSlotInventory[to._ints[0]];

            if (slot.AcquiredRunChip != null)
            {
                AcquiredInventory[from._ints[0]] = slot.AcquiredRunChip;
            }
            else
            {
                AcquiredInventory.RemoveAt(from._ints[0]);
            }

            slot.AcquiredRunChip = toEquip;

            return true;
        }
        else if (fromHero && toAcquired)
        {
            HeroChipSlot slot = Hero.HeroSlotInventory[from._ints[0]];
            AcquiredRunChip toUnequip = slot.AcquiredRunChip;
            if (toUnequip == null)
                return false;

            slot.AcquiredRunChip = AcquiredInventory[to._ints[0]];
            AcquiredInventory[to._ints[0]] = toUnequip;
            return true;
        }

        return false;
    }

    public bool AcquiredWaiGongMoveToEnd(IndexPath indexPath)
    {
        AcquiredInventory.MoveToEnd(indexPath._ints[0]);
        return true;
    }

    public bool TryWrite(IndexPath from, IndexPath to)
    {
        object o = Get<object>(from);
        if (o is AcquiredRunChip toWrite)
        {
            int[] powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = toWrite.GetPower(wuXing));

            Enemy.SetSlotContent(to._ints[0], toWrite.Chip.Clone(), powers);
        }
        else if (o is HeroChipSlot slot)
        {
            int[] powers = new int[WuXing.Length];
            WuXing.Traversal.Do(wuXing => powers[wuXing] = slot.GetPower(wuXing));

            Enemy.SetSlotContent(to._ints[0], slot.RunChip?.Clone(), powers);
        }
        else
        {
            int[] powers = new int[WuXing.Length];
            Enemy.SetSlotContent(to._ints[0], null, powers);
        }

        return true;
    }

    public bool Unequip(IndexPath indexPath)
    {
        HeroChipSlot slot = Hero.HeroSlotInventory[indexPath._ints[0]];
        AcquiredRunChip toUnequip = slot.AcquiredRunChip;
        if (toUnequip == null)
            return false;

        slot.AcquiredRunChip = null;
        AcquiredInventory.Add(toUnequip);
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

    public bool NextEnemyFromPool()
    {
        if(EnemyPool.Count() == 0)
            EnemyPool.Populate();

        bool success = EnemyPool.TryPopFirst(out RunEnemy runEnemy);
        if (success)
        {
            Enemy = runEnemy;
        }
        else
        {
            Enemy = new RunEnemy();
        }
        return success;
    }
}
