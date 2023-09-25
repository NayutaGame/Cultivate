
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArbitraryCardPickerPanel : Panel
{
    public TMP_Text InfoText;
    public TMP_Text StatusText;
    public Button ConfirmButton;

    public SkillInventoryView SkillInventoryView;

    private List<SkillView> _selections;

    private InteractDelegate InteractDelegate;

    private Address _address;

    public override void Configure()
    {
        base.Configure();
        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        ConfigureInteractDelegate();
        _selections = new List<SkillView>();

        SkillInventoryView.SetAddress(new Address($"{_address}.Inventory"));
        SkillInventoryView.SetDelegate(InteractDelegate);
    }

    public void OnDisable()
    {
        _selections.Do(v => v.SetSelected(false));
        _selections.Clear();
    }

    public override void Refresh()
    {
        base.Refresh();

        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {_selections.Count}   张";
        ConfirmButton.interactable = d.Range.Contains(_selections.Count);

        SkillInventoryView.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new InteractDelegate(1,
            getId: view => 0
        );

        InteractDelegate.SetHandle(InteractDelegate.POINTER_LEFT_CLICK, 0, (v, d) => ToggleSkill(v, d));
    }

    private bool ToggleSkill(IInteractable view, PointerEventData eventData)
    {
        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();

        SkillView skillView = view as SkillView;
        bool isSelected = _selections.Contains(skillView);

        if (isSelected)
        {
            skillView.SetSelected(false);
            _selections.Remove(skillView);
        }
        else
        {
            int space = d.Range.End - 1 - _selections.Count;
            if (space <= 0)
                return false;

            RunSkill skill = skillView.Get<RunSkill>();
            if (!d.CanSelect(skill))
                return false;

            skillView.SetSelected(true);
            _selections.Add(skillView);
        }

        Refresh();

        return true;
    }

    private void ConfirmSelections()
    {
        ArbitraryCardPickerPanelDescriptor d = _address.Get<ArbitraryCardPickerPanelDescriptor>();
        List<RunSkill> mapped = _selections.Map(v => v.Get<RunSkill>()).ToList();
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new SelectedSkillsSignal(mapped));
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
