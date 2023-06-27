
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    public RectTransform _enemySpriteTransform;
    public RectTransform _enemyHandTransform;
    public RectTransform _operationViewTransform;
    public CanvasGroup _operationViewCanvasGroup;

    public SlotInventoryView EnemyHand;
    public Image EnemySprite;

    public TMP_Text HomeHP;
    public GameObject HomeHPSlash;
    public TMP_Text AwayHP;
    public GameObject AwayHPSlash;

    public Button ActButton;

    private InteractDelegate InteractDelegate;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Battle");

        EnemyHand.Configure(new IndexPath($"{_indexPath}.Enemy.Slots"));

        ActButton.onClick.RemoveAllListeners();
        ActButton.onClick.AddListener(Act);
    }

    public override void Refresh()
    {
        EnemyHand.Refresh();

        if (RunManager.Instance.Battle.Report is { } report)
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
        RunManager.Instance.Combat(false, RunManager.Instance.Battle);
        RunCanvas.Instance.Refresh();
    }

    private void Act()
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
