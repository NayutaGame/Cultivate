using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyView : ItemView
{
    public TMP_Dropdown EnemyEntryDropdown;
    public TMP_Dropdown JingJieDropdown;
    public Button RandomEnemyButton;
    public TMP_InputField HealthInputField;
    public Button CopyButton;
    public ArenaEnemyEquippedInventoryView ArenaEnemyEquippedInventoryView;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);

        EnemyEntryDropdown.options = new();
        Encyclopedia.EnemyCategory.Traversal.Do(enemyEntry => EnemyEntryDropdown.options.Add(new TMP_Dropdown.OptionData(enemyEntry.Name)));
        EnemyEntryDropdown.onValueChanged.AddListener(EnemyEntryChanged);

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);


        RandomEnemyButton.onClick.AddListener(RandomEnemy);

        HealthInputField.onValueChanged.AddListener(HealthChanged);

        CopyButton.onClick.AddListener(Copy);

        ArenaEnemyEquippedInventoryView.Configure(new IndexPath($"{GetIndexPath()}.Slots"));
    }

    public override void Refresh()
    {
        base.Refresh();

        RunEnemy runEnemy = RunManager.Get<RunEnemy>(GetIndexPath());

        if (runEnemy == null)
            return;

        EnemyEntryDropdown.value = Encyclopedia.EnemyCategory.IndexOf(runEnemy.Entry);
        JingJieDropdown.value = runEnemy.JingJie;
        HealthInputField.text = runEnemy.Health.ToString();
        ArenaEnemyEquippedInventoryView.Refresh();
    }

    protected abstract void EnemyEntryChanged(int enemyEntryIndex);

    private void JingJieChanged(int jingJie)
    {
        RunManager.Get<RunEnemy>(GetIndexPath()).JingJie = jingJie;
        RunCanvas.Instance.Refresh();
    }

    protected abstract void RandomEnemy();

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int hp);
        hp = Mathf.Clamp(hp, 1, 9999);

        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        enemy.Health = hp;

        RunCanvas.Instance.Refresh();
    }

    private void Copy()
    {
        RunEnemy enemy = RunManager.Get<RunEnemy>(GetIndexPath());
        GUIUtility.systemCopyBuffer = enemy.GetEntryDescriptor();
    }
}
