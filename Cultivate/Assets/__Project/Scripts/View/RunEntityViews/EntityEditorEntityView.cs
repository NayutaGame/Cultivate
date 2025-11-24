
using CLLibrary;
using TMPro;
using UIRangeSliderNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorEntityView : XView
{
    public TMP_Dropdown EntityDropdown;

    public Slider LadderSlider;
    public TMP_Text LadderText;

    public UIRangeSlider AllowedDifficultySlider;
    public TMP_Text LowValueLabel;
    public TMP_Text HighValueLabel;
    
    public TMP_Dropdown JingJieDropdown;
    
    public Slider SlotCountSlider;
    public TMP_Text SlotCountText;
    
    public TMP_InputField HealthInputField;

    public Toggle InPoolToggle;

    public ListView FieldView;
    public ListView FormationListView;

    public GameObject Blank;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        if (FieldView != null)
        {
            FieldView.CheckAwake();
        }

        if (FormationListView != null)
            FormationListView.CheckAwake();
    }

    public override void SetAddress(Address address)
    {
        base.SetAddress(address);

        EntityDropdown.onValueChanged.RemoveAllListeners();
        AllowedDifficultySlider.onValuesChanged.RemoveAllListeners();
        JingJieDropdown.onValueChanged.RemoveAllListeners();
        SlotCountSlider.onValueChanged.RemoveAllListeners();
        HealthInputField.onValueChanged.RemoveAllListeners();
        LadderSlider.onValueChanged.RemoveAllListeners();
        InPoolToggle.onValueChanged.RemoveAllListeners();

        bool addressIsNull = address == null;

        EntityDropdown.gameObject.SetActive(!addressIsNull);
        AllowedDifficultySlider.gameObject.SetActive(!addressIsNull);
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
            Encyclopedia.EntityCategory.Do(entityEntry => EntityDropdown.options.Add(new TMP_Dropdown.OptionData(entityEntry.GetName())));
            EntityDropdown.onValueChanged.AddListener(ModelChanged);
        }

        if (AllowedDifficultySlider != null)
            AllowedDifficultySlider.onValuesChanged.AddListener(AllowedDifficultySliderChanged);

        if (JingJieDropdown != null)
        {
            JingJieDropdown.options = new();
            JingJie.Traversal.Do(jingJie => JingJieDropdown.options.Add(new TMP_Dropdown.OptionData(jingJie.GetName())));
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
            FieldView.RightClickNeuron.Join(RightClickSlotNeuron);
            FieldView.DropNeuron.Join(DropSlotNeuron);
        }

        if (FormationListView != null)
            FormationListView.SetAddress(GetAddress().Append(".ShowingFormations"));
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

    private void SetAllowedDifficulty(Bound bound)
    {
        AllowedDifficultySlider.SetValueWithoutNotify(bound.Start, bound.End);
        LowValueLabel.text = bound.Start.ToString();
        HighValueLabel.text = bound.End.ToString();
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
        {
            Blank.SetActive(true);
            return;
        }
        IEntity entity = Get<IEntity>();
        if (entity == null)
        {
            Blank.SetActive(true);
            return;
        }
        Blank.SetActive(false);
        SetEntry(entity.GetModel());
        SetAllowedDifficulty(entity.GetAllowedDifficulty());
        SetJingJie(entity.GetJingJie());
        SetSlotCount(entity.GetSlotCount());
        SetHealth(entity.GetHealth());
        SetLadder(entity.GetLadder());
        SetInPool(entity.IsInPool());
        FieldView.Sync();
        FormationListView.Sync();
    }

    private void ModelChanged(int entityEntryIndex)
    {
        IEntity entity = Get<IEntity>();
        entity.SetModel(Encyclopedia.EntityCategory[entityEntryIndex]);
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
        health = Mathf.Clamp(health, 1, 99999);

        IEntity entity = Get<IEntity>();
        entity.SetHealth(health);
        Refresh();
    }

    private void LadderChanged(float value)
    {
        IEntity entity = Get<IEntity>();
        entity.SetLadder((int)value);
        Refresh();
    }

    private void AllowedDifficultySliderChanged(float lowValue, float highValue)
    {
        IEntity entity = Get<IEntity>();
        entity.SetAllowedDifficulty(new((int)lowValue, (int)highValue));
        Refresh();
    }

    private void InPoolChanged(bool value)
    {
        IEntity entity = Get<IEntity>();
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
