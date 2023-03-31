using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using CLLibrary;
using UnityEngine.Serialization;

public class BattlePanel : Panel
{
    public TMP_Text HeroJingJieText;
    public TMP_Text HeroHPText;
    public TMP_Text SimulatedPlayerHP;
    public TMP_Text SimulatedEnemyHP;

    public BattleEnemyView BattleEnemyView;
    public RunChipInventoryView HeroEquippedInventoryView;
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;

    public Button CommitButton;
    public TMP_Text ReportText;

    public override void Configure()
    {
        BattleEnemyView.Configure(new IndexPath("Enemy"));
        HeroEquippedInventoryView.Configure(new IndexPath("Hero.HeroSlotInventory.Slots"));
        AcquiredWaiGongInventoryView.Configure(new IndexPath("AcquiredInventory"));
        CommitButton.onClick.AddListener(Commit);
    }

    public override void Refresh()
    {
        base.Refresh();
        HeroJingJieText.text = RunManager.Instance.JingJie.ToString();
        HeroHPText.text = RunManager.Instance.Hero.Health.ToString();

        if (RunManager.Instance.Report is { } report)
        {
            SimulatedPlayerHP.text = $"玩家剩余生命: {report.HomeLeftHp}";
            SimulatedEnemyHP.text = $"怪物剩余生命: {report.AwayLeftHp}";
            ReportText.text = report.ToString();
        }

        BattleEnemyView.Refresh();
        HeroEquippedInventoryView.Refresh();
        AcquiredWaiGongInventoryView.Refresh();
    }

    private void Commit()
    {
        RunManager.Instance.GenerateReport();
        RunCanvas.Instance.Refresh();
    }
}
