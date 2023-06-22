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
    public int GetIndexOfSkill(RunSkill skill) => _skills.IndexOf(skill);

    private Predicate<SkillEntry> _pred;
    private WuXing? _wuXing;
    private JingJie? _jingJie;

    public DiscoverSkillPanelDescriptor(string detailedText = null, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        _accessors = new()
        {
            { "Skills",                () => _skills },
        };

        _detailedText = detailedText ?? "请选择一张卡作为奖励";

        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie ?? RunManager.Instance.Map.JingJie;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        RunManager.Instance.SkillPool.TryDrawSkills(out _skills, pred: _pred, wuXing: _wuXing, jingJie: _jingJie , count: 3);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            RunManager.Instance.Battle.SkillInventory.AddSkill(_skills[selectedOptionSignal.Selected]);
            RunManager.Instance.Map.TryFinishNode();
            return null;
        }

        return this;
    }
}
