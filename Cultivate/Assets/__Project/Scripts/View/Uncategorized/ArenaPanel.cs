using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaPanel : Panel
{
    public SkillInventoryView SkillInventoryView;
    public ArenaEditorView ArenaEditorView;
    public ArenaScoreboardView ArenaScoreboardView;
    public TMP_Text ReportView;

    public override void Configure()
    {
        base.Configure();

        SkillInventoryView.Configure(new IndexPath("Arena.SkillInventory"));
        ArenaEditorView.Configure(new IndexPath("Arena"));
        ArenaScoreboardView.Configure();
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillInventoryView.Refresh();
        ArenaEditorView.Refresh();
        ArenaScoreboardView.Refresh();

        ReportView.text = RunManager.Instance.Arena.Report?.ToString();
    }
}
