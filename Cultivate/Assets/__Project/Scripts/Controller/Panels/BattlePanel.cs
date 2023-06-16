
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class BattlePanel : Panel
{
    public EntityView EnemyView;
    public EntityView HeroView;
    public SkillInventoryView SkillInventoryView;

    public TMP_Text SimulatedHP;
    public Image Light;
    public Button ReportButton;
    public Button StreamButton;
    public TMP_Text ReportText;

    private InteractDelegate InteractDelegate;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Battle");

        ConfigureInteractDelegate();

        EnemyView.Configure(new IndexPath($"{_indexPath}.Enemy"));
        EnemyView.SetDelegate(InteractDelegate);

        HeroView.Configure(new IndexPath($"{_indexPath}.Hero"));
        HeroView.SetDelegate(InteractDelegate);

        SkillInventoryView.Configure(new IndexPath($"{_indexPath}.SkillInventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);

        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new(4,
            getId: view =>
            {
                object item = RunManager.Get<object>(view.GetIndexPath());
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot slot)
                {
                    RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(_indexPath);
                    if (runEnvironment.Hero == slot.Owner)
                        return 2;
                    if (runEnvironment.Enemy == slot.Owner)
                        return 3;
                }
                return null;
            },
            dragDropTable: new Func<IInteractable, IInteractable, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), SkillSlot(Enemy) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,        null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         null,
                /* SkillSlot(Enemy) */ null,       null,           null,            null,
            });
    }

    public override void Refresh()
    {
        EnemyView.Refresh();
        HeroView.Refresh();
        SkillInventoryView.Refresh();

        if (RunManager.Instance.Battle.Report is { } report)
        {
            SimulatedHP.text = $"玩家 : 怪物\n{report.HomeLeftHp} : {report.AwayLeftHp}";
            Light.color = report.HomeVictory ? Color.green : Color.red;
            ReportText.text = report.ToString();
        }
        else
        {
            SimulatedHP.text = $"玩家 : 怪物\n无结果";
            Light.color = Color.gray;
            ReportText.text = "";
        }
    }

    private void Report()
    {
        RunManager.Instance.Combat(false, RunManager.Instance.Battle);
        RunCanvas.Instance.Refresh();
    }

    private void Stream()
    {
        RunManager.Instance.Combat(true, RunManager.Instance.Battle);
        RunCanvas.Instance.Refresh();
    }

    private bool TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(_indexPath);
        RunSkill lhs = RunManager.Get<RunSkill>(from.GetIndexPath());
        RunSkill rhs = RunManager.Get<RunSkill>(to.GetIndexPath());
        return runEnvironment.TryMerge(lhs, rhs);
    }

    private bool TryEquip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(_indexPath);
        RunSkill toEquip = RunManager.Get<RunSkill>(from.GetIndexPath());
        SkillSlot slot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TryEquip(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(_indexPath);
        SkillSlot slot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        return runEnvironment.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = RunManager.Get<RunEnvironment>(_indexPath);
        SkillSlot fromSlot = RunManager.Get<SkillSlot>(from.GetIndexPath());
        SkillSlot toSlot = RunManager.Get<SkillSlot>(to.GetIndexPath());
        return runEnvironment.TrySwap(fromSlot, toSlot);
    }
}
