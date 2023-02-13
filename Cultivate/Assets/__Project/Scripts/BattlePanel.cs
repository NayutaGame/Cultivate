using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class BattlePanel : Panel
{
    // status
    public AcquiredPoolView AcquiredPoolView;
    public SkillEditor SkillEditor;

    public override void Configure()
    {
        AcquiredPoolView.Configure();
        SkillEditor.Configure();
    }
}
