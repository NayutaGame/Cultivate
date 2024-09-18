
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorEntityView : SimpleView
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    
    public Slider SlotCountSlider;
    public TMP_Text SlotCountText;
    
    public TMP_InputField HealthInputField;

    public Slider LadderSlider;
    public TMP_Text LadderText;

    public Toggle InPoolToggle;

    public ListView FieldView;
    public ListView FormationListView;

    public ListView SmirkAgainstListView;
    public ListView AfraidAgainstListView;

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        EntityDropdown.onValueChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.RemoveAllListeners();
        SlotCountSlider.onValueChanged.RemoveAllListeners();
        HealthInputField.onValueChanged.RemoveAllListeners();
        LadderSlider.onValueChanged.RemoveAllListeners();
        InPoolToggle.onValueChanged.RemoveAllListeners();

        bool addressIsNull = address == null;

        EntityDropdown.gameObject.SetActive(!addressIsNull);
        JingJieDropdown.gameObject.SetActive(!addressIsNull);
        SlotCountSlider.gameObject.SetActive(!addressIsNull);
        HealthInputField.gameObject.SetActive(!addressIsNull);
        LadderSlider.gameObject.SetActive(!addressIsNull);
        InPoolToggle.gameObject.SetActive(!addressIsNull);
        FieldView.gameObject.SetActive(!addressIsNull);
        FormationListView.gameObject.SetActive(!addressIsNull);

        if (addressIsNull)
            return;

        if (EntityDropdown != null)
        {
            EntityDropdown.options = new();
            Encyclopedia.EntityCategory.Traversal.Do(entityEntry => EntityDropdown.options.Add(new TMP_Dropdown.OptionData(entityEntry.GetName())));
            EntityDropdown.onValueChanged.AddListener(EntryChanged);
        }

        if (JingJieDropdown != null)
        {
            JingJieDropdown.options = new();
            JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.ToString())));
            JingJieDropdown.onValueChanged.AddListener(JingJieChanged);
        }

        if (SlotCountSlider != null)
            SlotCountSlider.onValueChanged.AddListener(SlotCountChanged);

        if (HealthInputField != null)
            HealthInputField.onValueChanged.AddListener(HealthChanged);

        if (LadderSlider != null)
            LadderSlider.onValueChanged.AddListener(LadderChanged);
        
        if (InPoolToggle != null)
            InPoolToggle.onValueChanged.AddListener(InPoolChanged);

        if (FieldView != null)
        {
            FieldView.SetAddress(GetAddress().Append(".Slots"));
            FieldView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
            FieldView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);
            FieldView.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit,
                CanvasManager.Instance.FormationAnnotation.PointerExit);
            FieldView.RightClickNeuron.Join(RightClickSlotNeuron);
            FieldView.DropNeuron.Join(DropSlotNeuron);
        }

        if (FormationListView != null)
        {
            FormationListView.SetAddress(GetAddress().Append(".ShowingFormations"));
            FormationListView.PointerEnterNeuron.Join(PointerEnterFormationNeuron);
            FormationListView.PointerExitNeuron.Join(PointerExitFormationNeuron);
            FormationListView.PointerMoveNeuron.Join(PointerMoveFormationNeuron);
        }

        if (SmirkAgainstListView != null)
        {
            
        }

        if (AfraidAgainstListView != null)
        {
            
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

    private void SetSlotCount(int value)
    {
        SlotCountSlider.SetValueWithoutNotify(value);
        SlotCountText.text = value.ToString();
    }

    private void SetHealth(int health)
    {
        HealthInputField.SetTextWithoutNotify(health.ToString());
    }

    private void SetLadder(int value)
    {
        LadderSlider.SetValueWithoutNotify(value);
        LadderText.text = value.ToString();
    }

    private void SetInPool(bool inPool)
    {
        InPoolToggle.SetIsOnWithoutNotify(inPool);
    }

    #endregion

    public override void Refresh()
    {
        base.Refresh();
        if (GetAddress() == null)
            return;
        EntityModel entity = Get<EntityModel>();
        if (entity == null)
            return;
        SetEntry(entity.GetEntry());
        SetJingJie(entity.GetJingJie());
        SetSlotCount(entity.GetSlotCount());
        SetHealth(entity.GetBaseHealth());
        SetLadder(entity.GetLadder());
        SetInPool(entity.IsInPool());
        FieldView.Refresh();
        FormationListView.Refresh();
    }

    private void EntryChanged(int entityEntryIndex)
    {
        EntityModel entity = Get<EntityModel>();
        entity.SetEntry(Encyclopedia.EntityCategory[entityEntryIndex]);
        Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        EntityModel entity = Get<EntityModel>();
        entity.SetJingJie(jingJie);
        Refresh();
    }

    private void SlotCountChanged(float value)
    {
        EntityModel entity = Get<EntityModel>();
        entity.SetSlotCount((int)value);
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        EntityModel entity = Get<EntityModel>();
        entity.SetBaseHealth(health);
        Refresh();
    }

    private void LadderChanged(float value)
    {
        EntityModel entity = Get<EntityModel>();
        entity.SetLadder((int)value);
        Refresh();
    }

    private void InPoolChanged(bool value)
    {
        EntityModel entity = Get<EntityModel>();
        entity.SetInPool(value);
        Refresh();
    }

    public Neuron<InteractBehaviour, PointerEventData> BeginDragSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> RightClickSlotNeuron = new();
    public Neuron<InteractBehaviour, InteractBehaviour, PointerEventData> DropSlotNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerEnterFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerExitFormationNeuron = new();
    public Neuron<InteractBehaviour, PointerEventData> PointerMoveFormationNeuron = new();
}
