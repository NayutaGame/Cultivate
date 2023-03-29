using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SimulatePanel : Panel
{
    public TMP_InputField HeroHPInputField;
    public TMP_Text SimulatedPlayerHP;
    public TMP_Text SimulatedEnemyHP;

    public EnemyView EnemyView;
    public RunChipInventoryView HeroEquippedInventoryView;
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;







    public Button ReportButton;
    public Button StreamButton;

    public TMP_Text ReportText;

    public override void Configure()
    {
        HeroHPInputField.text = RunManager.Instance.Hero.Health.ToString();
        HeroHPInputField.onEndEdit.AddListener(SetHeroHP);

        EnemyView.Configure(new IndexPath("Enemy"));
        HeroEquippedInventoryView.Configure(new IndexPath("Hero.HeroSlotInventory.Slots"));
        AcquiredWaiGongInventoryView.Configure(new IndexPath("AcquiredInventory"));





        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
    }

    public override void Refresh()
    {
        HeroHPInputField.text = RunManager.Instance.Hero.Health.ToString();

        if (RunManager.Instance.Report != null)
        {
            SimulatedPlayerHP.text = $"玩家剩余生命: {RunManager.Instance.Report.HomeLeftHp}";
            SimulatedEnemyHP.text = $"怪物剩余生命: {RunManager.Instance.Report.AwayLeftHp}";
        }

        EnemyView.Refresh();
        HeroEquippedInventoryView.Refresh();
        AcquiredWaiGongInventoryView.Refresh();






        ReportText.text = RunManager.Instance.Report?.ToString();
    }

    private void SetHeroHP(string hpString)
    {
        int hp = int.Parse(hpString);
        hp = Mathf.Clamp(hp, 1, 9999);
        RunManager.Instance.Hero.Health = hp;
    }

    private void Report()
    {
        RunManager.Instance.GenerateReport();
    }

    private void Stream()
    {
        RunManager.Instance.Stream();
    }
}
