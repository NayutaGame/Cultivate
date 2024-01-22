
using System;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    public TMP_Text InfoText;
    public TMP_Text StatusText;
    public Button BackButton;
    public Button ConfirmButton;

    [NonSerialized] public ListView SkillListView;
    [NonSerialized] public ListView SlotListView;

    private List<int> _skillSelections;
    private List<int> _slotSelections;

    private Address _address;

    public override void Configure()
    {
        base.Configure();

        _address = new Address("Run.Environment.ActivePanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        SkillListView = CanvasManager.Instance.RunCanvas.DeckPanel.HandView;
        SlotListView = CanvasManager.Instance.RunCanvas.DeckPanel.FieldView;
        _skillSelections ??= new List<int>();
        _slotSelections ??= new List<int>();
    }

    public void OnDisable()
    {
        ClearAllSelections();
    }

    public void ClearAllSelections()
    {
        if (SkillListView == null)
            return;

        SkillListView.Traversal().Do(itemBehaviour => itemBehaviour.GetSelectBehaviour().SetSelected(false));
        _skillSelections.Clear();
        SlotListView.Traversal().Do(itemBehaviour => itemBehaviour.GetSelectBehaviour().SetSelected(false));
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

    public bool ToggleSkill(InteractBehaviour view)
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();

        SkillView skillView = view.GetComponent<SkillView>();
        int index = SkillListView.ActivePool.FindIndex(itemBehaviour => itemBehaviour.GetSimpleView() == skillView);
        bool isSelected = _skillSelections.Contains(index);

        if (isSelected)
        {
            skillView.SetSelected(false);
            _skillSelections.Remove(index);
        }
        else
        {
            int space = d.Range.End - 1 - SelectionCount;
            if (space <= 0)
                return false;

            RunSkill runSkill = skillView.Get<RunSkill>();
            if (!d.CanSelect(runSkill))
                return false;

            skillView.SetSelected(true);
            _skillSelections.Add(index);
        }

        return true;
    }

    public bool ToggleSkillSlot(InteractBehaviour view)
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();

        SlotCardView slotView = view.GetComponent<SlotCardView>();
        int index = SlotListView.ActivePool.FindIndex(itemBehaviour => itemBehaviour.GetSimpleView() == slotView);
        bool isSelected = _slotSelections.Contains(index);

        if (isSelected)
        {
            slotView.GetSelectBehaviour().SetSelected(false);
            _slotSelections.Remove(index);
        }
        else
        {
            int space = d.Range.End - 1 - SelectionCount;
            if (space <= 0)
                return false;

            SkillSlot slot = slotView.Get<SkillSlot>();
            if (!d.CanSelect(slot))
                return false;

            slotView.GetSelectBehaviour().SetSelected(true);
            _slotSelections.Add(index);
        }

        return true;
    }

    private void ConfirmSelections()
    {
        CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
        List<object> iRunSkillList = new List<object>();
        iRunSkillList.AddRange(_skillSelections.Map(i => SkillListView.ActivePool[i].GetSimpleView().Get<object>()));
        iRunSkillList.AddRange(_slotSelections.Map(i => SlotListView.ActivePool[i].GetSimpleView().Get<object>()));
        PanelDescriptor panelDescriptor = RunManager.Instance.Environment.Map.ReceiveSignal(new SelectedIRunSkillsSignal(iRunSkillList));
        CanvasManager.Instance.RunCanvas.SetNodeState(panelDescriptor);
    }
}
