using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArbitraryCardPickerPanel : Panel
{
    public TMP_Text InfoText;
    public TMP_Text StatusText;
    public Button ConfirmButton;

    public SkillInventoryView SkillInventoryView;

    private List<SkillView> _selections;

    private InteractDelegate InteractDelegate;

    private IndexPath _indexPath;

    public override void Configure()
    {
        base.Configure();
        _indexPath = new IndexPath("Run.CurrentNode.CurrentPanel");

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ConfirmSelections);

        ConfigureInteractDelegate();
        _selections = new List<SkillView>();

        SkillInventoryView.Configure(new IndexPath($"{_indexPath}.Inventory"));
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
        ArbitraryCardPickerPanelDescriptor d = runNode.CurrentPanel as ArbitraryCardPickerPanelDescriptor;

        InfoText.text = d.GetDetailedText();
        StatusText.text = $"可以选择 {d.Range.Start} ~ {d.Range.End - 1} 张卡\n已选   {_selections.Count}   张";

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
        ArbitraryCardPickerPanelDescriptor d = runNode.CurrentPanel as ArbitraryCardPickerPanelDescriptor;

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

            RunSkill skill = DataManager.Get<RunSkill>(skillView.GetIndexPath());
            if (!d.CanSelect(skill))
                return false;

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
        ArbitraryCardPickerPanelDescriptor d = runNode.CurrentPanel as ArbitraryCardPickerPanelDescriptor;
        List<RunSkill> mapped = _selections.Map(v => DataManager.Get<RunSkill>(v.GetIndexPath())).ToList();
        d.ConfirmSelections(mapped);
        PanelDescriptor panelDescriptor = RunManager.Instance.Map.ReceiveSignal(new Signal());
        RunCanvas.Instance.SetNodeState(panelDescriptor);
    }
}
