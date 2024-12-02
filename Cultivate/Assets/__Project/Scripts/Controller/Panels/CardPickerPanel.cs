
using System;
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    public TMP_Text TitleText;
    public TMP_Text ContentText;
    public Button ConfirmButton;

    // [NonSerialized] public LegacyListView SkillListView;
    // [NonSerialized] public LegacyListView SlotListView;
    //
    // private List<int> _skillSelections;
    // private List<int> _slotSelections;
    //
    // private Address _address;

    public override void Configure()
    {
        base.Configure();

        // _address = new Address("Run.Environment.ActivePanel");
        //
        // ConfirmButton.onClick.RemoveAllListeners();
        // ConfirmButton.onClick.AddListener(ConfirmSelections);
        //
        // SkillListView = CanvasManager.Instance.RunCanvas.DeckPanel.HandView;
        // SlotListView = CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList;
        // _skillSelections ??= new List<int>();
        // _slotSelections ??= new List<int>();
    }

    private void OnEnable()
    {
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.LeftClickNeuron.Add(ToggleSkill);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.LeftClickNeuron.Add(ToggleSkillSlot);
    }

    public void OnDisable()
    {
        // CanvasManager.Instance.RunCanvas.DeckPanel.HandView.LeftClickNeuron.Remove(ToggleSkill);
        // CanvasManager.Instance.RunCanvas.DeckPanel.PlayerEntity.SkillList.LeftClickNeuron.Remove(ToggleSkillSlot);
        //
        // ClearAllSelections();
    }

    public void ClearAllSelections()
    {
        // if (SkillListView == null)
        //     return;
        //
        // SkillListView.Traversal().Do(itemBehaviour => itemBehaviour.GetSelectBehaviour().SetSelected(false, animated: false));
        // _skillSelections.Clear();
        // SlotListView.Traversal().Do(itemBehaviour => itemBehaviour.GetSelectBehaviour().SetSelected(false, animated: false));
        // _slotSelections.Clear();
    }

    public override void Refresh()
    {
        base.Refresh();

        // CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
        //
        // TitleText.text = d.GetTitleText();
        // ContentText.text = d.GetDetailedText(SelectionCount);
        // ConfirmButton.interactable = d.Bound.Contains(SelectionCount);
    }

    // private int SelectionCount => _skillSelections.Count + _slotSelections.Count;
    //
    // private void ToggleSkill(LegacyInteractBehaviour ib, PointerEventData eventData)
    // {
    //     CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
    //     
    //     LegacyItemBehaviour currItemBehaviour = ib.GetCLView().GetItemBehaviour();
    //     
    //     int index = SkillListView.ActivePool.FindIndex(itemBehaviour => itemBehaviour == currItemBehaviour);
    //     bool isSelected = _skillSelections.Contains(index);
    //
    //     if (isSelected)
    //     {
    //         currItemBehaviour.GetSelectBehaviour().SetSelected(false);
    //         _skillSelections.Remove(index);
    //
    //         CanvasManager.Instance.RunCanvas.Refresh();
    //         // return true;
    //     }
    //     else
    //     {
    //         int space = d.Bound.End - 1 - SelectionCount;
    //         if (space <= 0)
    //             return;
    //             // return false;
    //
    //         RunSkill runSkill = ib.GetSimpleView().Get<RunSkill>();
    //         if (!d.CanSelect(runSkill))
    //             return;
    //             // return false;
    //
    //         currItemBehaviour.GetSelectBehaviour().SetSelected(true);
    //         _skillSelections.Add(index);
    //
    //         CanvasManager.Instance.RunCanvas.Refresh();
    //         // return true;
    //     }
    // }
    //
    // private void ToggleSkillSlot(LegacyInteractBehaviour ib, PointerEventData eventData)
    // {
    //     CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
    //
    //     LegacyItemBehaviour currItemBehaviour = ib.GetCLView().GetItemBehaviour();
    //     int index = SlotListView.ActivePool.FindIndex(itemBehaviour => itemBehaviour == currItemBehaviour);
    //     bool isSelected = _slotSelections.Contains(index);
    //
    //     if (isSelected)
    //     {
    //         currItemBehaviour.GetSelectBehaviour().SetSelected(false);
    //         _slotSelections.Remove(index);
    //
    //         CanvasManager.Instance.RunCanvas.Refresh();
    //         // return true;
    //     }
    //     else
    //     {
    //         int space = d.Bound.End - 1 - SelectionCount;
    //         if (space <= 0)
    //             return;
    //             // return false;
    //
    //         SkillSlot slot = ib.GetSimpleView().Get<SkillSlot>();
    //         if (!d.CanSelect(slot))
    //             return;
    //             // return false;
    //
    //         currItemBehaviour.GetSelectBehaviour().SetSelected(true);
    //         _slotSelections.Add(index);
    //
    //         CanvasManager.Instance.RunCanvas.Refresh();
    //         // return true;
    //     }
    // }
    //
    // private void ConfirmSelections()
    // {
    //     CardPickerPanelDescriptor d = _address.Get<CardPickerPanelDescriptor>();
    //     List<object> iRunSkillList = new List<object>();
    //     iRunSkillList.AddRange(_skillSelections.Map(i => SkillListView.ActivePool[i].GetSimpleView().Get<object>()));
    //     iRunSkillList.AddRange(_slotSelections.Map(i => SlotListView.ActivePool[i].GetSimpleView().Get<object>()));
    //
    //     Signal signal = new ConfirmDeckSignal(iRunSkillList);
    //     CanvasManager.Instance.RunCanvas.SetPanelSAsyncFromSignal(signal);
    // }
}
