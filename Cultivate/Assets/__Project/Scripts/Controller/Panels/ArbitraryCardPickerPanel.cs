
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArbitraryCardPickerPanel : Panel
{
    public TMP_Text Text1;
    public TMP_Text Text2;
    public TMP_Text Text3;
    public Button ConfirmButton;
    public ListView SkillListView;

    private List<SimpleView> _selections;
    private Address _address;

    public override void Configure()
    {
        base.Configure();
        _address = new Address("Run.Environment.ActivePanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        _selections = new List<SimpleView>();

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

        Text1.text = d.GetDetailedText();
        Text2.text = $"可选择{d.Range.Start}~{d.Range.End - 1}张";
        Text3.text = $"已选择 {_selections.Count} 张";
        ConfirmButton.interactable = d.Range.Contains(_selections.Count);

        SkillListView.Refresh();
    }

    private void ToggleSkill(InteractBehaviour ib, PointerEventData eventData)
        => ToggleSkill(ib.GetSimpleView());

    private bool ToggleSkill(SimpleView v)
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
            int space = d.Range.End - 1 - _selections.Count;
            if (space <= 0)
                return false;

            SkillDescriptor skill = v.Get<SkillDescriptor>();
            if (!d.CanSelect(skill))
                return false;

            v.GetSelectBehaviour().SetSelected(true);
            _selections.Add(v);
        }

        Refresh();

        return true;
    }

    private void PlayCardHoverSFX(InteractBehaviour ib, PointerEventData d)
        => AudioManager.Play("CardHover");

    private void ConfirmSelections()
    {
        List<SkillDescriptor> mapped = _selections.Map(v => v.Get<SkillDescriptor>()).ToList();
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.ReceiveSignalProcedure(new SelectedSkillsSignal(mapped));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
    }
}
