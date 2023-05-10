using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CLLibrary;
using UnityEngine;

[Serializable]
public class SkillSlot : ISkillModel, IDragDrop
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    [SerializeReference] private RunEntity _owner;
    public RunEntity Owner => _owner;
    [SerializeField] private int _index;

    [SerializeReference] private RunSkill _skill;
    public RunSkill Skill
    {
        get => _skill;
        set
        {
            _skill = value?.Clone();
            EnvironmentChanged();
        }
    }

    public bool ShowPreview()
        => _skill != null;

    private bool IsReveal;
    public bool GetReveal()
        => IsReveal;
    public void SetReveal(bool isReveal)
        => IsReveal = isReveal;

    public SkillSlot(RunEntity owner, int index)
    {
        _owner = owner;
        _index = index;
        IsReveal = true;
    }

    public int GetManaCost()
        => _skill?.GetManaCost() ?? 0;

    public Color GetManaCostColor()
        => IsManaShortage ? Color.red : Color.black;

    public string GetManaCostString()
    {
        int manaCost = GetManaCost();
        return manaCost == 0 ? "" : manaCost.ToString();
    }

    public string GetName()
        => _skill?.GetName() ?? "空";

    public string GetAnnotatedDescription(string evaluated = null)
        => _skill?.GetAnnotatedDescription(evaluated);

    public SkillTypeCollection GetSkillTypeCollection()
        => _skill?.GetSkillTypeCollection() ?? SkillTypeCollection.None;

    public Color GetColor()
        => _skill?.GetColor() ?? CanvasManager.Instance.JingJieColors[JingJie.LianQi];

    public Sprite GetCardFace()
        => _skill?.Entry.CardFace;

    public string GetDescription()
        => _skill?.GetDescription();

    public string GetAnnotationText()
        => _skill?.GetAnnotationText();

    public string GetJingJieString()
        => _skill?.GetJingJie().Index.ToString() ?? "null";

    public bool TryIncreaseJingJie()
    {
        if (_skill == null)
            return false;

        JingJie curr = _skill.JingJie;
        JingJie next = curr + 1;
        if (!_skill.Entry.JingJieRange.Contains(next))
            next = _skill.Entry.JingJieRange.Start;
        _skill.JingJie = next;
        EnvironmentChanged();
        return true;
    }

    [NonSerialized] public bool RunConsumed;

    public bool TryConsume()
    {
        if (!RunConsumed)
            return false;

        Skill = null;
        return true;
    }

    [NonSerialized] public bool IsManaShortage;

    #region IDragDrop

    private DragDropDelegate _dragDropDelegate;

    public DragDropDelegate GetDragDropDelegate()
        => _dragDropDelegate;

    public void SetDragDropDelegate(DragDropDelegate dragDropDelegate)
        => _dragDropDelegate = dragDropDelegate;

    #endregion
}
