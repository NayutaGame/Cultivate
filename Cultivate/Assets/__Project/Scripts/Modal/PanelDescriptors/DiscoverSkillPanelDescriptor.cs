using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DiscoverSkillPanelDescriptor : PanelDescriptor
{
    private string _detailedText;
    public string GetDetailedText() => _detailedText;

    private List<RunSkill> _skills;
    public int GetSkillCount() => _skills.Count;
    public RunSkill GetSkill(int i) => _skills[i];

    private Predicate<SkillEntry> _pred;
    private WuXing? _wuXing;
    private JingJie? _jingJie;

    public DiscoverSkillPanelDescriptor(string detailedText = null, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        _accessors = new()
        {
            { "Skills",                () => _skills },
        };

        _interactDelegate = new InteractDelegate(1,
            getID: item => 0,
            lMouseTable: new Func<IInteractable, bool>[]
            {
                TrySelectOption,
            }
        );

        _detailedText = detailedText ?? "请选择一张卡作为奖励";

        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie ?? RunManager.Instance.Map.JingJie;
    }

    private bool TrySelectOption(IInteractable item)
    {
        int i = _skills.IndexOf(item as RunSkill);
        ReceiveSignal(new SelectedOptionSignal(i));
        // RunCanvas.Instance.NodePanel.Refresh();
        RunCanvas.Instance.SetIndexPathForPreview(null);
        return true;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        RunManager.Instance.SkillPool.TryDrawSkills(out _skills, pred: _pred, wuXing: _wuXing, jingJie: _jingJie , count: 3);
        _skills.Do(s => s.SetInteractDelegate(GetInteractDelegate()));
    }

    public override void DefaultReceiveSignal(Signal signal)
    {
        base.DefaultReceiveSignal(signal);
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            _skills.Do(s => s.SetInteractDelegate(null));
            RunManager.Instance.Battle.SkillInventory.AddSkill(_skills[selectedOptionSignal.Selected]);
            RunManager.Instance.Map.TryFinishNode();
        }
    }

    #region Interact

    private InteractDelegate _interactDelegate;

    public InteractDelegate GetInteractDelegate()
        => _interactDelegate;

    public void SetInteractDelegate(InteractDelegate interactDelegate)
        => _interactDelegate = interactDelegate;

    #endregion
}
