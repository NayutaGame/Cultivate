
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;

public class RunManager : Singleton<RunManager>, GDictionary
{
    public static readonly int NeiGongLimit = 4;
    public static readonly int WaiGongLimit = 12;

    public static readonly int[] NeiGongLimitFromJingJie = new[] { 0, 1, 2, 3, 4, 4 };
    public static readonly int[] WaiGongLimitFromJingJie = new[] { 3, 6, 8, 10, 12, 12 };

    public static readonly int[] WaiGongStartFromJingJie = new[] { 9, 6, 4, 2, 0, 0 };

    public static readonly float EUREKA_DISCOUNT_RATE = 0.5f;

    public event Action<StageCommitDetails> StageCommitEvent;
    public void StageCommit(StageCommitDetails d) => StageCommitEvent?.Invoke(d);

    public event Action<AcquireDetails> AcquireEvent;
    public void Acquire(AcquireDetails d) => AcquireEvent?.Invoke(d);

    public event Action<BuildDetails> BuildEvent;
    public void Build(BuildDetails d) => BuildEvent?.Invoke(d);

    public event Action<PowerChangedDetails> PowerChangedEvent;
    public void PowerChanged(PowerChangedDetails d) => PowerChangedEvent?.Invoke(d);

    public event Action<StatusChangedDetails> StatusChangedEvent;
    public void StatusChanged(StatusChangedDetails d) => StatusChangedEvent?.Invoke(d);

    public event Action StageEnvironmentChangedEvent;
    public void StageEnvironmentChanged() => StageEnvironmentChangedEvent?.Invoke();

    private ChipPool _chipPool;
    public DanTian DanTian { get; private set; }
    private ProductInventory _productInventory;
    public TechInventory TechInventory { get; private set; }
    public Map Map { get; private set; }
    public ChipInventory ChipInventory { get; private set; }
    public AcquiredWaiGongInventory AcquiredInventory { get; private set; }
    public RunHero Hero { get; private set; }
    public EnemyPool EnemyPool { get; private set; }

    private RunEnemy _enemy;
    public RunEnemy Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            StageEnvironmentChanged();
        }
    }

    public ArenaWaiGongInventory ArenaWaiGongInventory;
    public Arena Arena;

    private JingJie _jingJie;

    public JingJie JingJie
    {
        get => _jingJie;
        set
        {
            _jingJie = value;
            Hero.SetJingJie(_jingJie);
            DanTian.SetJingJie(_jingJie);
            Map.SetJingJie(_jingJie);
        }
    }

    private int _mingYuan;
    public int MingYuan
    {
        get => _mingYuan;
        set => _mingYuan = value;
    }

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

    public bool IsStream { get; private set; }
    public StageReport Report;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public static T Get<T>(IndexPath indexPath)
    {
        object curr = Instance;
        foreach (string key in indexPath.Values)
        {
            if (int.TryParse(key, out int i))
            {
                IList l = curr as IList;
                if (l.Count <= i)
                    return default;
                curr = l[i];
            }
            else
            {
                curr = (curr as GDictionary).GetAccessors()[key]();
            }
        }

        if (curr is T ret)
            return ret;
        else
            return default;
    }

    public override void DidAwake()
    {
        base.DidAwake();

        _chipPool = new();
        DanTian = new();
        _productInventory = new();
        TechInventory = new();
        Map = new();
        ChipInventory = new();
        AcquiredInventory = new();

        Hero = new();
        EnemyPool = new();

        CreateEnemyDetails d = new CreateEnemyDetails();
        Enemy = new RunEnemy(DrawEnemy(d), d);

        ArenaWaiGongInventory = new();
        Arena = new();

        _accessors = new()
        {
            { "DanTian",               () => DanTian },
            { "TechInventory",         () => TechInventory },
            { "Map",                   () => Map },
            { "ChipInventory",         () => ChipInventory },
            { "AcquiredInventory",     () => AcquiredInventory },
            { "Hero",                  () => Hero },
            { "Enemy",                 () => Enemy },
            { "ArenaWaiGongInventory", () => ArenaWaiGongInventory },
            { "Arena",                 () => Arena },
            // { "TryGetProduct",         TryGetProduct },
            // private object TryGetProduct(IndexPath indexPath) => _productInventory[indexPath._ints[0]];
        };

        _mingYuan = 100;
        _turn = 1;
        _xiuWei = 0;
        _chanNeng = 0;

        Modifier = Modifier.Default;
        Modifier.AddChild(DanTian.Modifier);

        StageEnvironmentChangedEvent += CalcReport;
        StageEnvironmentChangedEvent += CalcManaShortageBrief;

        DesignerEnvironment.EnterRun();
    }

    public RunNode TryGetCurrentNode() => Map.TryGetCurrentNode();
    public string GetStatusString() => $"命元：{_mingYuan}\n气血：{Hero.Health}\n初始灵力：{Hero.Mana}";

    public void AddTurn()
    {
        _turn += 1;
        _xiuWei += TurnXiuWei;
        _chanNeng += TurnChanNeng;
    }

    public void AddXiuWei(int xiuWei = 10)
    {
        _xiuWei += xiuWei;
    }

    public void AddChanNeng(int chanNeng = 10)
    {
        _chanNeng += chanNeng;
    }

    public void AddMingYuan(int mingYuan = 10)
    {
        _mingYuan += mingYuan;
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

    public bool CanAffordTech(IndexPath indexPath)
    {
        RunTech runTech = Get<RunTech>(indexPath);
        return runTech.GetCost() <= _xiuWei;
    }

    public bool TrySetDoneTech(IndexPath indexPath)
    {
        if (!CanAffordTech(indexPath))
            return false;

        RunTech runTech = Get<RunTech>(indexPath);
        _xiuWei -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
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

    // public bool TryUpgradeInventory(IndexPath from, IndexPath to)
    // {
    //     int fromIndex = from._ints[0];
    //     int toIndex = to._ints[0];
    //
    //     if (!Instance._chipInventory.CanUpgradeInventory(fromIndex, toIndex)) return false;
    //     Instance._chipInventory.UpgradeInventory(fromIndex, toIndex);
    //     return true;
    // }

    public bool TryDrawWaiGong()
    {
        _chipPool.Shuffle();
        ChipEntry chip = _chipPool.ForcePopItem(c => c is WaiGongEntry && c.JingJieRange.Contains(_jingJie));
        ChipInventory.Add(new RunChip(chip, chip.JingJieRange.Start));
        return true;
    }

    public bool TryDrawStone()
    {
        _chipPool.Shuffle();
        ChipEntry chip = _chipPool.ForcePopItem(c => c is WuXingChipEntry);
        ChipInventory.Add(new RunChip(chip, chip.JingJieRange.Start));
        return true;
    }

    public bool TryDrawAcquired(Predicate<ChipEntry> pred, JingJie jingJie)
    {
        _chipPool.Shuffle();
        ChipEntry chip = _chipPool.ForcePopItem(pred);
        RunChip draw = new RunChip(chip, jingJie);
        Tile emptyTile = DanTian.FirstEmptyTile();
        if (emptyTile == null)
            return false;

        return draw.TryPlug(emptyTile);
    }

    public bool TryDrawAcquired(JingJie jingJie)
    {
        _chipPool.Shuffle();
        ChipEntry chip = _chipPool.ForcePopItem(c => c is WaiGongEntry && c.JingJieRange.Contains(jingJie));
        RunChip draw = new RunChip(chip, jingJie);
        Tile emptyTile = DanTian.FirstEmptyTile();
        if (emptyTile == null)
            return false;

        return draw.TryPlug(emptyTile);
    }

    public RunChip DrawChip(string chipName)
    {
        ChipEntry chip = chipName;
        RunChip picked = new RunChip(chip, chip.JingJieRange.Start);
        ChipInventory.Add(picked);
        return picked;
    }

    public void DrawChip(Predicate<ChipEntry> pred, int count)
    {
        _chipPool.Shuffle();
        for (int i = 0; i < count; i++)
        {
            ChipEntry chip = _chipPool.ForcePopItem(pred);
            ChipInventory.Add(new RunChip(chip, chip.JingJieRange.Start));
        }
    }

    public EnemyEntry DrawEnemy(CreateEnemyDetails d)
    {
        EnemyPool.Shuffle();
        EnemyEntry entry = EnemyPool.ForcePopItem(e => e.CanCreate(d));
        return entry;
    }

    public bool TryClickNode(IndexPath indexPath)
    {
        RunNode runNode = Get<RunNode>(indexPath);
        if (runNode.State != RunNode.RunNodeState.ToChoose || !Map.Selecting)
            return false;

        Map.SelectedNode(runNode);
        return true;
    }

    private void CalcReport()
    {
        StageEntity[] entities = new StageEntity[]
        {
            new StageHero(Hero, 0),
            new StageEnemy(Enemy, 1),
        };

        Report = StageManager.SimulateBrief(entities);
    }

    [NonSerialized] public bool[] ManaShortageBrief;
    private void CalcManaShortageBrief() => ManaShortageBrief = StageManager.ManaSimulate();

    public void GenerateReport()
    {
        IsStream = false;
        AppManager.Push(new AppStageS());
    }

    public void Stream()
    {
        IsStream = true;
        AppManager.Push(new AppStageS());
    }

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
}
