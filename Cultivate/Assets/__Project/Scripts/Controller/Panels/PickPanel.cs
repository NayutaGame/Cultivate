
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;

public class PickPanel : Panel
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
        ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
        PickCell cell = cellAdapter.AsCell() as PickCell;

        DetailedText.text = cell.GetDetailedText() +
                            $"\n可选择{cell.Bound.Start}~{cell.Bound.End}张" +
                            $"\n已选择 {_selections.Count} 张";
        
        ConfirmButton.SetStateToInactiveFrom(!cell.Bound.Contains(_selections.Count));
        
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
            ICellAdapter cellAdapter = _address.Get<ICellAdapter>();
            PickCell cell = cellAdapter.AsCell() as PickCell;
            // SkillEntryDescriptor skill = selectBehaviour.Get<SkillEntryDescriptor>();
            
            if (!cell.HasSpace(_selections.Count))
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
        List<SkillReference> skillReferences = _selections.Map(v => v.Get<SkillReference>()).ToList();
        RunManager.Instance.Environment.ConfirmSelectionsProcedure(skillReferences);
    }
}
