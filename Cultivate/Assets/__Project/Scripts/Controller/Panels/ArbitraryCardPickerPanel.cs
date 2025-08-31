
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class ArbitraryCardPickerPanel : Panel
{
    public TMP_Text DetailedText;
    public Button4State ConfirmButton;
    public ListView SkillListView;

    private List<SelectBehaviour> _selections;
    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Run.Environment.ActivePanel");
        _selections = new();
        SkillListView.SetAddress(_address.Append(".Inventory"));
        SkillListView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillListView.LeftClickNeuron.Join(ToggleSkill);
    }

    private void OnEnable()
    {
        ConfirmButton.LeftClickNeuron.Add(ConfirmSelections);
    }

    public void OnDisable()
    {
        ConfirmButton.LeftClickNeuron.Remove(ConfirmSelections);
        
        _selections.Do(b => b.SetSelect(false));
        _selections.Clear();
    }

    public override void Refresh()
    {
        ArbitraryCardPickerCell d = _address.Get<ArbitraryCardPickerCell>();

        DetailedText.text = d.GetDetailedText() +
                            $"可选择{d.Bound.Start}~{d.Bound.End - 1}张" +
                            $"已选择 {_selections.Count} 张";
        
        ConfirmButton.SetStateToInactiveFrom(!d.Bound.Contains(_selections.Count));
        
        SkillListView.Sync();
    }

    private void ToggleSkill(InteractBehaviour ib, PointerEventData eventData)
    {
        SlotView slotView = ib.GetView() as SlotView;
        ToggleSkill(slotView.GetContentView().GetBehaviour<SelectBehaviour>());
    }

    private void ToggleSkill(SelectBehaviour selectBehaviour)
    {
        bool isSelected = _selections.Contains(selectBehaviour);

        if (isSelected)
        {
            selectBehaviour.SetSelectAsync(false);
            _selections.Remove(selectBehaviour);
        }
        else
        {
            ArbitraryCardPickerCell d = _address.Get<ArbitraryCardPickerCell>();
            // SkillEntryDescriptor skill = selectBehaviour.Get<SkillEntryDescriptor>();
            
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

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void ConfirmSelections(InteractBehaviour ib, PointerEventData d)
    {
        List<SkillEntryDescriptor> descriptors = _selections.Map(v => v.Get<SkillEntryDescriptor>()).ToList();
        RunManager.Instance.Environment.ConfirmSelectionsProcedure(descriptors);
    }
}
