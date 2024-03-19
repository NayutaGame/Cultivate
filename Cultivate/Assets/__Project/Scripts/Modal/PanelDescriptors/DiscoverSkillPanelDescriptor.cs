using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DiscoverSkillPanelDescriptor : PanelDescriptor
{
    private string _titleText;
    public string GetTitleText() => _titleText;
    public void SetTitleText(string value) => _titleText = value;

    private string _detailedText;
    public string GetDetailedText() => _detailedText;
    public void SetDetailedText(string value) => _detailedText = value;

    private ListModel<RunSkill> _skills;
    public int GetSkillCount() => _skills.Count();
    public RunSkill GetSkill(int i) => _skills[i];
    public int GetIndexOfSkill(RunSkill skill) => _skills.IndexOf(skill);

    private Predicate<SkillEntry> _pred;
    private WuXing? _wuXing;
    private JingJie? _jingJie;

    public DiscoverSkillPanelDescriptor(string titleText = null, string detailedText = null, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        _accessors = new()
        {
            { "Skills",                () => _skills },
        };

        _titleText = titleText ?? "";
        _detailedText = detailedText ?? "请选择一张卡作为奖励";

        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie ?? RunManager.Instance.Environment.Map.GetLevel();
        _skills = new();
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        DiscoverSkillDetails d = new DiscoverSkillDetails(_pred, _wuXing, _jingJie, 3);
        RunManager.Instance.Environment.DiscoverSkillProcedure(d);

        _skills.Clear();
        _skills.AddRange(d.Skills);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            RunManager.Instance.Environment.Hand.Add(_skills[selectedOptionSignal.Selected]);
            // TODO: Discover Animation
            return null;
        }

        return this;
    }
}
