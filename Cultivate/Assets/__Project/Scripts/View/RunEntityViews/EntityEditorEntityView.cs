
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorEntityView : AddressBehaviour
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    public TMP_InputField HealthInputField;
    public Toggle NormalToggle;
    public Toggle EliteToggle;
    public Toggle BossToggle;

    public ListView FieldView;
    public ListView FormationListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        EntityDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.RemoveAllListeners();
        HealthInputField.onValueChanged.RemoveAllListeners();
        NormalToggle.onValueChanged.RemoveAllListeners();
        EliteToggle.onValueChanged.RemoveAllListeners();
        BossToggle.onValueChanged.RemoveAllListeners();

        bool addressIsNull = address == null;

        EntityDropdown.gameObject.SetActive(!addressIsNull);
        JingJieDropdown.gameObject.SetActive(!addressIsNull);
        HealthInputField.gameObject.SetActive(!addressIsNull);
        NormalToggle.gameObject.SetActive(!addressIsNull);
        EliteToggle.gameObject.SetActive(!addressIsNull);
        BossToggle.gameObject.SetActive(!addressIsNull);
        FieldView.gameObject.SetActive(!addressIsNull);
        FormationListView.gameObject.SetActive(!addressIsNull);

        if (addressIsNull)
            return;

        if (EntityDropdown != null)
        {
            EntityDropdown.options = new();
            Encyclopedia.EntityCategory.Traversal.Do(entityEntry => EntityDropdown.options.Add(new TMP_Dropdown.OptionData(entityEntry.Name)));
            EntityDropdown.onValueChanged.AddListener(EntryChanged);
        }

        if (JingJieDropdown != null)
        {
            JingJieDropdown.options = new();
            JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
            JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
        }

        if (HealthInputField != null)
        {
            HealthInputField.onValueChanged.AddListener(HealthChanged);
        }

        NormalToggle.onValueChanged.AddListener(NormalToggled);
        EliteToggle.onValueChanged.AddListener(EliteToggled);
        BossToggle.onValueChanged.AddListener(BossToggled);

        if (FieldView != null)
        {
            FieldView.SetAddress(GetAddress().Append(".Slots"));
            FieldView.PointerEnterNeuron.Set(PointerEnterSlotNeuron);
            FieldView.PointerExitNeuron.Set(PointerExitSlotNeuron);
            FieldView.PointerMoveNeuron.Set(PointerMoveSlotNeuron);
            FieldView.BeginDragNeuron.Set(BeginDragSlotNeuron);
            FieldView.RightClickNeuron.Set(RightClickSlotNeuron);
            FieldView.DropNeuron.Set(DropSlotNeuron);
        }

        if (FormationListView != null)
        {
            FormationListView.SetAddress(GetAddress().Append(".ActivatedSubFormations"));
            FormationListView.PointerEnterNeuron.Set(PointerEnterFormationNeuron);
            FormationListView.PointerExitNeuron.Set(PointerExitFormationNeuron);
            FormationListView.PointerMoveNeuron.Set(PointerMoveFormationNeuron);
        }
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

    private void SetNormal(bool value)
    {
        NormalToggle.SetIsOnWithoutNotify(value);
    }

    private void SetElite(bool value)
    {
        EliteToggle.SetIsOnWithoutNotify(value);
    }

    private void SetBoss(bool value)
    {
        BossToggle.SetIsOnWithoutNotify(value);
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
        SetNormal(entity.IsNormal());
        SetElite(entity.IsElite());
        SetBoss(entity.IsBoss());
        FieldView.Refresh();
        FormationListView.Refresh();
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

    private void NormalToggled(bool value)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetNormal(value);
        Refresh();
    }

    private void EliteToggled(bool value)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetElite(value);
        Refresh();
    }

    private void BossToggled(bool value)
    {
        IEntityModel entity = Get<IEntityModel>();
        entity.SetBoss(value);
        Refresh();
    }

    public Neuron<InteractBehaviour, PointerEventData> PointerEnterSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> BeginDragSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickSlotNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerEnterFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveFormationNeuron = new();
}
