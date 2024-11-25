
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
    public LegacyListView SkillListView;

    private List<LegacySimpleView> _selections;
    private Address _address;

    public override void Configure()
    {
        base.Configure();
        _address = new Address("Run.Environment.ActivePanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        _selections = new List<LegacySimpleView>();

        SkillListView.SetAddress(_address.Append(".Inventory"));
        SkillListView.PointerEnterNeuron.Join(PlayCardHoverSFX);
        SkillListView.LeftClickNeuron.Join(ToggleSkill);
    }

    public void OnDisable()
    {
        _selections.Do(v => v.GetSelectBehaviour().SetSelected(false));
        _selections.Clear();
    }

    public override void Refresh()
    {
        base.Refresh();

        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();

        DetailedText.text = d.GetDetailedText() +
                            $"可选择{d.Bound.Start}~{d.Bound.End - 1}张" +
                            $"已选择 {_selections.Count} 张";
        ConfirmButton.interactable = d.Bound.Contains(_selections.Count);

        SkillListView.Refresh();
    }

    private void ToggleSkill(LegacyInteractBehaviour ib, PointerEventData eventData)
        => ToggleSkill(ib.GetSimpleView());

    private bool ToggleSkill(LegacySimpleView v)
    {
        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();

        bool isSelected = _selections.Contains(v);

        if (isSelected)
        {
            v.GetSelectBehaviour().SetSelected(false);
            _selections.Remove(v);
        }
        else
        {
            if (!d.HasSpace(_selections.Count))
                return false;

            SkillEntryDescriptor skill = v.Get<SkillEntryDescriptor>();
            if (!d.CanSelect(skill))
                return false;

            v.GetSelectBehaviour().SetSelected(true);
            _selections.Add(v);
        }

        Refresh();

        return true;
    }

    private void PlayCardHoverSFX(LegacyInteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void ConfirmSelections()
    {
        List<SkillEntryDescriptor> mapped = _selections.Map(v => v.Get<SkillEntryDescriptor>()).ToList();
        Signal signal = new ConfirmSkillsSignal(mapped);
        CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    }
}
