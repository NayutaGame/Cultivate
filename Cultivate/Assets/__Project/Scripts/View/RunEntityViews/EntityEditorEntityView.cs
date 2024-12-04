
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorEntityView : LegacySimpleView
{
    public TMP_Dropdown EntityDropdown;
    public TMP_Dropdown JingJieDropdown;
    
    public Slider SlotCountSlider;
    public TMP_Text SlotCountText;
    
    public TMP_InputField HealthInputField;

    public Slider LadderSlider;
    public TMP_Text LadderText;

    public Toggle InPoolToggle;

    public LegacyListView FieldView;
    public LegacyListView FormationListView;

    public LegacyListView SmirkAgainstListView;
    public LegacyListView AfraidAgainstListView;

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
            // FieldView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
            // FieldView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);
            // FieldView.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit,
            //     CanvasManager.Instance.FormationAnnotation.PointerExit);
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
            SmirkAgainstListView.SetAddress(GetAddress().Append(".SmirkAgainstSlots"));
            // SmirkAgainstListView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
            // SmirkAgainstListView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);
            // SmirkAgainstListView.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit,
            //     CanvasManager.Instance.FormationAnnotation.PointerExit);
            // SmirkAgainstListView.RightClickNeuron.Join(RightClickSlotNeuron);
            SmirkAgainstListView.DropNeuron.Join(DropSmirkAgainstSlotNeuron);
        }

        if (AfraidAgainstListView != null)
        {
            AfraidAgainstListView.SetAddress(GetAddress().Append(".AfraidAgainstSlots"));
            // AfraidAgainstListView.PointerExitNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit);
            // AfraidAgainstListView.PointerMoveNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerMove);
            // AfraidAgainstListView.BeginDragNeuron.Join(CanvasManager.Instance.SkillAnnotation.PointerExit,
            //     CanvasManager.Instance.FormationAnnotation.PointerExit);
            // AfraidAgainstListView.RightClickNeuron.Join(RightClickSlotNeuron);
            AfraidAgainstListView.DropNeuron.Join(DropAfraidAgainstSlotNeuron);
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
        IEntity entity = Get<IEntity>();
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
        SmirkAgainstListView.Refresh();
        AfraidAgainstListView.Refresh();
    }

    private void EntryChanged(int entityEntryIndex)
    {
        IEntity entity = Get<IEntity>();
        entity.SetEntry(Encyclopedia.EntityCategory[entityEntryIndex]);
        Refresh();
    }

    private void JingJieChanged(int jingJie)
    {
        IEntity entity = Get<IEntity>();
        entity.SetJingJie(jingJie);
        Refresh();
    }

    private void SlotCountChanged(float value)
    {
        IEntity entity = Get<IEntity>();
        entity.SetSlotCount((int)value);
        Refresh();
    }

    private void HealthChanged(string value)
    {
        int.TryParse(value, out int health);
        health = Mathf.Clamp(health, 1, 9999);

        IEntity entity = Get<IEntity>();
        entity.SetBaseHealth(health);
        Refresh();
    }

    private void LadderChanged(float value)
    {
        IEntity entity = Get<IEntity>();
        entity.SetLadder((int)value);
        Refresh();
    }

    private void InPoolChanged(bool value)
    {
        IEntity entity = Get<IEntity>();
        entity.SetInPool(value);
        Refresh();
    }

    public Neuron<LegacyInteractBehaviour, PointerEventData> BeginDragSlotNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> RightClickSlotNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DropSlotNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DropSmirkAgainstSlotNeuron = new();
    public Neuron<LegacyInteractBehaviour, LegacyInteractBehaviour, PointerEventData> DropAfraidAgainstSlotNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerEnterFormationNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerExitFormationNeuron = new();
    public Neuron<LegacyInteractBehaviour, PointerEventData> PointerMoveFormationNeuron = new();
}
