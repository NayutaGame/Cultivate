
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    public TMP_Text InfoText;
    public TMP_Text StatusText;
    public Button ConfirmButton;

    private List<SkillView> _skillSelections;
    private List<SlotView> _slotSelections;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Battle.Map.CurrentNode.CurrentPanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        _skillSelections = new List<SkillView>();
        _slotSelections = new List<SlotView>();
    }

    public void OnDisable()
    {
        _skillSelections.Do(v => v.SetSelected(false));
        _skillSelections.Clear();
        _slotSelections.Do(v => v.SetSelected(false));
        _slotSelections.Clear();
    }

    public override void Refresh()
    {
        base.Refresh();

        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {SelectionCount}   张";
        ConfirmButton.interactable = d.Range.Contains(SelectionCount);
    }

    private int SelectionCount => _skillSelections.Count + _slotSelections.Count;

    public bool ToggleSkill(IInteractable view)
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();

        SkillView skillView = view as SkillView;
        bool isSelected = _skillSelections.Contains(skillView);

        if (isSelected)
        {
            skillView.SetSelected(false);
            _skillSelections.Remove(skillView);
        }
        else
        {
            int space = d.Range.End - 1 - SelectionCount;
            if (space <= 0)
                return false;

            RunSkill runSkill = skillView.GetIndexPath().Get<RunSkill>();
            if (!d.CanSelect(runSkill))
                return false;

            skillView.SetSelected(true);
            _skillSelections.Add(skillView);
        }

        return true;
    }

    public bool ToggleSkillSlot(IInteractable view)
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();

        SlotView slotView = view as SlotView;
        bool isSelected = _slotSelections.Contains(slotView);

        if (isSelected)
        {
            slotView.SetSelected(false);
            _slotSelections.Remove(slotView);
        }
        else
        {
            int space = d.Range.End - 1 - SelectionCount;
            if (space <= 0)
                return false;

            SkillSlot slot = slotView.GetIndexPath().Get<SkillSlot>();
            if (!d.CanSelect(slot))
                return false;

            slotView.SetSelected(true);
            _slotSelections.Add(slotView);
        }

        return true;
    }

    private void ConfirmSelections()
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
        List<object> iRunSkillList = new List<object>();
        iRunSkillList.AddRange(_skillSelections.Map(v => v.Get<object>()));
        iRunSkillList.AddRange(_slotSelections.Map(v => v.Get<object>()));
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new SelectedIRunSkillsSignal(iRunSkillList));
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
