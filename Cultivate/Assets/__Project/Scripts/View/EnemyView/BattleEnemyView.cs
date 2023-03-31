using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemyView : ItemView
{
    public TMP_Text NameText;
    public TMP_Text JingJieText;
    public TMP_Text HPText;
    public TMP_Text DescriptionText;
    public Button CopyButton;
    public InventoryView<RunChipView> EquippedInventoryView;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        CopyButton.onClick.AddListener(Copy);

        EquippedInventoryView.Configure(new IndexPath($"{GetIndexPath()}.Slots"));
    }

    public override void Refresh()
    {
        base.Refresh();

        RunEnemy runEnemy = RunManager.Get<RunEnemy>(GetIndexPath());

        if (runEnemy == null)
            return;

        NameText.text = runEnemy.Entry.Name;
        JingJieText.text = runEnemy.JingJie.ToString();
        HPText.text = runEnemy.Health.ToString();
        DescriptionText.text = runEnemy.Entry.Description;
        EquippedInventoryView.Refresh();
    }

    private void Copy()
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        GUIUtility.systemCopyBuffer = enemy.GetEntryDescriptor();
    }
}
