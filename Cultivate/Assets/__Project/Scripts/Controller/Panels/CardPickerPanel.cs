
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

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();

        _indexPath = new IndexPath("Run.Battle.Map.CurrentNode.CurrentPanel");

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

        CardPickerPanelDescriptor d = DataManager.Get<CardPickerPanelDescriptor>(_indexPath);

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {SelectionCount}   张";
    }

    private int SelectionCount => _skillSelections.Count + _slotSelections.Count;

    public bool ToggleSkill(IInteractable view)
    {
        CardPickerPanelDescriptor d = DataManager.Get<CardPickerPanelDescriptor>(_indexPath);

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

            RunSkill runSkill = DataManager.Get<RunSkill>(skillView.GetIndexPath());
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
        CardPickerPanelDescriptor d = DataManager.Get<CardPickerPanelDescriptor>(_indexPath);

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

            SkillSlot slot = DataManager.Get<SkillSlot>(slotView.GetIndexPath());
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
        CardPickerPanelDescriptor d = DataManager.Get<CardPickerPanelDescriptor>(_indexPath);
        List<object> iRunSkillList = new List<object>();
        iRunSkillList.AddRange(_skillSelections.Map(v => DataManager.Get<object>(v.GetIndexPath())));
        iRunSkillList.AddRange(_slotSelections.Map(v => DataManager.Get<object>(v.GetIndexPath())));
        d.ConfirmSelections(iRunSkillList);
        PanelDescriptor panelDescriptor = RunManager.Instance.Battle.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
