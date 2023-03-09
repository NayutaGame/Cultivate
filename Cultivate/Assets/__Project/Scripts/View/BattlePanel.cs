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
    public Button NextEnemyButton;

    public override void Configure()
    {
        AcquiredWaiGongInventoryView.Configure(RunManager.Instance.AcquiredInventory);
        SkillEditor.Configure();
        BattleButton.onClick.AddListener(Battle);
        NextEnemyButton.onClick.AddListener(NextEnemy);
    }

    public override void Refresh()
    {
        AcquiredWaiGongInventoryView.Refresh();
        SkillEditor.Refresh();
    }

    private void Battle()
    {
        AppManager.Push(new AppStageS());
    }

    private void NextEnemy()
    {
        RunManager.Instance.NextEnemyFromPool();
        SkillEditor.EnemyHPInputField.text = RunManager.Instance.Enemy.Health.ToString();
        SkillEditor.Refresh();
    }
}
