
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    [SerializeField] private ListView Requirements;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button ConfirmButton;
    
    private Address _address;
    
    private List<SelectBehaviour> _selections;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        
        _selections = new();

        _address = new Address("Run.Environment.ActivePanel");
        
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);
    }

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.LeftClickNeuron.Add(ToggleSelection);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.FieldView.LeftClickNeuron.Add(ToggleSelection);
    }

    public void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.DeckPanel.HandView.LeftClickNeuron.Remove(ToggleSelection);
        CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.FieldView.LeftClickNeuron.Remove(ToggleSelection);
        ClearSelections();
    }

    public void ClearSelections()
    {
        if (_selections == null)
            return;
        _selections.Do(s => s.SetSelect(false));
        _selections.Clear();
    }

    public override void Refresh()
    {
        CardPickerCell d = _address.Get<CardPickerCell>();
        
        TitleText.text = d.GetTitleText();
        ContentText.text = d.GetDetailedText(_selections.Count);
        ConfirmButton.interactable = d.Bound.Contains(_selections.Count);
    }
    
    private void ToggleSelection(InteractBehaviour ib, PointerEventData eventData)
    {
        SlotView slotView = ib.GetView() as SlotView;
        SelectBehaviour selectBehaviour = slotView.GetContentView().GetBehaviour<SelectBehaviour>();
        bool isSelected = _selections.Contains(selectBehaviour);
    
        if (isSelected)
        {
            selectBehaviour.SetSelectAsync(false);
            _selections.Remove(selectBehaviour);
        }
        else
        {
            CardPickerCell d = _address.Get<CardPickerCell>();
            object obj = selectBehaviour.Get<object>();
            if (obj is RunSkill skill && !d.CanSelect(skill))
                return;
            else if (obj is SkillSlot slot && !d.CanSelect(slot))
                return;

            if (!d.HasSpace(_selections.Count))
            {
                if (_selections.Count > 0)
                {
                    SelectBehaviour first = _selections[0];
                    first.SetSelectAsync(false);
                    _selections.Remove(first);
                }
                else
                {
                    return;
                }
            }

            selectBehaviour.SetSelectAsync(true);
            _selections.Add(selectBehaviour);
        }
        
        Refresh();
    }
    
    private void ConfirmSelections()
    {
        CardPickerCell d = _address.Get<CardPickerCell>();
        List<DeckIndex> indices = new List<DeckIndex>();
        _selections.Do(selectBehaviour =>
        {
            object obj = selectBehaviour.Get<object>();
            if (obj is RunSkill skill)
                indices.Add(skill.ToDeckIndex());
            else if (obj is SkillSlot slot)
                indices.Add(slot.ToDeckIndex());
        });
        
        RunManager.Instance.Environment.ConfirmDeckSelectionsProcedure(indices);
    }
}
