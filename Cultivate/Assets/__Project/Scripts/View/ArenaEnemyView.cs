using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaEnemyView : ItemView
{
    public TMP_Dropdown EnemyEntryDropdown;
    public Button RandomEnemyButton;
    public TMP_InputField HealthInputField;
    public Button CopyButton;
    // public ArenaEnemyEquippedInventoryView ArenaEnemyEquippedInventoryView;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        List<TMP_Dropdown.OptionData> options = new();
        Encyclopedia.EnemyCategory.Traversal.Do(enemyEntry => options.Add(new TMP_Dropdown.OptionData(enemyEntry.Name)));
        EnemyEntryDropdown.options = options;
        EnemyEntryDropdown.onValueChanged.AddListener(EnemyEntryChanged);

        RandomEnemyButton.onClick.AddListener(RandomEnemy);

        HealthInputField.onValueChanged.AddListener(HealthChanged);

        CopyButton.onClick.AddListener(Copy);

        // ArenaEnemyEquippedInventoryView.Configure();
    }

    public override void Refresh()
    {
        base.Refresh();

        RunEnemy runEnemy = RunManager.Get<RunEnemy>(GetIndexPath());

        if (runEnemy == null)
            return;

        EnemyEntryDropdown.value = Encyclopedia.EnemyCategory.IndexOf(runEnemy.Entry);
        HealthInputField.text = runEnemy.Health.ToString();
        // ArenaEnemyEquippedInventoryView.Refresh();
    }

    public void EnemyEntryChanged(int enemyEntryIndex)
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        int index = RunManager.Instance.Arena.IndexOf(enemy);
        RunManager.Instance.Arena.SetEnemy(index, enemyEntryIndex);
        RunCanvas.Instance.Refresh();
    }

    public void RandomEnemy()
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        int index = RunManager.Instance.Arena.IndexOf(enemy);
        RunManager.Instance.Arena.SetRandom(index);
        RunCanvas.Instance.Refresh();
    }

    public void HealthChanged(string value)
    {
        int.TryParse(value, out int hp);
        hp = Mathf.Clamp(hp, 1, 9999);

        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        enemy.Health = hp;

        RunCanvas.Instance.Refresh();
    }

    public void Copy()
    {
        RunEnemy runEnemy = RunManager.Get<RunEnemy>(GetIndexPath());
        GUIUtility.systemCopyBuffer = runEnemy.GetEntryDescriptor();
    }
}
