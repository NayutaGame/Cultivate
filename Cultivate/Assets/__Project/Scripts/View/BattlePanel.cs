using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    // status
    [FormerlySerializedAs("AcquiredPoolView")] public ChipInventoryView chipInventoryView;
    public SkillEditor SkillEditor;

    public Button BattleButton;
    public Button EscapeButton;

    public override void Configure()
    {
        chipInventoryView.Configure();
        SkillEditor.Configure();
        BattleButton.onClick.AddListener(Battle);
    }

    public override void Refresh()
    {
        chipInventoryView.Refresh();
        SkillEditor.Refresh();
    }

    public void Battle()
    {
        AppManager.Push(new AppStageS());
    }
}
