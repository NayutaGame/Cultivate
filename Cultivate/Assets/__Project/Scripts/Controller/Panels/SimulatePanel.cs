
using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimulatePanel : Panel
{
    public MutableEntityView EnemyView;
    public MutableEntityView HeroView;
    public SkillInventoryView SkillInventoryView;

    public TMP_Text SimulatedHP;
    public Image Light;
    public Button ReportButton;
    public Button StreamButton;
    public TMP_Text ReportText;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Simulate");

        ConfigureInteractDelegate();

        HeroView.Configure(new Address($"{_address}.Hero"));
        HeroView.SetDelegate(InteractDelegate);

        EnemyView.Configure(new Address($"{_address}.Enemy"));
        EnemyView.SetDelegate(InteractDelegate);

        SkillInventoryView.Configure(new Address($"{_address}.SkillInventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);

        ReportButton.onClick.RemoveAllListeners();
        ReportButton.onClick.AddListener(Report);

        StreamButton.onClick.RemoveAllListeners();
        StreamButton.onClick.AddListener(Stream);
    }

    private void ConfigureInteractDelegate()
    {
        // InteractDelegate = new(4,
        //     getId: view =>
        //     {
        //         object item = DataManager.Get<object>(view.GetIndexPath());
        //         if (item is RunSkill)
        //             return 0;
        //         if (item is SkillInventory)
        //             return 1;
        //         if (item is SkillSlot slot)
        //         {
        //             RunEnvironment runEnvironment = DataManager.Get<RunEnvironment>(_indexPath);
        //             if (runEnvironment.Hero == slot.Owner)
        //                 return 2;
        //             if (runEnvironment.Enemy == slot.Owner)
        //                 return 3;
        //         }
        //         return null;
        //     },
        //     dragDropTable: new Func<IInteractable, IInteractable, bool>[]
        //     {
        //         /*                     RunSkill,   SkillInventory, SkillSlot(Hero), SkillSlot(Enemy) */
        //         /* RunSkill         */ TryMerge,   null,           TryEquip,        TryWrite,
        //         /* SkillInventory   */ null,       null,           null,            null,
        //         /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         TryWrite,
        //         /* SkillSlot(Enemy) */ null,       null,           null,            null,
        //     },
        //     rMouseTable: new Func<IInteractable, bool>[]
        //     {
        //         /* RunSkill         */ TryIncreaseJingJie,
        //         /* SkillInventory   */ null,
        //         /* SkillSlot(Hero)  */ TryIncreaseJingJie,
        //         /* SkillSlot(Enemy) */ TryIncreaseJingJie,
        //     });
    }

    public override void Refresh()
    {
        HeroView.Refresh();
        EnemyView.Refresh();
        SkillInventoryView.Refresh();

        // if (RunManager.Instance.Simulate.Report is { } report)
        // {
        //     SimulatedHP.text = $"玩家 : 怪物\n{report.HomeLeftHp} : {report.AwayLeftHp}";
        //     Light.color = report.HomeVictory ? Color.green : Color.red;
        //     ReportText.text = report.ToString();
        // }
        // else
        // {
        //     SimulatedHP.text = $"玩家 : 怪物\n无结果";
        //     Light.color = Color.gray;
        //     ReportText.text = "";
        // }
    }

    private void Report()
    {
        // RunManager.Instance.Combat(false, RunManager.Instance.Simulate);
        // RunCanvas.Instance.Refresh();
    }

    private void Stream()
    {
        // RunManager.Instance.Combat(true, RunManager.Instance.Simulate);
        // RunCanvas.Instance.Refresh();
    }

    private bool TryMerge(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
        RunSkill lhs = from.Get<RunSkill>();
        RunSkill rhs = to.Get<RunSkill>();
        return runEnvironment.TryMerge(lhs, rhs);
    }

    private bool TryEquip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
        RunSkill toEquip = from.Get<RunSkill>();
        SkillSlot slot = to.Get<SkillSlot>();
        return runEnvironment.TryEquipSkill(toEquip, slot);
    }

    private bool TryUnequip(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
        SkillSlot slot = from.Get<SkillSlot>();
        return runEnvironment.TryUnequip(slot, null);
    }

    private bool TrySwap(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();
        SkillSlot fromSlot = from.Get<SkillSlot>();
        SkillSlot toSlot = to.Get<SkillSlot>();
        return runEnvironment.TrySwap(fromSlot, toSlot);
    }

    private bool TryWrite(IInteractable from, IInteractable to)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();

        object fromItem = from.Get<object>();
        SkillSlot toSlot = to.Get<SkillSlot>();

        if (fromItem is RunSkill fromSkill)
        {
            return runEnvironment.TryWrite(fromSkill, toSlot);
        }
        else if (fromItem is SkillSlot fromSlot)
        {
            return runEnvironment.TryWrite(fromSlot, toSlot);
        }

        return false;
    }

    private bool TryIncreaseJingJie(IInteractable view)
    {
        RunEnvironment runEnvironment = _address.Get<RunEnvironment>();

        object item = view.Get<object>();

        if (item is RunSkill skill)
        {
            return runEnvironment.TryIncreaseJingJie(skill);
        }
        else if (item is SkillSlot slot)
        {
            return runEnvironment.TryIncreaseJingJie(slot);
        }

        return false;
    }
}
