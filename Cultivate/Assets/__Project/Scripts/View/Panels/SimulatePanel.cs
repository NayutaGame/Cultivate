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

    public Button ReportButton;
    public Button StreamButton;

    public TMP_Text ReportText;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(new IndexPath("AcquiredInventory"));
        SkillEditor.Configure();
        ReportButton.onClick.AddListener(Report);
        StreamButton.onClick.AddListener(Stream);
    }

    public override void Refresh()
    {
        AcquiredWaiGongInventoryView.Refresh();
        SkillEditor.Refresh();

        ReportText.text = RunManager.Instance.Report?.ToString();
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
