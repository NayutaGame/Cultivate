using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SimulatePanel : Panel
{
    // status
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;
    public SkillEditor SkillEditor;

    public Button ReportButton;
    public Button EscapeButton;
    public Button NextEnemyButton;

    public TMP_Text ReportText;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(RunManager.Instance.AcquiredInventory);
        SkillEditor.Configure();
        ReportButton.onClick.AddListener(Report);
        NextEnemyButton.onClick.AddListener(NextEnemy);
    }

    public override void Refresh()
    {
        AcquiredWaiGongInventoryView.Refresh();
        SkillEditor.Refresh();
    }

    private void Report()
    {
        AppManager.Push(new AppStageS());
    }

    private void NextEnemy()
    {
        RunManager.Instance.TryDrawEnemy();
        SkillEditor.EnemyHPInputField.text = RunManager.Instance.Enemy.Health.ToString();
        SkillEditor.Refresh();
    }
}
