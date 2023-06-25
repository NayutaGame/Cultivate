
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

    private List<AbstractSkillView> _skillSelections;
    private List<SlotView> _slotSelections;

    public override void Configure()
    {
        base.Configure();

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        _skillSelections = new List<AbstractSkillView>();
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

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {SelectionCount}   张";
    }

    private int SelectionCount => _skillSelections.Count + _slotSelections.Count;

    public bool ToggleSkill(IInteractable view)
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;

        AbstractSkillView skillView = view as AbstractSkillView;
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

            RunSkill runSkill = RunManager.Get<RunSkill>(skillView.GetIndexPath());
            if (!d.CanSelect(runSkill))
                return false;

            skillView.SetSelected(true);
            _skillSelections.Add(skillView);
        }

        ConfirmButton.interactable = d.Range.Contains(SelectionCount);
        return true;
    }

    public bool ToggleSkillSlot(IInteractable view)
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;

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

            SkillSlot slot = RunManager.Get<SkillSlot>(slotView.GetIndexPath());
            if (!d.CanSelect(slot))
                return false;

            slotView.SetSelected(true);
            _slotSelections.Add(slotView);
        }

        ConfirmButton.interactable = d.Range.Contains(SelectionCount);
        return true;
    }

    private void ConfirmSelections()
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;
        List<object> iRunSkillList = new List<object>();
        iRunSkillList.AddRange(_skillSelections.Map(v => RunManager.Get<object>(v.GetIndexPath())));
        iRunSkillList.AddRange(_slotSelections.Map(v => RunManager.Get<object>(v.GetIndexPath())));
        d.ConfirmSelections(iRunSkillList);
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
