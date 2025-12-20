
using System;
using UnityEngine;

[RequireComponent(typeof(XView))]
public class HighlightRegistrationBehaviour : XBehaviour
{
    private HighlightBehaviour _highlightBehaviour;

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _highlightBehaviour = GetView().GetBehaviour<HighlightBehaviour>();
    }

    private void OnEnable()
    {
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Add(Highlight);
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Add(Unhighlight);
    }

    private void OnDisable()
    {
        CanvasManager.Instance.RunCanvas.HighlightQualifiersNeuron.Remove(Highlight);
        CanvasManager.Instance.RunCanvas.UnhighlightQualifiersNeuron.Remove(Unhighlight);
    }

    private void Highlight(Predicate<RunSkill> predicate)
    {
        object obj = GetView().Get<object>();
        if (obj is RunSkill runSkill && predicate(runSkill))
        {
            _highlightBehaviour.SetHighlight(true);
        }
        else if (obj is SkillSlot skillSlot && skillSlot.Skill != null && predicate(skillSlot.Skill))
        {
            _highlightBehaviour.SetHighlight(true);
        }
    }

    private void Unhighlight()
    {
        _highlightBehaviour.SetHighlight(false);
    }
}