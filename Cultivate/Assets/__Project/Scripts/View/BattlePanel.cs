using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    // status
    public AcquiredWaiGongInventoryView AcquiredWaiGongInventoryView;
    public SkillEditor SkillEditor;

    public Button BattleButton;
    public Button EscapeButton;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(RunManager.Instance.AcquiredInventory);
        SkillEditor.Configure();
        BattleButton.onClick.AddListener(Battle);
    }

    public override void Refresh()
    {
        AcquiredWaiGongInventoryView.Refresh();
        SkillEditor.Refresh();
    }

    public void Battle()
    {
        AppManager.Push(new AppStageS());
    }
}
