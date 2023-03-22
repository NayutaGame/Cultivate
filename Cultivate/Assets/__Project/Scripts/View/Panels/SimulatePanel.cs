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

    public TMP_Text ReportText;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(RunManager.Instance.AcquiredInventory);
        SkillEditor.Configure();
        NextEnemyButton.onClick.AddListener(NextEnemy);
        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
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
        RunEnemy e = RunManager.Instance.DrawEnemy(d).Create(d);
        RunManager.Instance.SetEnemy(e);

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
}
