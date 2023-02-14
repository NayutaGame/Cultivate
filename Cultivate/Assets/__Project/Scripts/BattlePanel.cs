using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : Panel
{
    // status
    public AcquiredPoolView AcquiredPoolView;
    public SkillEditor SkillEditor;

    public Button BattleButton;
    public Button EscapeButton;

    public override void Configure()
    {
        AcquiredPoolView.Configure();
        SkillEditor.Configure();
        BattleButton.onClick.AddListener(Battle);
    }

    public void Battle()
    {
        AppManager.Push(new AppStageS());
    }
}
