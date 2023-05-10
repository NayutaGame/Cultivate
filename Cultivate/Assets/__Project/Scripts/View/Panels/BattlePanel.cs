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
    public TMP_Text SimulatedHP;

    public BattleEnemyView BattleEnemyView;
    public RunChipInventoryView HeroEquippedInventoryView;
    public SkillInventoryView SkillInventoryView;

    public Button CommitButton;
    public TMP_Text CommitText;
    public TMP_Text ReportText;

    public override void Configure()
    {
        BattleEnemyView.Configure(new IndexPath("Enemy"));
        HeroEquippedInventoryView.Configure(new IndexPath("Hero.HeroSlotInventory.Slots"));
        SkillInventoryView.Configure(new IndexPath("AcquiredInventory"));
        CommitButton.onClick.AddListener(Commit);
    }

    public override void Refresh()
    {
        base.Refresh();
        HeroJingJieText.text = RunManager.Instance.Battle.Hero.GetJingJie().ToString();
        HeroHPText.text = RunManager.Instance.Battle.Hero.GetHealth().ToString();

        if (RunManager.Instance.Report is { } report)
        {
            SimulatedHP.text = $"玩家 {report.HomeLeftHp} : {report.AwayLeftHp} 敌人";
            ReportText.text = report.ToString();
            if (report.HomeVictory)
            {
                CommitText.color = Color.green;
                CommitText.text = $"提交胜利";
            }
            else
            {
                CommitText.color = Color.gray;
                CommitText.text = $"提交逃跑\n命元: {RunManager.Instance.MingYuan}->{RunManager.Instance.MingYuan - report.MingYuanPenalty}";
            }
        }
        else
        {
            SimulatedHP.text = "";
            ReportText.text = "";
            CommitText.color = Color.black;
            CommitText.text = "刷新";
        }

        BattleEnemyView.Refresh();
        HeroEquippedInventoryView.Refresh();
        SkillInventoryView.Refresh();
    }

    private void Commit()
    {
        // RunManager.Instance.Combat();
        RunCanvas.Instance.Refresh();
    }
}
