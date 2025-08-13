
using System.Collections.Generic;
using CLLibrary;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    [SerializeField] private ListView Requirements;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text ContentText;
    [SerializeField] private Button ConfirmButton;
    
    private Address _address;

    public override void AwakeFunction()
    {
        base.AwakeFunction();

        _address = new Address("Run.Environment.ActivePanel");
        Requirements.SetAddress(_address.Append(".Requirements"));
        // Requirements.DropNeuron.Join(Equip, Swap);
        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);
    }

    public override void Refresh()
    {
        CardPickerCell d = _address.Get<CardPickerCell>();
        Requirements.Sync();
        TitleText.text = d.GetTitleText();
        // ContentText.text = d.GetDetailedText(_selections.Count);
        // ConfirmButton.interactable = d.Bound.Contains(_selections.Count);
    }
    
    private void ConfirmSelections()
    {
        CardPickerCell d = _address.Get<CardPickerCell>();
        List<DeckIndex> indices = new List<DeckIndex>();
        // _selections.Do(selectBehaviour =>
        // {
        //     object obj = selectBehaviour.Get<object>();
        //     if (obj is RunSkill skill)
        //         indices.Add(skill.ToDeckIndex());
        //     else if (obj is SkillSlot slot)
        //         indices.Add(slot.ToDeckIndex());
        // });
        
        RunManager.Instance.Environment.ConfirmDeckSelectionsProcedure(indices);
    }

    // private void Equip(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    // {
    //     RunSkill skill = from.Get<RunSkill>();
    //     SkillSlot slot = to.Get<SkillSlot>();
    //     if (skill == null || slot == null)
    //         return;
    //     
    //     RunManager.Instance.Environment.TryEquipProcedure(skill, slot);
    // }
    //
    // private void Swap(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    // {
    //     SkillSlot fromSlot = from.Get<SkillSlot>();
    //     SkillSlot toSlot = to.Get<SkillSlot>();
    //     if (fromSlot == null || toSlot == null)
    //         return;
    //     
    //     RunManager.Instance.Environment.TrySwapProcedure(fromSlot, toSlot);
    // }
}
