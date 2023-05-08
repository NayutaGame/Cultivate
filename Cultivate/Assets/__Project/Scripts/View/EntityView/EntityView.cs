
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityView : MonoBehaviour, IIndexPath
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    public Button RandomButton;
    public TMP_InputField HealthInputField;
    public Button CopyButton;
    public Button PasteButton;

    public InventoryView<AbstractSkillView> EquippedInventoryView;

    private IndexPath _indexPath;
    public IndexPath GetIndexPath()
        => _indexPath;

    public void Configure(IndexPath indexPath)
    {
        _indexPath = indexPath;

        EntityDropdown.options = new();
        Encyclopedia.EntityCategory.Traversal.Do(entityEntry => EntityDropdown.options.Add(new TMP_Dropdown.OptionData(entityEntry.Name)));
        EntityDropdown.onValueChanged.AddListener(EntryChanged);

        JingJieDropdown.options = new();
        JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
        JingJieDropdown.onValueChanged.AddListener(JingJieChanged);

        RandomButton.onClick.AddListener(RandomEntity);
        HealthInputField.onValueChanged.AddListener(HealthChanged);
        CopyButton.onClick.AddListener(Copy);
        PasteButton.onClick.AddListener(Paste);

        EquippedInventoryView.Configure(new IndexPath($"{GetIndexPath()}.Slots"));
    }

    #region Accessors

    private void SetEntry(EntityEntry entry)
    {
        EntityDropdown.value = entry == null ? 0 : Encyclopedia.EntityCategory.IndexOf(entry);
    }

    private void SetJingJie(JingJie jingJie)
    {
        JingJieDropdown.value = jingJie;
    }

    private void SetHealth(int health)
    {
        HealthInputField.text = health.ToString();
    }

    #endregion

    public void Refresh()
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        if (entity == null)
            return;

        SetEntry(entity.GetEntry());
        SetJingJie(entity.GetJingJie());
        SetHealth(entity.GetHealth());
        EquippedInventoryView.Refresh();
    }

    private void EntryChanged(int entityEntryIndex)
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        entity.SetEntry(Encyclopedia.EntityCategory[entityEntryIndex]);
        RunCanvas.Instance.Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        entity.SetJingJie(jingJie);
        RunCanvas.Instance.Refresh();
    }

    private void RandomEntity()
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        int r = RandomManager.Range(0, Encyclopedia.EntityCategory.GetCount());
        entity.SetEntry(Encyclopedia.EntityCategory[r]);
        RunCanvas.Instance.Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        entity.SetHealth(health);
        RunCanvas.Instance.Refresh();
    }

    private void Copy()
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        GUIUtility.systemCopyBuffer = entity.ToJson();
    }

    private void Paste()
    {
        IEntityModel entity = RunManager.Get<IEntityModel>(GetIndexPath());
        entity.FromJson(GUIUtility.systemCopyBuffer);
    }
}
