
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArbitraryCardPickerPanel : Panel
{
    public TMP_Text DetailedText;
    public Button ConfirmButton;
    public ListView SkillListView;

    private List<SelectBehaviour> _selections;
    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _address = new Address("Run.Environment.ActivePanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        _selections = new();

        SkillListView.SetAddress(_address.Append(".Inventory"));
        SkillListView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillListView.LeftClickNeuron.Join(ToggleSkill);
    }

    public void OnDisable()
    {
        _selections.Do(b => b.SetSelect(false));
        _selections.Clear();
    }

    public override void Refresh()
    {
        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();

        DetailedText.text = d.GetDetailedText() +
                            $"可选择{d.Bound.Start}~{d.Bound.End - 1}张" +
                            $"已选择 {_selections.Count} 张";
        ConfirmButton.interactable = d.Bound.Contains(_selections.Count);
        
        SkillListView.Refresh();
    }

    private void ToggleSkill(InteractBehaviour ib, PointerEventData eventData)
        => ToggleSkill(ib.GetView().GetBehaviour<SelectBehaviour>());

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
            ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();
            SkillEntryDescriptor skill = selectBehaviour.Get<SkillEntryDescriptor>();
            
            if (!d.CanSelect(skill))
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

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void ConfirmSelections()
    {
        List<SkillEntryDescriptor> descriptors = _selections.Map(v => v.Get<SkillEntryDescriptor>()).ToList();
        RunManager.Instance.Environment.ConfirmSelectionsProcedure(descriptors);
    }
}
