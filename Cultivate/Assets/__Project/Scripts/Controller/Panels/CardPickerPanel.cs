
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    public TMP_Text TitleText;
    public TMP_Text ContentText;
    public Button ConfirmButton;
    
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
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
        
        TitleText.text = d.GetTitleText();
        ContentText.text = d.GetDetailedText(_selections.Count);
        ConfirmButton.interactable = d.Bound.Contains(_selections.Count);
    }
    
    private void ToggleSelection(InteractBehaviour ib, PointerEventData eventData)
    {
        SelectBehaviour selectBehaviour = ib.GetView().GetBehaviour<SelectBehaviour>();
        bool isSelected = _selections.Contains(selectBehaviour);
    
        if (isSelected)
        {
            selectBehaviour.SetSelectAsync(false);
            _selections.Remove(selectBehaviour);
        }
        else
        {
            CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
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
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
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
