using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;
    // public SkillEditor SkillEditor;

    public Button ReportButton;
    public Button EscapeButton;

    public TMP_Text ReportText;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(new IndexPath("AcquiredInventory"));
        // SkillEditor.Configure();
        ReportButton.onClick.AddListener(Report);
        EscapeButton.onClick.AddListener(Escape);
    }

    public override void Refresh()
    {
        base.Refresh();

        AcquiredWaiGongInventoryView.Refresh();
        // SkillEditor.Refresh();

        ReportText.text = RunManager.Instance.Report?.ToString();
    }

    private void Report()
    {
        AppManager.Push(new AppStageS());
    }

    private void Escape()
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        BattlePanelDescriptor d = runNode.CurrentPanel as BattlePanelDescriptor;
        d.ReceiveSignal(new BattleResultSignal(BattleResultSignal.BattleResultState.Escape));
        RunCanvas.Instance.Refresh();
    }
}
