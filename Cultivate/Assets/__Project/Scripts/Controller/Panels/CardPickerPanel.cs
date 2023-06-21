using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPickerPanel : Panel
{
    public TMP_Text InfoText;
    public TMP_Text StatusText;
    public Button ConfirmButton;

    public EntityView HeroView;
    public SkillInventoryView SkillInventoryView;

    private List<AbstractSkillView> _selections;

    private InteractDelegate InteractDelegate;

    public override void Configure()
    {
        base.Configure();

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        ConfigureInteractDelegate();
        _selections = new List<AbstractSkillView>();

        HeroView.Configure(new IndexPath("Battle.Hero"));
        HeroView.SetDelegate(InteractDelegate);

        SkillInventoryView.Configure(new IndexPath("Battle.SkillInventory"));
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

        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {_selections.Count}   张";

        HeroView.Refresh();
        SkillInventoryView.Refresh();
    }

    private void ConfigureInteractDelegate()
    {
        InteractDelegate = new InteractDelegate(1,
            getId: view => 0,
            lMouseTable: new Func<IInteractable, bool>[]
            {
                ToggleSkill,
            }
        );
    }

    private bool ToggleSkill(IInteractable view)
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;

        AbstractSkillView skillView = view as AbstractSkillView;
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

            object o = RunManager.Get<object>(skillView.GetIndexPath());
            if (o is RunSkill skill)
            {
                if (!d.CanSelect(skill))
                    return false;
            }
            else if (o is SkillSlot slot)
            {
                if (!d.CanSelect(slot))
                    return false;
            }
            else
            {
                return false;
            }

            skillView.SetSelected(true);
            _selections.Add(skillView);
        }

        ConfirmButton.interactable = d.Range.Contains(_selections.Count);
        Refresh();

        return true;
    }

    private void ConfirmSelections()
    {
        RunNode runNode = RunManager.Instance.TryGetCurrentNode();
        CardPickerPanelDescriptor d = runNode.CurrentPanel as CardPickerPanelDescriptor;
        List<object> mapped = _selections.Map(v => RunManager.Get<object>(v.GetIndexPath())).ToList();
        d.ConfirmSelections(mapped);
        RunManager.Instance.Map.ReceiveSignal(new Signal());
    }
}
