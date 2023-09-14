
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutableEntityView : MonoBehaviour, IAddress, IInteractable
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    public Button RandomButton;
    public TMP_InputField HealthInputField;
    public Button CopyButton;
    public Button PasteButton;

    public ListView EquippedInventoryView; // SlotView

    private Address _address;
    public Address GetAddress() => _address;
    public T Get<T>() => _address.Get<T>();

    #region Interact

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate()
        => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        EquippedInventoryView.SetDelegate(InteractDelegate);
    }

    #endregion

    public void Configure(Address address)
    {
        _address = address;

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

        EquippedInventoryView.Configure(GetAddress().Append(".Slots"));
    }

    #region Accessors

    private void SetEntry(EntityEntry entry)
    {
        EntityDropdown.SetValueWithoutNotify(entry == null ? 0 : Encyclopedia.EntityCategory.IndexOf(entry));
    }

    private void SetJingJie(JingJie jingJie)
    {
        JingJieDropdown.SetValueWithoutNotify(jingJie);
    }

    private void SetHealth(int health)
    {
        HealthInputField.SetTextWithoutNotify(health.ToString());
    }

    #endregion

    public void Refresh()
    {
        IEntityModel entity = Get<IEntityModel>();
        if (entity == null)
            return;

        SetEntry(entity.GetEntry());
        SetJingJie(entity.GetJingJie());
        SetHealth(entity.GetBaseHealth());
        EquippedInventoryView.Refresh();
    }

    private void EntryChanged(int entityEntryIndex)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetEntry(Encyclopedia.EntityCategory[entityEntryIndex]);
        RunCanvas.Instance.Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetJingJie(jingJie);
        RunCanvas.Instance.Refresh();
    }

    private void RandomEntity()
    {
        IEntityModel entity = Get<IEntityModel>();
        int r = RandomManager.Range(0, Encyclopedia.EntityCategory.GetCount());
        entity.SetEntry(Encyclopedia.EntityCategory[r]);
        RunCanvas.Instance.Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntityModel entity = Get<IEntityModel>();
        entity.SetBaseHealth(health);
        RunCanvas.Instance.Refresh();
    }

    private void Copy()
    {
        IEntityModel entity = Get<IEntityModel>();
        GUIUtility.systemCopyBuffer = entity.ToJson();
    }

    private void Paste()
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.FromJson(GUIUtility.systemCopyBuffer);
        RunCanvas.Instance.Refresh();
    }
}
