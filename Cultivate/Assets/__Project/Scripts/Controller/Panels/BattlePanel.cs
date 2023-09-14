
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    public RectTransform _enemySpriteTransform;
    public RectTransform _enemyHandTransform;
    public RectTransform _enemySubFormationInventoryTransform;

    public RectTransform _operationViewTransform;
    public CanvasGroup _operationViewCanvasGroup;

    public ListView FieldView; // SlotView
    public Image EnemySprite;
    public ListView EnemySubFormationInventory; // FormationView

    public TMP_Text HomeHP;
    public GameObject HomeHPSlash;
    public TMP_Text AwayHP;
    public GameObject AwayHPSlash;

    public Button ActButton;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");

        FieldView.Configure(_address.Append(".Enemy.Slots"));
        EnemySubFormationInventory.Configure(_address.Append(".Enemy.ActivatedSubFormations"));

        ActButton.onClick.RemoveAllListeners();
        ActButton.onClick.AddListener(Act);
    }

    public override void Refresh()
    {
        FieldView.Refresh();
        EnemySubFormationInventory.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();

        if (d.Report is { } report)
        {
            HomeHP.text = report.HomeLeftHp.ToString();
            AwayHP.text = report.AwayLeftHp.ToString();
            if (report.HomeVictory)
            {
                HomeHP.color = Color.white;
                HomeHPSlash.SetActive(false);
                AwayHP.color = Color.black;
                AwayHPSlash.SetActive(true);
            }
            else
            {
                HomeHP.color = Color.black;
                HomeHPSlash.SetActive(true);
                AwayHP.color = Color.white;
                AwayHPSlash.SetActive(false);
            }
            // ReportText.text = report.ToString();
        }
        else
        {
            HomeHP.text = "玩家";
            AwayHP.text = "怪物";
            HomeHP.color = Color.white;
            HomeHPSlash.SetActive(false);
            AwayHP.color = Color.white;
            AwayHPSlash.SetActive(false);
            // ReportText.text = "";
        }
    }

    private void Report()
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat(false, true);
        RunCanvas.Instance.Refresh();
    }

    private void Act()
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat(true, true);
        RunCanvas.Instance.Refresh();
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
}
