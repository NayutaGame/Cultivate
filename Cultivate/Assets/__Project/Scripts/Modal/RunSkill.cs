using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RunSkill : ISkillModel, IDragDrop
{
    public SkillEntry _entry;
    public JingJie JingJie;
    public int RunUsedTimes { get; protected set; }
    public int RunEquippedTimes { get; protected set; }

    public RunSkill(SkillEntry entry, JingJie jingJie)
    {
        _entry = entry;
        JingJie = jingJie;
    }

    private RunSkill(RunSkill prototype)
    {
        _entry = prototype._entry;
        JingJie = prototype.JingJie;
        RunUsedTimes = prototype.RunUsedTimes;
        RunEquippedTimes = prototype.RunEquippedTimes;
    }

    public string GetName()
        => _entry.Name;

    public string GetAnnotatedDescription(string evaluated = null)
        => _entry.GetAnnotatedDescription(evaluated ?? GetDescription());

    public SkillTypeCollection GetSkillTypeCollection()
        => _entry.SkillTypeCollection;

    public JingJie GetJingJie()
        => JingJie;

    public Color GetColor()
        => CanvasManager.Instance.JingJieColors[GetJingJie()];

    public Sprite GetCardFace()
        => _entry.CardFace;

    public string GetDescription()
        => _entry.Evaluate(JingJie, JingJie - _entry.JingJieRange.Start);

    public bool GetReveal()
        => true;

    public int GetManaCost()
        => _entry.GetManaCost(JingJie, JingJie - _entry.JingJieRange.Start);

    public Color GetManaCostColor()
        => Color.black;

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetAnnotationText()
    {
        StringBuilder sb = new();
        foreach (IAnnotation annotation in _entry.GetAnnotations())
            sb.Append($"<style=\"Highlight\">{annotation.GetName()}</style>  {annotation.GetAnnotatedDescription()}\n");

        return sb.ToString();
    }

    public RunSkill Clone()
        => new(this);

    #region IDragDrop

    private DragDropDelegate _dragDropDelegate;

    public DragDropDelegate GetDragDropDelegate()
        => _dragDropDelegate;

    public void SetDragDropDelegate(DragDropDelegate dragDropDelegate)
        => _dragDropDelegate = dragDropDelegate;

    #endregion
}
