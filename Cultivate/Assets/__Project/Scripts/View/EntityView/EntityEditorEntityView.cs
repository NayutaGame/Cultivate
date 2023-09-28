
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityEditorEntityView : ItemView, IInteractable
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    public TMP_InputField HealthInputField;
    public ListView SlotListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        if (address == null)
        {
            EntityDropdown.onValueChanged.RemoveAllListeners();
            JingJieDropdown.onValueChanged.RemoveAllListeners();
            HealthInputField.onValueChanged.RemoveAllListeners();

            EntityDropdown.gameObject.SetActive(false);
            JingJieDropdown.gameObject.SetActive(false);
            HealthInputField.gameObject.SetActive(false);
            SlotListView.gameObject.SetActive(false);

            return;
        }

        EntityDropdown.gameObject.SetActive(true);
        JingJieDropdown.gameObject.SetActive(true);
        HealthInputField.gameObject.SetActive(true);
        SlotListView.gameObject.SetActive(true);

        if (EntityDropdown != null)
        {
            EntityDropdown.options = new();
            Encyclopedia.EntityCategory.Traversal.Do(entityEntry => EntityDropdown.options.Add(new TMP_Dropdown.OptionData(entityEntry.Name)));
            EntityDropdown.onValueChanged.RemoveAllListeners();
            EntityDropdown.onValueChanged.AddListener(EntryChanged);
        }

        if (JingJieDropdown != null)
        {
            JingJieDropdown.options = new();
            JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
            JingJieDropdown.onValueChanged.RemoveAllListeners();
            JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
        }

        if (HealthInputField != null)
        {
            HealthInputField.onValueChanged.RemoveAllListeners();
            HealthInputField.onValueChanged.AddListener(HealthChanged);
        }

        if (SlotListView != null)
            SlotListView.SetAddress(GetAddress().Append(".Slots"));
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

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
            return;
        IEntityModel entity = Get<IEntityModel>();
        if (entity == null)
            return;
        SetEntry(entity.GetEntry());
        SetJingJie(entity.GetJingJie());
        SetHealth(entity.GetBaseHealth());
        SlotListView.Refresh();
    }

    private void EntryChanged(int entityEntryIndex)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetEntry(Encyclopedia.EntityCategory[entityEntryIndex]);
        Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetJingJie(jingJie);
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntityModel entity = Get<IEntityModel>();
        entity.SetBaseHealth(health);
        Refresh();
    }

    // private void Copy()
    // {
    //     IEntityModel entity = Get<IEntityModel>();
    //     GUIUtility.systemCopyBuffer = entity.ToJson();
    // }
    //
    // private void Paste()
    // {
    //     IEntityModel entity = Get<IEntityModel>();
    //     entity.FromJson(GUIUtility.systemCopyBuffer);
    //     RunCanvas.Instance.Refresh();
    // }

    #region Interact

    private InteractDelegate InteractDelegate;
    public InteractDelegate GetDelegate() => InteractDelegate;
    public void SetDelegate(InteractDelegate interactDelegate)
    {
        InteractDelegate = interactDelegate;
        if (SlotListView != null)
            SlotListView.SetDelegate(InteractDelegate);
    }

    #endregion
}
