
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

    public Button CombatButton;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");

        FieldView.SetAddress(_address.Append(".Enemy.Slots"));
        EnemySubFormationInventory.SetAddress(_address.Append(".Enemy.ActivatedSubFormations"));

        CombatButton.onClick.RemoveAllListeners();
        CombatButton.onClick.AddListener(Combat);
    }

    public override void Refresh()
    {
        FieldView.Refresh();
        EnemySubFormationInventory.Refresh();

        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();

        if (d.GetResult() is { } result)
        {
            HomeHP.text = result.HomeLeftHp.ToString();
            AwayHP.text = result.AwayLeftHp.ToString();
            if (result.HomeVictory)
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

    private void Combat()
    {
        BattlePanelDescriptor d = _address.Get<BattlePanelDescriptor>();
        d.Combat();
        RunCanvas.Instance.Refresh();
    }
}
