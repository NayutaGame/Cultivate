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
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;
    public SkillEditor SkillEditor;

    public Button NextEnemyButton;
    public Button ReportButton;
    public Button StreamButton;
    public Button CopyEnemyButton;

    public TMP_Text ReportText;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(new IndexPath("AcquiredInventory"));
        SkillEditor.Configure();
        NextEnemyButton.onClick.AddListener(NextEnemy);
        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
        CopyEnemyButton.onClick.AddListener(CopyEnemy);
    }

    public override void Refresh()
    {
        AcquiredWaiGongInventoryView.Refresh();
        SkillEditor.Refresh();

        ReportText.text = RunManager.Instance.Report?.ToString();
    }

    private void NextEnemy()
    {
        CreateEnemyDetails d = new CreateEnemyDetails();
        EnemyEntry enemyEntry = RunManager.Instance.DrawEnemy(d);
        RunManager.Instance.SetEnemy(new RunEnemy(enemyEntry, d));

        SkillEditor.EnemyHPInputField.text = RunManager.Instance.Enemy.Health.ToString();
        SkillEditor.Refresh();
    }

    private void Report()
    {
        RunManager.Instance.GenerateReport();
    }

    private void Stream()
    {
        RunManager.Instance.Stream();
    }

    private void CopyEnemy()
    {
        GUIUtility.systemCopyBuffer = RunManager.Instance.Enemy.GetEntryDescriptor();
    }
}
